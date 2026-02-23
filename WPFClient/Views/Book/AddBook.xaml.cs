using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Publisher;

namespace WPFClient.Views.Book
{
    public partial class AddBook : Window
    {
        public BookDTO Book { get; set; }
        private BookController BookController { get; }

        public AddBook(BookController bookController)
        {
            InitializeComponent();

            Book = new BookDTO();
            BookController = bookController;

            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            BookController.Add(Book.ToModel());
            Close();
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
    }
}
