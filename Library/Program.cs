using System;
using Library.Entities;
using Library.Service;

namespace Library
{
    class Program
    {
        static void InitializeAdmin()
        {
            if (!_libraryService.HasAdmin())
            {
                var admin = new User("Aslam", "123", "admin@library.com", "123456789", Role.Admin);
                _libraryService.AddUser(admin);
                Console.WriteLine("Создан начальный администратор: Aslam, пароль: 123");
            }
        }

        private static User _currentUser;
        private static LibraryService _libraryService = new LibraryService();

        static void Main(string[] args)
        {
            bool isRunning = true;

            InitializeAdmin();

            {
                while (isRunning)
                {
                    Console.WriteLine("\n--- Добро пожаловать в библиотеку ---");
                    Console.WriteLine("1. Регистрация");
                    Console.WriteLine("2. Войти");
                    Console.WriteLine("3. Войти как админ");
                    Console.WriteLine("4. Выйти");
                    Console.Write("Выберите действие: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Register();
                            break;

                        case "2":
                            Login(Role.Reader); 
                            break;

                        case "3":
                            Login(Role.Admin); 
                            break;

                        case "4":
                            isRunning = false;
                            Console.WriteLine("Программа завершена.");
                            break;

                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }

                    
                    while (_currentUser != null)
                    {
                        if (_currentUser.UserRole == Role.Admin)
                        {
                            ShowAdminMenu();
                        }
                        else
                        {
                            ShowReaderMenu();
                        }

                        string menuChoice = Console.ReadLine();

                        switch (menuChoice)
                        {
                            case "1":
                                if (_currentUser.UserRole == Role.Admin)
                                    AddUser();
                                else
                                    BorrowBook();
                                break;

                            case "2":
                                if (_currentUser.UserRole == Role.Admin)
                                    RemoveUser();
                                else
                                    ReturnBook();
                                break;

                            case "3":
                                if (_currentUser.UserRole == Role.Admin)
                                    AddBook();
                                else
                                    _libraryService.DisplayAllBooks();
                                break;

                            case "4":
                                if (_currentUser.UserRole == Role.Admin)
                                    _libraryService.DisplayAllUsers();
                                else
                                    Logout();
                                break;

                            case "5":
                                if (_currentUser.UserRole == Role.Admin)
                                    BorrowBook();
                                else
                                    isRunning = false;
                                break;

                            case "6":
                                if (_currentUser.UserRole == Role.Admin)
                                    ReturnBook();
                                break;

                            case "7":
                                if (_currentUser.UserRole == Role.Admin)
                                    Logout();
                                break;

                            case "8":
                                if (_currentUser.UserRole == Role.Reader)
                                    isRunning = false;
                                break;

                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }

                        
                        if (_currentUser == null)
                        {
                            break;
                        }
                    }
                }
            }

            static void ShowAdminMenu()
            {
                Console.WriteLine("\n--- Меню администратора ---");
                Console.WriteLine("1. Добавить пользователя");
                Console.WriteLine("2. Удалить пользователя");
                Console.WriteLine("3. Добавить книгу");
                Console.WriteLine("4. Показать всех пользователей");
                Console.WriteLine("5. Выдать книгу пользователю");
                Console.WriteLine("6. Вернуть книгу");
                Console.WriteLine("7. Выйти в главное меню");
                Console.WriteLine("8. Выйти из программы");
                Console.Write("Выберите действие: ");
            }

            static void ShowReaderMenu()
            {
                Console.WriteLine("\n--- Меню читателя ---");
                Console.WriteLine("1. Взять книгу");
                Console.WriteLine("2. Вернуть книгу");
                Console.WriteLine("3. Показать все книги");
                Console.WriteLine("4. Выйти в главное меню");
                Console.WriteLine("5. Выйти из программы");
                Console.Write("Выберите действие: ");
            }

            static void Register()
            {
                Console.WriteLine("\n--- Регистрация ---");
                Console.Write("Введите имя пользователя: ");
                string userName = Console.ReadLine();

                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

                Console.Write("Введите email: ");
                string email = Console.ReadLine();

                Console.Write("Введите телефон: ");
                string phone = Console.ReadLine();

               
                var user = new User(userName, password, email, phone, Role.Reader);
                _libraryService.AddUser(user);
                Console.WriteLine("Регистрация успешна. Теперь войдите в систему.");
            }

            static void Login(Role expectedRole)
            {
                Console.WriteLine("\n--- Вход ---");
                Console.Write("Введите имя пользователя: ");
                string userName = Console.ReadLine();

                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

               
                _currentUser = _libraryService.FindUserByCredentials(userName, password);

                if (_currentUser != null && _currentUser.UserRole == expectedRole)
                {
                    Console.WriteLine($"Добро пожаловать, {_currentUser.UserName}!");
                }
                else
                {
                    Console.WriteLine("Неверные имя пользователя, пароль или роль.");
                    _currentUser = null; 
                }
            }

            static void Logout()
            {
                _currentUser = null; 
                Console.WriteLine("Вы вышли из системы.");
            }

            static void AddUser()
            {
                if (_currentUser.UserRole != Role.Admin)
                {
                    Console.WriteLine("Ошибка: только администратор может добавлять пользователей.");
                    return;
                }

                Console.Write("Введите имя пользователя: ");
                string userName = Console.ReadLine();

                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

                Console.Write("Введите email: ");
                string email = Console.ReadLine();

                Console.Write("Введите телефон: ");
                string phone = Console.ReadLine();

                Console.Write("Введите роль (Admin/Reader): ");
                if (Enum.TryParse(Console.ReadLine(), out Role userRole))
                {
                    var user = new User(userName, password, email, phone, userRole);
                    _libraryService.AddUser(user);
                }
                else
                {
                    Console.WriteLine("Неверная роль. Пользователь не добавлен.");
                }
            }

            static void RemoveUser()
            {
                if (_currentUser.UserRole != Role.Admin)
                {
                    Console.WriteLine("Ошибка: только администратор может удалять пользователей.");
                    return;
                }

                Console.Write("Введите имя пользователя для удаления: ");
                string userName = Console.ReadLine();

                _libraryService.RemoveUser(userName);
            }

            static void AddBook()
            {
                if (_currentUser.UserRole != Role.Admin)
                {
                    Console.WriteLine("Ошибка: только администратор может добавлять книги.");
                    return;
                }

                Console.Write("Введите название книги: ");
                string title = Console.ReadLine();

                Console.Write("Введите автора: ");
                string author = Console.ReadLine();

                Console.Write("Введите жанр: ");
                string genre = Console.ReadLine();

                Console.Write("Введите год издания: ");
                if (int.TryParse(Console.ReadLine(), out int year))
                {
                    var book = new Book(title, author, genre, year);
                    _libraryService.AddBook(book);
                }
                else
                {
                    Console.WriteLine("Неверный формат года.");
                }
            }

            static void BorrowBook()
            {
                if (_currentUser.UserRole == Role.Admin)
                {
                    Console.Write("Введите ID пользователя: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid userId))
                    {
                        var user = _libraryService.FindUserById(userId);
                        if (user == null)
                        {
                            Console.WriteLine("Пользователь не найден.");
                            return;
                        }

                        Console.Write("Введите название книги: ");
                        string title = Console.ReadLine();
                        var book = _libraryService.FindBookByTitle(title);

                        if (book == null)
                        {
                            Console.WriteLine("Книга не найдена.");
                            return;
                        }

                        user.BorrowBook(book);
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат ID пользователя.");
                    }
                }
                else
                {

                    Console.Write("Введите название книги: ");
                    string title = Console.ReadLine();
                    var book = _libraryService.FindBookByTitle(title);

                    if (book == null)
                    {
                        Console.WriteLine("Книга не найдена.");
                        return;
                    }

                    _currentUser.BorrowBook(book);
                }
            }

            static void ReturnBook()
            {
                if (_currentUser.UserRole == Role.Admin)
                {
                    Console.Write("Введите ID пользователя: ");
                    if (Guid.TryParse(Console.ReadLine(), out Guid userId))
                    {
                        var user = _libraryService.FindUserById(userId);
                        if (user == null)
                        {
                            Console.WriteLine("Пользователь не найден.");
                            return;
                        }

                        Console.Write("Введите название книги: ");
                        string title = Console.ReadLine();
                        var book = _libraryService.FindBookByTitle(title);

                        if (book == null)
                        {
                            Console.WriteLine("Книга не найдена.");
                            return;
                        }

                        user.ReturnBook(book);
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат ID пользователя.");
                    }
                }
                else
                {

                    Console.Write("Введите название книги: ");
                    string title = Console.ReadLine();
                    var book = _libraryService.FindBookByTitle(title);

                    if (book == null)
                    {
                        Console.WriteLine("Книга не найдена.");
                        return;
                    }

                    _currentUser.ReturnBook(book);
                }
            }

        }
    }
}