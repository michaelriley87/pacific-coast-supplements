using System.Collections.Generic;

namespace PacificCoastSupplements.Api.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<ProductVariant> Variants { get; set; } = new();
    }
}
