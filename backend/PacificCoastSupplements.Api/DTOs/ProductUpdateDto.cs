using System.ComponentModel.DataAnnotations;

namespace PacificCoastSupplements.Api.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}
