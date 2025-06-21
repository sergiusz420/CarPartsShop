namespace CarPartsShop.Models
{
    public class CartItem
    {
      public int Id { get; set; }
      public int ProductId { get; set; }
      public int Quantity { get; set; }
      public bool Deleted { get; set; } = false;
    }
}
