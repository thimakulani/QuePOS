using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.API.Data
{
    public class POSDbContext : IdentityDbContext
    {
        public POSDbContext(DbContextOptions<POSDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Define DbSets for each entity
        public DbSet<StoreUser> StoreUsers { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
       // public DbSet<Customer> Customers { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Store> Stores { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
                var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent;
                var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var entries = ChangeTracker.Entries().ToList(); // Create a copy of the entries collection
                foreach (var entry in entries)
                {
                    var entity = entry.Entity;
                    var action = entry.State switch
                    {
                        EntityState.Added => "Added",
                        EntityState.Modified => "Modified",
                        EntityState.Deleted => "Deleted",
                        _ => null
                    };

                    if (action != null)
                    {
                        var auditLog = new AuditTrail
                        {
                            UserId = userId,
                            Action = action,
                            TableName = entity.GetType().Name,
                            RecordId = GetRecordId(entity),
                            Timestamp = DateTime.UtcNow,
                            Details = $"{action}",
                            IPAddress = ipAddress,
                            UserAgent = userAgent
                        };

                        AuditTrails.Add(auditLog);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private static int GetRecordId(object entity)
        {
            var entityType = entity.GetType();

            var idProperty = entityType.GetProperty("Id");

            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity);

                if (idValue != null && int.TryParse(idValue.ToString(), out int id))
                {
                    return id;
                }
            }
            return 0;
        }
    }
}
