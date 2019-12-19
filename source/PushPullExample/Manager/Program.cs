using System;
using System.Collections.Generic;
using Core;
using NetMQ;
using NetMQ.Sockets;

namespace PushPullExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var manager = new PushSocket("@tcp://*:10000"))
            {
                var command = string.Empty;
            
                while (true)
                {
                    Console.WriteLine("Type 'yes' to start");
                    command = Console.ReadLine();
    
                    if (command.Equals("yes"))
                    {
                        var remoteControl = new Product(new Guid("1EDB76B0-F6C6-41B5-BCAA-CE8D1A18D361"), "Remove Control", "A remote control to rule the world.", 50, 5);
                        var tv = new Product(new Guid("7691B9A6-6B0E-41C7-BA9B-FB2DA66710D6"),"TV", "The best televison in the universe ", 2000, 3, remoteControl);
                        var productToBeValidated = Newtonsoft.Json.JsonConvert.SerializeObject(tv);
                    
                        manager.SendFrame(productToBeValidated);
                    }
                }
            }
        }
    }

    
    
    
}