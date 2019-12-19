using System.Collections.Generic;
using Core;

namespace PushPullExample
{
    public class ValidatedProductMessage
    {
        public IList<string> Erros { get; set; }
        public Product Product { get; set; }
        public string ProcessedBy { get; set; }

        public ValidatedProductMessage(Product product, string worker, params string[] erros)
        {
            Erros = erros;
            Product = product;
            ProcessedBy = worker;
        }
    }
}