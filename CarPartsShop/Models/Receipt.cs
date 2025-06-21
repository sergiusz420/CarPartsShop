namespace CarPartsShop.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public List<CartItem> Items { get; set; } = [];
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
