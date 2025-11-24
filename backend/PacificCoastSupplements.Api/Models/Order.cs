namespace PacificCoastSupplements.Api.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
    }
}
