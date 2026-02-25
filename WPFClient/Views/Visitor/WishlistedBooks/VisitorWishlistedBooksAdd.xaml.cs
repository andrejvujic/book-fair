using Core.DAO;
using Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor.WishlistedBooks
{
    public partial class VisitorWishlistedBooksAdd : Window
    {
        private VisitorDTO Visitor { get; }

        public string RawSearchQuery { get; set; } = string.Empty;

        private HashSet<int> VisitorWishlistedBookIds { get; }
        private HashSet<int> VisitorPurchasedBookIds { get; }

        public ObservableCollection<BookDTO> AllowedBooks { get; set; } = new ObservableCollection<BookDTO>();
        private BookController BookController { get; }
        private VisitorController VisitorController { get; }

        public VisitorWishlistedBooksAdd(VisitorDTO visitor)
        {
            InitializeComponent();
            Visitor = visitor;
            VisitorWishlistedBookIds = new HashSet<int>(Visitor.WishlistedBooks.Select((b) => b.Id));
            VisitorPurchasedBookIds = new HashSet<int>(Visitor.PurchasedBooks.Select((b) => b.Id));

            BookController = new BookController(App.Context.BookDAO, App.Context.VisitorDAO, App.Context.AuthorDAO, App.Context.PurchaseDAO);
            VisitorController = new VisitorController(
                App.Context.VisitorDAO,
                App.Context.AddressDAO,
                App.Context.BookDAO,
                App.Context.PurchaseDAO
            );

            DataContext = this;

            Update();
        }

        void Update()
        {
            // Knjigu je moguce dodati na listu zelja iskljucivo ako nije prethodno
            // dodana na listu zelja i ako nije medju kupljenim knjigama.
            Render(
                BookController
                    .PerformSerach(
                        RawSearchQuery,
                        b => !VisitorWishlistedBookIds.Contains(b.Id) && !VisitorPurchasedBookIds.Contains(b.Id)
                    )
                    .PerformSorting()
                    .GetAllWithoutPagination()
            );
        }

        private void Render(List<Core.Models.Book> books)
        {
            AllowedBooks.Clear();
            foreach (Core.Models.Book b in books)
                AllowedBooks.Add(new BookDTO(b));
        }

        private void UpdateSearchQuery()
        {
            SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            Update();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) =>
            UpdateSearchQuery();

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            RawSearchQuery = string.Empty;
            SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            Update();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox target = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(target.Text)) return;
            UpdateSearchQuery();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateSearchQuery();
                return;
            }
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
