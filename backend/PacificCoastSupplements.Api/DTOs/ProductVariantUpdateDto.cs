namespace PacificCoastSupplements.Api.DTOs
{
    public class ProductVariantUpdateDto
    {
        public string Size { get; set; } = string.Empty;
        public string Flavor { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
