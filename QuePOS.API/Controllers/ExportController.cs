using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using OfficeOpenXml;
using System.Security.Claims;
namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly POSDbContext _context;

        public ExportController(POSDbContext context)
        {
            _context = context;
        }

        [HttpGet("export_to_excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            var data = _context.Sales.Include(x => x.SaleDetails).Where(x => x.StoreID == user.StoreID).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SalesData");
                worksheet.Cells.LoadFromCollection(data, true);

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesData.xlsx");
            };
        }
    }
}
