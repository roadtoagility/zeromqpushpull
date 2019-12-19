using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NetMQ;
using NetMQ.Sockets;
using PushPullExample;

namespace Finisher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var priceValidator = new PullSocket("@tcp://localhost:11000"))
            {
                while (true)
                {
                    var message = priceValidator.ReceiveFrameString();
                    var productMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidatedProductMessage>(message);
                    
                    Console.WriteLine($"Product validated: {productMessage.Product.Name}");
                    Console.WriteLine($"Worker id: {productMessage.ProcessedBy}");
                    Console.WriteLine("Erros:");
                    
                    foreach (var error in productMessage.Erros)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }
    }
}