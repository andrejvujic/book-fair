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
        private HashSet<int> VisitorWishlistedBookIds { get; }
        private HashSet<int> VisitorPurchasedBookIds { get; }

        public List<BookDTO> AllowedBooks { get; set; }
        private BookController BookController { get; }
        private VisitorController VisitorController { get; }

        public VisitorWishlistedBooksAdd(VisitorDTO visitor)
        {
            InitializeComponent();
            Visitor = visitor;
            VisitorWishlistedBookIds = new HashSet<int>(Visitor.WishlistedBooks.Select((b) => b.Id));
            VisitorPurchasedBookIds = new HashSet<int>(Visitor.PurchasedBooks.Select((b) => b.Id));

            BookController = new BookController(App.Context.BookDAO);
            VisitorController = new VisitorController(
                App.Context.VisitorDAO,
                App.Context.AddressDAO,
                App.Context.BookDAO,
                App.Context.PurchaseDAO
            );

            AllowedBooks = GetAllowedBooks();

            DataContext = this;
        }

        private List<BookDTO> GetAllowedBooks()
        {
            // Knjigu je moguce dodati na listu zelja iskljucivo ako nije prethodno
            // dodana na listu zelja i ako nije medju kupljenim knjigama.
            return BookController
                .GetWhere(
                    (b) => !VisitorWishlistedBookIds.Contains(b.Id) && !VisitorPurchasedBookIds.Contains(b.Id)
                )
                .Select((b) => new BookDTO(b))
                .ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BookDTO? targetBook = AllowedBooksDataGrid.SelectedItem as BookDTO;
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

            Visitor.WishlistedBooks.Add(targetBook.ToModel());
            VisitorController.Update(Visitor.ToModel());
            Close();
        }
    }
}
