using System.Collections.Generic;

namespace Core
{
    public class ValidatedProduct
    {
        public Product Product { get; set; }
        public IList<string> ValidationMessages { get; set; }
        public bool IsPriceValidated { get; set; }
        public bool IsProductValidated { get; set; }

        public ValidatedProduct()
        {
            ValidationMessages = new List<string>();
        }
    }
}