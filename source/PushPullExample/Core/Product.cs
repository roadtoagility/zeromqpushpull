using System;
using System.Collections.Generic;

namespace Core
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IList<Product> RelatedProducts { get; set; }
        public int Quantity { get; set; }

        public Product(Guid id, string name, string desc, decimal price, int quantity, params Product[] products)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = desc;
            RelatedProducts = products;
        }
    }
}