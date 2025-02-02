using System;
using System.Collections.Generic;
using System.Linq;
using Library.Entities;

namespace Library.Service
{
    public class LibraryService
    {
        private List<User> Users { get; } = new List<User>();
        private List<Book> Books { get; } = new List<Book>();

        public void AddUser(User user)
        {
            if (user.UserRole == Role.Admin && HasAdmin())
            {
                Console.WriteLine("Ошибка: в системе уже есть администратор.");
                return;
            }

            Users.Add(user);
            Console.WriteLine($"Пользователь {user.UserName} зарегистрирован.");
        }

        public void RemoveUser(Guid userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Users.Remove(user);
                Console.WriteLine($"Пользователь {user.UserName} удален.");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        public User FindUserByCredentials(string userName, string password)
        {
            return Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }

        public User FindUserById(Guid userId)
        {
            return Users.FirstOrDefault(u => u.Id == userId);
        }

        public void DisplayAllBooks()
        {
            Console.WriteLine("Список всех книг:");
            foreach (var book in Books)
            {
                Console.WriteLine(book);
            }
        }

        public void DisplayAllUsers()
        {
            Console.WriteLine("Список всех пользователей:");
            foreach (var user in Users)
            {
                Console.WriteLine($"{user.UserName} ({user.UserRole})");
            }
        }

        public Book FindBookByTitle(string title)
        {
            return Books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
            Console.WriteLine($"Книга {book.Title} добавлена в библиотеку.");
        }
        
        public bool HasAdmin()
        {
            return Users.Any(u => u.UserRole == Role.Admin);
        }
        
        
        
    }
}