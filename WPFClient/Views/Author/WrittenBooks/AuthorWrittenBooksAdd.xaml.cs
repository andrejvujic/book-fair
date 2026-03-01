using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Author.WrittenBooks
{
    public partial class AuthorWrittenBooksAdd : Window
    {
        public List<BookDTO> AllowedBooks { get; set; } = new List<BookDTO>();
        private AuthorDTO Author { get; }

        private AuthorController AuthorController { get; } = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO, App.Context.BookDAO);
        private BookController BookController { get; } = new BookController(App.Context.BookDAO,App.Context.VisitorDAO, App.Context.AuthorDAO, App.Context.PurchaseDAO);


        public AuthorWrittenBooksAdd(AuthorDTO author)
        {
            InitializeComponent();

            Author = author;

            // Popunjava listu knjiga sa knjigama koje se ne nalaze
            // u listi knjiga koje je autor napisao.
            PopulateAllowedBooks();

            DataContext = this;
        }

        private void PopulateAllowedBooks()
        {
            foreach (var b in BookController
                              .GetWhere(
                                   // Prikazivati samo knjige koje nemaju autora?
                                   (b1) => !b1.IsDeleted && b1.Authors.Count == 0 // && Author.Books.All((b2) => b1.Id != b2.Id)
                              )
                    )
                AllowedBooks.Add(new BookDTO(b));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<BookDTO> books = AllowedBooksDataGrid.SelectedItems.Cast<BookDTO>().ToList();

            if (books.Count > 0)
            {
                foreach (var b in books)
                    Author.Books.Add(b.ToModel());

                foreach (var b in books)
                {
                    b.Authors.Add(Author.ToModel());
                    BookController.Update(b.ToModel());
                }

                AuthorController.Update(Author.ToModel());
                Close();
                return;
            }

            MessageBox.Show(
                  TranslationSource.Instance["AddBookText"],
                  TranslationSource.Instance["AddBookCaption"],
                  MessageBoxButton.OK,
                  MessageBoxImage.Warning
            );
        }
    }
}
