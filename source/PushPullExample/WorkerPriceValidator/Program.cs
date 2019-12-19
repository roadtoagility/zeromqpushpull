using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NetMQ;
using NetMQ.Sockets;
using PushPullExample;

namespace WorkerPriceValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new Database();
            
            using (var priceValidator = new SubscriberSocket(">tcp://localhost:10000"))
            using (var pricePush = new PushSocket("@tcp://*:11000"))
            {
                priceValidator.Subscribe("product");
                while (true)
                {
                    var topic = priceValidator.ReceiveFrameString();
                    var productTohavePriceValidated = priceValidator.ReceiveFrameString();
                    Console.WriteLine(productTohavePriceValidated);
                    
                    var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productTohavePriceValidated);
                    var databaseProduct = database.Products.SingleOrDefault(x => x.Id == product.Id);
                    ValidatedProductMessage message = null;

                    if (databaseProduct != null)
                    {
                        if (databaseProduct.Price != product.Price)
                        {
                            message = new ValidatedProductMessage(product, "Wrong price, can not be displayed on the site.");
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
                    
                    pricePush.SendFrame(Newtonsoft.Json.JsonConvert.SerializeObject(message));
                }
            }
        }
    }
}