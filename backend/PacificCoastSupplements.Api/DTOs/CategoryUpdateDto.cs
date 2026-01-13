using System.ComponentModel.DataAnnotations;

namespace PacificCoastSupplements.Api.DTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
