using System.Collections.Generic;

namespace BookShare.ViewModels
{
    internal class SearchViewModel
    {
        public List<BookCardViewModel> BookCards { get; set; }

        public SearchViewModel()
        {
            // Dummy data for testing
            BookCards = new List<BookCardViewModel>
            {
                new BookCardViewModel
                {
                    ImagePath = "image 1.jpg",
                    Name = "John Smith",
                    BookName = "The Great Gatsby",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante."
                },
                new BookCardViewModel
                {
                    ImagePath = "image2.jpg",
                    Name = "Jane Doe",
                    BookName = "To Kill a Mockingbird",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante."
                },
                new BookCardViewModel
                {
                    ImagePath = "image3.jpg",
                    Name = "Bob Johnson",
                    BookName = "1984",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante."
                },
                                new BookCardViewModel
                {
                    ImagePath = "image 1.jpg",
                    Name = "John Smith",
                    BookName = "The Great Gatsby",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante."
                },                new BookCardViewModel
                {
                    ImagePath = "image 1.jpg",
                    Name = "John Smith",
                    BookName = "The Great Gatsby",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante."
                },                new BookCardViewModel
                {
                    ImagePath = "image 1.jpg",
                    Name = "John Smith",
                    BookName = "The Great Gatsby",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer posuere erat a ante."
                },
            };
        }
    }
}
