using System;
using System.Collections.Generic;

namespace LibrarySystem
{
    class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get; private set; }

        public Book(int id, string title, string author)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Назва книги не може бути порожньою.");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор книги не може бути порожнім.");
            Id = id;
            Title = title;
            Author = author;
            IsAvailable = true;
        }
        public void MarkAsBorrowed()
        {
            IsAvailable = false;
        }
        public void MarkAsReturned()
        {
            IsAvailable = true;
        }
        public override string ToString()
        {
            string status = IsAvailable ? "доступна" : "в оренді";
            return $"[{Id}] \"{Title}\" - {Author} ({status})";
        }
    }
    class User
    {
        private List<Book> _borrowedBooks = new List<Book>();

        public int Id { get; }
        public string Name { get; }

        public IReadOnlyList<Book> BorrowedBooks
        {
            get { return _borrowedBooks; }
        }

        public User(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Ім'я користувача не може бути порожнім.");

            Id = id;
            Name = name;
        }

        public void AddBook(Book book)
        {
            _borrowedBooks.Add(book);
        }

        public void RemoveBook(Book book)
        {
            _borrowedBooks.Remove(book);
        }

        public bool HasBook(Book book)
        {
            return _borrowedBooks.Contains(book);
        }

        public override string ToString()
        {
            return $"[{Id}] {Name}, книг в оренді: {_borrowedBooks.Count}";
        }
    }

    class Library
    {
        private List<Book> _books = new List<Book>();
        private List<User> _users = new List<User>();

        public void AddBook(Book book)
        {
            if (FindBook(book.Id) != null)
            {
                Console.WriteLine($"Помилка: книга з Id {book.Id} вже існує.");
                return;
            }
            _books.Add(book);
        }

        public void AddUser(User user)
        {
            if (FindUser(user.Id) != null)
            {
                Console.WriteLine($"Помилка: користувач з Id {user.Id} вже існує.");
                return;
            }
            _users.Add(user);
        }

        private Book FindBook(int id)
        {
            foreach (Book book in _books)
            {
                if (book.Id == id)
                    return book;
            }
            return null;
        }

        private User FindUser(int id)
        {
            foreach (User user in _users)
            {
                if (user.Id == id)
                    return user;
            }
            return null;
        }

        public bool IssueBook(int userId, int bookId)
        {
            User user = FindUser(userId);
            if (user == null)
            {
                Console.WriteLine($"Помилка: користувача з Id {userId} не існує.");
                return false;
            }

            Book book = FindBook(bookId);
            if (book == null)
            {
                Console.WriteLine($"Помилка: книги з Id {bookId} не існує.");
                return false;
            }

            if (!book.IsAvailable)
            {
                Console.WriteLine($"Помилка: книга \"{book.Title}\" вже в оренді.");
                return false;
            }

            book.MarkAsBorrowed();
            user.AddBook(book);
            Console.WriteLine($"Успіх: {user.Name} взяв(ла) книгу \"{book.Title}\".");
            return true;
        }

        public bool ReturnBook(int userId, int bookId)
        {
            User user = FindUser(userId);
            if (user == null)
            {
                Console.WriteLine($"Помилка: користувача з Id {userId} не існує.");
                return false;
            }

            Book book = FindBook(bookId);
            if (book == null)
            {
                Console.WriteLine($"Помилка: книги з Id {bookId} не існує.");
                return false;
            }

            if (!user.HasBook(book))
            {
                Console.WriteLine($"Помилка: {user.Name} не брав(ла) книгу \"{book.Title}\".");
                return false;
            }

            book.MarkAsReturned();
            user.RemoveBook(book);
            Console.WriteLine($"Успіх: {user.Name} повернув(ла) книгу \"{book.Title}\".");
            return true;
        }

        public void PrintAllBooks()
        {
            Console.WriteLine("--- Усі книги ---");
            foreach (Book book in _books)
                Console.WriteLine(book);
        }

        public void PrintAllUsers()
        {
            Console.WriteLine("--- Усі користувачі ---");
            foreach (User user in _users)
            {
                Console.WriteLine(user);
                foreach (Book book in user.BorrowedBooks)
                    Console.WriteLine($"     - \"{book.Title}\"");
            }
        }

        public void PrintAvailableBooks()
        {
            Console.WriteLine("--- Доступні книги ---");
            bool found = false;
            foreach (Book book in _books)
            {
                if (book.IsAvailable)
                {
                    Console.WriteLine(book);
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("Немає доступних книг.");
        }

        public void PrintBorrowedBooks()
        {
            Console.WriteLine("--- Книги в оренді ---");
            bool found = false;
            foreach (Book book in _books)
            {
                if (!book.IsAvailable)
                {
                    Console.WriteLine(book);
                    found = true;
                }
            }
            if (!found)
                Console.WriteLine("Жодна книга не в оренді.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Library library = new Library();

            library.AddBook(new Book(1, "Кобзар", "Тарас Шевченко"));
            library.AddBook(new Book(2, "Лісова пісня", "Леся Українка"));
            library.AddBook(new Book(3, "Intermezzo", "Михайло Коцюбинський"));

            library.AddUser(new User(1, "Артем"));
            library.AddUser(new User(2, "Олена"));

            library.PrintAvailableBooks();
            Console.WriteLine();

            library.IssueBook(1, 1);
            Console.WriteLine();

            library.IssueBook(2, 1);
            Console.WriteLine();


            library.IssueBook(1, 99);
            library.IssueBook(99, 2);
            library.ReturnBook(2, 1);
            Console.WriteLine();
            library.PrintBorrowedBooks();
            library.PrintAllUsers();
            Console.WriteLine();

            library.ReturnBook(1, 1);
            Console.WriteLine();

            library.PrintAvailableBooks();
            library.PrintAllBooks();
        }
    }
}