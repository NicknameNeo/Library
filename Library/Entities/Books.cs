using System;


namespace Library.Entities
{
    
    
    public class Book
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? DueDate { get; set; }
        public User Borrower { get; set; }

        public Book(string title, string author, string genre, int year)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
        }

        public override string ToString()
        {
            return $"{Title} by {Author} ({Year}), Genre: {Genre}, Available: {IsAvailable}";
        }
    }
    
}