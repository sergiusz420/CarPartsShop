using System.Collections.Generic;
using CarPartsShop.Models;


namespace CarPartsShop.Data
{
    public static class TempDb
    {
        public static List<Product> Products = [];
        public static List<ProductCategory> Categories { get; set; } = [];
        public static List<CartItem> CartItems { get; set; } = [];

        public static Customer MockCustomer { get; set; } = new Customer()
        {
            Id = 1,
            FullName = "Jan Kowalski",
            Email = "jan.kowalski@example.com"
        };
    }

}