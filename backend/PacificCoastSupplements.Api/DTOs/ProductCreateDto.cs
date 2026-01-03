using System.ComponentModel.DataAnnotations;

namespace PacificCoastSupplements.Api.DTOs
{
    public class ProductCreateDto : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        public List<ProductVariantCreateDto> Variants { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Variants == null || Variants.Count == 0)
            {
                yield return new ValidationResult(
                    "Product must have at least one variant.",
                    new[] { nameof(Variants) }
                );
            }
        }
    }
}
