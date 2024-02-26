using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Brands
    {
        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public int CategoryId { get; set;}
       

    }
}
