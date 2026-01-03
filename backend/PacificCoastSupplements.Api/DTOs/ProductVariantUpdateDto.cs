using System.ComponentModel.DataAnnotations;

namespace PacificCoastSupplements.Api.DTOs
{
    public class ProductVariantUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Size { get; set; } = string.Empty;
        [StringLength(50)]
        public string Flavor { get; set; } = string.Empty;
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
