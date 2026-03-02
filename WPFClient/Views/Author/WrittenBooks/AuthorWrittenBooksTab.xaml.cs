using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Author.WrittenBooks
{
    public partial class AuthorWrittenBooksTab : UserControl, IObserver
    {
        public ObservableCollection<BookDTO> WrittenBooks { get; set; } = new ObservableCollection<BookDTO>();

        public static readonly DependencyProperty AuthorProperty =
                   DependencyProperty.Register(
                       nameof(Author),
                       typeof(AuthorDTO),
                       typeof(AuthorWrittenBooksTab),
                       new PropertyMetadata(null, OnDependencyPropertyChanged)
                   );
    
        private AuthorController AuthorController { get; }
        private BookController BookController { get; }

        public AuthorDTO? Author
        {
            get => (AuthorDTO?)GetValue(AuthorProperty);
            set => SetValue(AuthorProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AuthorWrittenBooksTab control = (AuthorWrittenBooksTab)d;
            control.Update();
        }

        public void Update()
        {
            if (Author == null) return;
            Render(Author.Books.Where(b => !b.IsDeleted).ToList());
        }

        public void Render(List<Core.Models.Book> books)
        {
            WrittenBooks.Clear();
            foreach (var b in books)
                WrittenBooks.Add(new BookDTO(b));
        }

        public AuthorWrittenBooksTab()
        {
            InitializeComponent();

            AuthorController = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO, App.Context.BookDAO);
            AuthorController.Subscribe(this);

            BookController = new BookController(App.Context.BookDAO, App.Context.VisitorDAO, App.Context.AuthorDAO, App.Context.PurchaseDAO);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Author == null) return;
            AuthorWrittenBooksAdd dialog = new AuthorWrittenBooksAdd(Author);
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Author == null) return;

            List<BookDTO> books = WrittenBooksDataGrid.SelectedItems.Cast<BookDTO>().ToList();  
            if (books.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    TranslationSource.Instance["DeleteBookConfirmationBody"],
                    TranslationSource.Instance["DeleteBookCaption"],
					MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                switch (result)
                {
                    case MessageBoxResult.Yes:
                            // Mora se brisati preko ID-a jer bi se konverzijom
                            // BookDTO u Book napravila nova instanca od Book
                            // i metoda Remove je ne bi uklonio jer bi referenca bila
                            // razlicita.
                            HashSet<int> ids = new HashSet<int>(books.Select(b => b.Id));
                            
                            Author.Books.RemoveAll((b) => ids.Contains(b.Id));
                            AuthorController.Update(Author.ToModel());
                            App.Context.RestoreRelations();
                            break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }

                return;
            }

            MessageBox.Show(
				  TranslationSource.Instance["NoBookSelectedDelete"],
				  TranslationSource.Instance["NoBookSelectedDeleteCaption"],
				  MessageBoxButton.OK,
                  MessageBoxImage.Warning
             );
        }
    }
}
