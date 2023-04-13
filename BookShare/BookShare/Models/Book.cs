using System;

namespace BookShare.Models
{
    public class Book : IComparable<Book>
    {
        public string BookId { get; set; }
        public string Username { get; set; }
        public string Bookname { get; set; }
        public string Details { get; set; }
        public string Contactlink { get; set; }
        public string ContactMethod { get; set; }
        public string ContactIcon { get; set; }
        public string ProfileIcon { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string PublisherGender { get; set; }


        public int CompareTo(Book other)
        {
            
                if (other == null) return 1;
                return string.Compare(this.Bookname, other.Bookname);
            
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Book))
            {
                return false;
            }
            else
            {
                Book other = (Book)obj;
               bool value= this.Bookname == other.Bookname &&
                    this.Details == other.Details &&
                    this.Status == other.Status &&
                    this.ContactMethod==other.ContactMethod&&
                    this.Contactlink==other.Contactlink;
                Console.WriteLine("n" + value + "n");
                return value;
            }
        }
        public override int GetHashCode()
        {
            return (Bookname + Details + Status + ContactMethod + Contactlink).GetHashCode();
        }

    }
}