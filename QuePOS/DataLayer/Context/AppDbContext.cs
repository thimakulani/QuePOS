using System;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public sealed class AppDbContext : DbContext
    {
        // Her auto code generation yapıldığında alttaki veritabanı bağlantı adresi değiştirilmelidir.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Add Here MsSql Connection Address");
        }

        public DbSet<Order> Orders { get; set; }
    }
}