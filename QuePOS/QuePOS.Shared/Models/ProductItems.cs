using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuePOS.Shared.Models
{
    public class ProductItems
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Bc { get; set; }
        [StringLength(100)]
        public string Base_unit_bc { get; set; }
        [StringLength(100)]
        public string Mp_prod_id { get; set; }
        [StringLength(100)]
        public string Scat { get; set; }
        [StringLength(100)]
        public string Cat { get; set; }
        [StringLength(100)]
        public string Seg1 { get; set; }
        [StringLength(100)]
        public string Seg2 { get; set; }
        [StringLength(100)]
        public string Seg3 { get; set; }
        [StringLength(100)]
        public string Pac { get; set; }
        [StringLength(100)]
        public string Var { get; set; }
        [StringLength(100)]
        public string Man { get; set; }
        [StringLength(100)]
        public string Brn { get; set; }
        [StringLength(100)]
        public string Sub_brn { get; set; }
        public string Desc_sourced { get; set; }
        public string Desc_algo { get; set; }
        public int? Base_unit_count { get; set; }
        public double? Vol_base_unit { get; set; }
        public double? Tot_vol { get; set; }
        [StringLength(100)]
        public string Uom { get; set; }
    }
}
