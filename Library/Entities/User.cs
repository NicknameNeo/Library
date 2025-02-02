using System;
using System.Collections.Generic;

namespace Library.Entities
{
    public class User
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public Role UserRole { get; set; }
        public List<Book> BorrowedBooks { get; } = new List<Book>();

        public User(string userName, string password, string email, string phone, Role userRole)
        {
            UserName = userName;
            Password = password;
            Email = email;
            Phone = phone;
            UserRole = userRole;
        }

        public void BorrowBook(Book book)
        {
            if (book.IsAvailable)
            {
                book.IsAvailable = false;
                book.Borrower = this;
                book.DueDate = DateTime.Now.AddDays(14); 
                BorrowedBooks.Add(book);
                Console.WriteLine($"{UserName} взял книгу: {book.Title}");
            }
            else
            {
                Console.WriteLine($"Книга {book.Title} недоступна.");
            }
        }

        public void ReturnBook(Book book)
        {
            if (BorrowedBooks.Contains(book))
            {
                book.IsAvailable = true;
                book.Borrower = null;
                book.DueDate = null;
                BorrowedBooks.Remove(book);
                Console.WriteLine($"{UserName} вернул книгу: {book.Title}");
            }
            else
            {
                Console.WriteLine($"Книга {book.Title} не была взята этим пользователем.");
            }
        }
    }

    public enum Role
    {
        Admin,
        Reader
    }
}