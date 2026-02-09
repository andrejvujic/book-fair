using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Book
{
    public partial class UpdateBook : Window
    {
        public BookDTO Book { get; set; }
        public BookController BookController { get; set; }

        public UpdateBook(BookDTO book, BookController bookController)
        {
            InitializeComponent();

            Book = book.Copy();
            BookController = bookController;

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BookController.Update(Book.ToModel());
            Close();
        }
    }
}
