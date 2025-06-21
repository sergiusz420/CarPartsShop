using System.Collections.Generic;
using CarPartsShop.Models;


namespace CarPartsShop.Data
{
    public static class TempDb
    {
        public static List<Product> Products = new List<Product>();
        public static List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public static List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}