namespace PacificCoastSupplements.Api.Models
{
    public class ProductVariant
    {
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string Size { get; set; } = string.Empty;
        public string Flavor { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
