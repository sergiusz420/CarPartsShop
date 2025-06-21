namespace CarPartsShop.Models
{
    public class CartItem
    {
      public int Id { get; set; }
      public int ProductId { get; set; }
      public int Quantity { get; set; }
      public bool Deleted { get; set; } = false;
      public string ProductName { get; set; }
      public decimal Price { get; set; }
      public string CustomerEmail { get; set; }
      public bool Processed { get; set; } = false;
    }
}
