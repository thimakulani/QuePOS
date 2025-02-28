using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Interfaces;
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
        public POSDbContext(DbContextOptions<POSDbContext> options, IHttpContextAccessor httpContextAccessor, /*IRepository<StoreUser> repository,*/ IServiceProvider serviceProvider) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            // _repository = repository;
            _serviceProvider = serviceProvider;
        }

        // Define DbSets for each entity
        public DbSet<ProductItems> ProductItems { get; set; }
        public DbSet<StoreUser> StoreUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<Store> Stores { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IRepository<StoreUser> _repository;
        private readonly IServiceProvider _serviceProvider;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
                var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
                var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Ensure store is not null
                var scope = _serviceProvider.CreateScope();
                var _repository = scope.ServiceProvider.GetRequiredService<IRepository<StoreUser>>();
                var store = await _repository.GetFirstOrDefault(x => x.UserId == userId);

                var storeId = store?.Id ?? 0; // Handle null store

                var entries = ChangeTracker.Entries().ToList();
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
                            UserAgent = userAgent,
                            StoreId = storeId,
                        };

                        await AuditTrails.AddAsync(auditLog, cancellationToken); // Use async AddAsync
                    }
                }

                return await base.SaveChangesAsync(cancellationToken); // Ensure database changes are saved
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw; // Re-throw to ensure errors propagate
            }
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
