namespace CarPartsShop.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted {  get; set; } = false;
    }
}
