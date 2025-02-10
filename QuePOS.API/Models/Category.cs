﻿using System.ComponentModel.DataAnnotations;

namespace QuePOS.API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Relationships
    }

}
