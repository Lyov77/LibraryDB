using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDB
{
    internal class Library
    {
        private readonly string connectionString;

        public Library(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddBookAndAuthor(string bookTitle, string authorName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the author already exists
                int authorId;
                using (SqlCommand authorCommand = new SqlCommand("SELECT AuthorID FROM Authors WHERE AuthorName = @AuthorName", connection))
                {
                    authorCommand.Parameters.AddWithValue("@AuthorName", authorName);
                    authorId = (int?)authorCommand.ExecuteScalar() ?? 0;
                }

                // If the author doesn't exist, add them
                if (authorId == 0)
                {
                    using (SqlCommand addAuthorCommand = new SqlCommand("INSERT INTO Authors (AuthorName) VALUES (@AuthorName); SELECT SCOPE_IDENTITY();", connection))
                    {
                        addAuthorCommand.Parameters.AddWithValue("@AuthorName", authorName);
                        authorId = Convert.ToInt32(addAuthorCommand.ExecuteScalar());
                    }
                }

                // Add the book
                using (SqlCommand addBookCommand = new SqlCommand("INSERT INTO Books (Title, AuthorID) VALUES (@Title, @AuthorID)", connection))
                {
                    addBookCommand.Parameters.AddWithValue("@Title", bookTitle);
                    addBookCommand.Parameters.AddWithValue("@AuthorID", authorId);
                    addBookCommand.ExecuteNonQuery();
                }
                Console.WriteLine($"Book '{bookTitle} by {authorName}' added!");
            }

        }

        public void FindAndDisplayBook(string bookTitle)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Books.Title, Authors.AuthorName FROM Books JOIN Authors ON Books.AuthorID = Authors.AuthorID WHERE Books.Title = @Title", connection))
                {
                    command.Parameters.AddWithValue("@Title", bookTitle);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"Book found: {reader["Title"]} by {reader["AuthorName"]}");
                        }
                        else
                        {
                            Console.WriteLine($"Book '{bookTitle}' not found.");
                        }
                    }
                }
            }
        }

        public void FindByAuthor(string authorName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Books.Title, Authors.AuthorName FROM Books JOIN Authors ON Books.AuthorID = Authors.AuthorID WHERE AuthorName = @AuthorName", connection))
                {
                    command.Parameters.AddWithValue("@AuthorName", authorName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"Books by {authorName}:");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["Title"]} by {reader["AuthorName"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No books found by {authorName}.");
                        }
                    }
                }
            }
        }

        public void ShowAllBooksAndAuthors()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Books.Title, Authors.AuthorName FROM Books JOIN Authors ON Books.AuthorID = Authors.AuthorID", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("All Books and Authors:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Title"]} by {reader["AuthorName"]}");
                    }
                }
            }
        }

        public void DeleteBook(string bookTitle)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Find the book ID by title
                int bookId;
                using (SqlCommand findBookCommand = new SqlCommand("SELECT ID FROM Books WHERE Title = @Title", connection))
                {
                    findBookCommand.Parameters.AddWithValue("@Title", bookTitle);
                    bookId = (int?)findBookCommand.ExecuteScalar() ?? 0;
                }

                if (bookId == 0)
                {
                    Console.WriteLine($"Book '{bookTitle}' not found. Unable to delete.");
                    return;
                }

                // Delete the book
                using (SqlCommand deleteBookCommand = new SqlCommand("DELETE FROM Books WHERE ID = @BookID", connection))
                {
                    deleteBookCommand.Parameters.AddWithValue("@BookID", bookId);
                    deleteBookCommand.ExecuteNonQuery();
                }

                Console.WriteLine($"Book '{bookTitle}' deleted successfully.");
            }
        }
    }
}
