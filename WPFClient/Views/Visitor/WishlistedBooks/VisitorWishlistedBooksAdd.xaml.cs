using Core.DAO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor.WishlistedBooks
{
    public partial class VisitorWishlistedBooksAdd : Window
    {
        private VisitorDTO Visitor { get; }
        public List<Core.Models.Book> AllowedBooks { get; set; }
        private VisitorController VisitorController { get; }

        public VisitorWishlistedBooksAdd(VisitorDTO visitor)
        {
            InitializeComponent();
            Visitor = visitor;
            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);
            AllowedBooks = GetAllowedBooks();
            DataContext = this;
        }

        private List<Core.Models.Book> GetAllowedBooks()
        {
            List<Core.Models.Book> allBooks = App.Context.BookDAO.GetAll();
            // Knjigu je moguce dodati na listu zelja iskljucivo ako nije prethodno
            // dodana na listu zelja i ako nije medju kupljenim knjigama.
            return allBooks.Where(
                (book) => !Visitor.WishlistedBooks.Contains(book) && !Visitor.PurchasedBooks.Contains(book)
            ).ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Core.Models.Book? targetBook = AllowedBooksDataGrid.SelectedItem as Core.Models.Book;
            if (targetBook == null)
            {
                MessageBox.Show(
                      "Nije odabrana knjiga za dodavanje. Potrebno je odabrati knjigu iz liste.",
                      "Nije odabrana knjiga",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning
                );
                return;
            }

            Visitor.WishlistedBooks.Add(targetBook);
            VisitorController.Update(Visitor.ToModel());
            Close();
        }
    }
}
