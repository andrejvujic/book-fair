using Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Purchase;
using WPFClient.Views.Visitor.WishlistedBooks;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorWishlistedBooksTab : UserControl, IObserver
    {
        public static readonly DependencyProperty VisitorProperty =
            DependencyProperty.Register(
                nameof(Visitor),
                typeof(VisitorDTO),
                typeof(VisitorWishlistedBooksTab),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public VisitorDTO? Visitor
        {
            get => (VisitorDTO?)GetValue(VisitorProperty);
            set => SetValue(VisitorProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorWishlistedBooksTab control = (VisitorWishlistedBooksTab)d;
            control.Update();
        }

        public ObservableCollection<BookDTO> WishlistedBooks { get; set; } = new ObservableCollection<BookDTO>();
        private VisitorController VisitorController { get; set; }

        public VisitorWishlistedBooksTab()
        {
            InitializeComponent();
            VisitorController = new VisitorController(
                App.Context.VisitorDAO,
                App.Context.AddressDAO,
                App.Context.BookDAO,
                App.Context.PurchaseDAO
            );
            VisitorController.Subscribe(this);
        }

        public void Update()
        {
            if (Visitor == null) return;

            WishlistedBooks.Clear();
            foreach (var book in Visitor.WishlistedBooks)
                WishlistedBooks.Add(new BookDTO(book));
        }

        private void MakePurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Visitor == null) return;

            BookDTO? book = WishlistedBooksDataGrid.SelectedItem as BookDTO;
            if (book == null)
            {
                MessageBox.Show(
                      "Nije odabrana knjiga za kupovinu. Potrebno je odabrati knjigu iz liste, a zatim odabrati opciju 'Kupovina'.",
                      "Nije odabrana knjiga za kupovinu",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning
                  );
                return;
            }

            MakePurchase window = new MakePurchase(Visitor.ToModel(), book.ToModel());
            window.Owner = Window.GetWindow(this);
            if (!(window.ShowDialog() ?? false)) return;

            Visitor.WishlistedBooks.RemoveAll(b => b.Id == book.Id);
            Visitor.PurchasedBooks.Add(book.ToModel());
            VisitorController.Update(Visitor.ToModel());
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Visitor == null) return;

            BookDTO? targetBook = WishlistedBooksDataGrid.SelectedItem as BookDTO;
            if (targetBook == null)
            {
                MessageBox.Show(
                      "Nije odabrana knjiga za brisanje. Potrebno je odabrati knjigu iz liste, a zatim odabrati opciju 'Obriši'.",
                      "Nije odabrana knjiga za brisanje",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning
                  );
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite da obrišete knjigu iz liste želja?",
                "Brisanje knjige",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            switch (result)
                {
                    case MessageBoxResult.Yes:
                        Visitor.WishlistedBooks.RemoveAll((book) => book.Id == targetBook.Id);
                        VisitorController.Update(Visitor.ToModel());
                    break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Visitor == null) return;
            VisitorWishlistedBooksAdd addBookDialog = new VisitorWishlistedBooksAdd(Visitor);
            addBookDialog.Owner = Window.GetWindow(this);
            addBookDialog.ShowDialog();
        }
    }
}
