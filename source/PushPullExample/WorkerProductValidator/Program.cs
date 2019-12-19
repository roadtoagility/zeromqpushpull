using System;
using System.Linq;
using Core;
using NetMQ;
using NetMQ.Sockets;
using PushPullExample;

namespace WorkerProductValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new Database();
            
            using (var priceValidator = new SubscriberSocket(">tcp://localhost:10000"))
            using (var productPush = new PushSocket("@tcp://*:12000"))
            {
                priceValidator.Subscribe("product");
                while (true)
                {
                    var topic = priceValidator.ReceiveFrameString();
                    var productTohavePriceValidated = priceValidator.ReceiveFrameString();
                    var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productTohavePriceValidated);
                    Console.WriteLine(productTohavePriceValidated);
                    
                    var databaseProduct = database.Products.SingleOrDefault(x => x.Id == product.Id);
                    ValidatedProductMessage message = null;

                    if (databaseProduct != null)
                    {
                        if (string.IsNullOrEmpty(product.Name))
                        {
                            message = new ValidatedProductMessage(product, "Product name is required, can not be displayed on the site.");
                        }else if (string.IsNullOrEmpty(product.Description))
                        {
                            message = new ValidatedProductMessage(product, "Product description is required, can not be displayed on the site.");
                        }else if (product.Price > 1000 && product.RelatedProducts.Count == 0)
                        {
                            message = new ValidatedProductMessage(product, "Expensive products must to have at least 1 related product, can not be displayed on the site.");
                        }
                        else
                        {
                            message = new ValidatedProductMessage(product); 
                        }
                    }
                    else
                    {
                        message = new ValidatedProductMessage(null, "Product not found, need to be created by the Product Team before be published on site");
                    }
                    
                    productPush.SendFrame(Newtonsoft.Json.JsonConvert.SerializeObject(message));
                }
            }
        }
    }
}