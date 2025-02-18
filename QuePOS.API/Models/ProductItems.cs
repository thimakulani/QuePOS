using System.ComponentModel.DataAnnotations;

namespace QuePOS.API.Models
{
    public class ProductItems
    {
        [Key]
        public int Id { get; set; }
        public string Bc { get; set; }
        public string Base_unit_bc { get; set; }
        public string Mp_prod_id { get; set; }
        public string Scat { get; set; }
        public string Cat { get; set; }
        public string Seg1 { get; set; }
        public string Seg2 { get; set; }
        public string Seg3 { get; set; }
        public string Pac { get; set; }
        public string Var { get; set; }
        public string Man { get; set; }
        public string brn { get; set; }
        public string Sub_brn { get; set; }
        public string Desc_sourced { get; set; }
        public string Desc_algo { get; set; }
        public int? Base_unit_count { get; set; }
        public double? Vol_base_unit { get; set; }
        public double? Tot_vol { get; set; }
        public string Uom { get; set; }
    }
}
