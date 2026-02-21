using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Author.WrittenBooks
{
    public partial class AuthorWrittenBooksAdd : Window
    {
        public List<BookDTO> AllowedBooks { get; set; } = new List<BookDTO>();
        private Core.Models.Author Author { get; }

        private BookController BookController { get; } = new BookController(App.Context.BookDAO);


        public AuthorWrittenBooksAdd(Core.Models.Author author)
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
                                   (b1) => Author.Books.All((b2) => b1.Id != b2.Id)
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
                // TODO: Ovde treba koristiti AuthorController
                App.Context.AuthorDAO.Update(Author);
                Close();
                return;
            }

            MessageBox.Show(
                  "Nije odabrana nijedna knjiga za dodavanje. Potrebno je odabrati barem jednu knjigu iz liste.",
                  "Nije odabrana nijedna knjiga",
                  MessageBoxButton.OK,
                  MessageBoxImage.Warning
            );
        }
    }
}
