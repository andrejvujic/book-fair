using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Publisher;

namespace WPFClient.Views.Book
{
    public partial class UpdateBook : Window
    {
        public BookDTO Book { get; set; }
        private BookController BookController { get; }

        public UpdateBook(BookDTO book, BookController bookController)
        {
            InitializeComponent();

            Book = book.Copy();
            BookController = bookController;

            DataContext = this;
        }
        private void SelectPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            SelectPublisher window = new SelectPublisher();
            window.Owner = this;
            window.ShowDialog();

            if (window.SelectedPublisher == null) return;
            PublisherDTO selectedPublisher = window.SelectedPublisher;
            Book.Publisher = selectedPublisher.ToModel();
        }

        private void DeselectPublisherButton_Click(object sender, RoutedEventArgs e) =>
            Book.Publisher = null;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BookController.Update(Book.ToModel());
            Close();
        }
    }
}
