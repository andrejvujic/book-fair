using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Book
{
    public partial class AddBook : Window
    {
        public BookDTO Book { get; set; }
        private readonly BookController bookController;

        public AddBook(BookController bookController)
        {
            InitializeComponent();

            Book = new BookDTO();
            this.bookController = bookController;

            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            bookController.Add(Book.ToModel());
            Close();
        }
    }
}
