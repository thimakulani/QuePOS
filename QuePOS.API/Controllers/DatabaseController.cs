using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DatabaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("drop-all-tables")]
        public async Task<IActionResult> DropAllTables(string Password)
        {
            if (Password == "22147674")
            {
                try
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        string dropTablesQuery = @"
                    DROP TABLE IF EXISTS dbo.AspNetUsers;
                    DROP TABLE IF EXISTS dbo.AspNetUserRoles;
                    DROP TABLE IF EXISTS dbo.AspNetUserClaims;
                    DROP TABLE IF EXISTS dbo.AspNetRoleClaims;
                    DROP TABLE IF EXISTS dbo.Stores;
                    DROP TABLE IF EXISTS dbo.StoreUsers;
                    DROP TABLE IF EXISTS dbo.Categories;
                    DROP TABLE IF EXISTS dbo.Products;
                    DROP TABLE IF EXISTS dbo.SaleDetails;
                    DROP TABLE IF EXISTS dbo.Sales;
                    DROP TABLE IF EXISTS dbo.AspNetRoles;
                    DROP TABLE IF EXISTS dbo.AuditTrails;
                    DROP TABLE IF EXISTS dbo.__EFMigrationsHistory;
                ";

                        using (SqlCommand command = new SqlCommand(dropTablesQuery, conn))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    return Ok(new { message = "All tables dropped successfully!" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error dropping tables", error = ex.Message });
                }
            }
            else
            {
                return BadRequest("INVALID KILL SWITCH");
            }
        }
    }
}
