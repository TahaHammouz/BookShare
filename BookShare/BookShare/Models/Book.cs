using System;
namespace BookShare.Models
{
    public class Book
    {
        public Book()
        {
        }
        public string Username { get; set; }
        public string Bookname { get; set; }
        public string Details { get; set; }
        public string Contactlink { get; set; }
        public string ContactMethod { get; set; }
        public string ContactIcon { get; set; }
        public string Status { get; set; }
    }
}