using System;

namespace WebApplication2.Models
{
    public class Products
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
        public string ProductDescription { get; set;}

        public decimal Price { get; set;}

        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
