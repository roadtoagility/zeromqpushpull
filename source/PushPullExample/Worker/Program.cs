using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NetMQ;
using NetMQ.Sockets;
using PushPullExample;

namespace Worker
{
    class Program
    {
        private static Database Database = new Database();
        
        static void Main(string[] args)
        {
            var identity = Guid.NewGuid().ToString();
            
            using (var receiver = new PullSocket(">tcp://localhost:10000"))
            using (var validationPush = new PushSocket(">tcp://localhost:11000"))
            {
                while (true)
                {
                    var productTohavePriceValidated = receiver.ReceiveFrameString();
                    Console.WriteLine(productTohavePriceValidated);
                    
                    var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productTohavePriceValidated);
                    
                    var validationMessages = ValidateProduct(product);
                    validationMessages.AddRange(ValidatePrice(product));

                    var message = new ValidatedProductMessage(product, identity,validationMessages.ToArray());
                    
                    validationPush.SendFrame(Newtonsoft.Json.JsonConvert.SerializeObject(message));
                }
            }
        }

        private static List<string> ValidateProduct(Product product)
        {
            var databaseProduct = Database.Products.SingleOrDefault(x => x.Id == product.Id);
            var message = new List<string>();

            if (databaseProduct != null)
            {
                if (string.IsNullOrEmpty(product.Name))
                {
                    message.Add("Product name is required, can not be displayed on the site.");
                }else if (string.IsNullOrEmpty(product.Description))
                {
                    message.Add( "Product description is required, can not be displayed on the site.");
                }else if (product.Price > 1000 && product.RelatedProducts.Count == 0)
                {
                    message.Add( "Expensive products must to have at least 1 related product, can not be displayed on the site.");
                }
            }
            else
            {
                message.Add("Product not found, need to be created by the Product Team before be published on site");
            }

            return message;
        }

        private static List<string> ValidatePrice(Product product)
        {
            var message = new List<string>();
            var databaseProduct = Database.Products.SingleOrDefault(x => x.Id == product.Id);

            if (databaseProduct != null)
            {
                if (databaseProduct.Price != product.Price)
                {
                    message.Add("Wrong price, can not be displayed on the site.");
                }
            }
            else
            {
                message.Add("Product not found, need to be created by the Product Team before be published on site");
            }

            return message;
        }
    }
}