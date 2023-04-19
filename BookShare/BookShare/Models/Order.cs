using System;
namespace BookShare.Models
{
    public class Order
    {
        public string Bookname { get; set; }
        public string BookPrice { get; set; }
        public string URL { get; set; }
        public string ShortenedName { get; set; }
        public string Dimensions { get; set; }
        public string NumberOfPages { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
    }
}