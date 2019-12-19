using System;
using System.Collections.Generic;

namespace Core
{
    public class Database
    {
        public IList<Product> Products { get; private set; }
        
        public Database()
        {
            Products = GetProducts();
        }
        
        private static IList<Product> GetProducts()
        {
            var remoteControl = new Product(new Guid("1EDB76B0-F6C6-41B5-BCAA-CE8D1A18D361"), "Remove Control", "A remote control to rule the world.", 30, 5);
            var tv = new Product(new Guid("7691B9A6-6B0E-41C7-BA9B-FB2DA66710D6"),"TV", "The best televison in the universe ", 1500, 3, remoteControl);
            
            return new List<Product>(){remoteControl, tv};
        }
    }
}