using Core.DAO;
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
            bookController.Add(Book.ToBook());
            Close();
        }
    }
}
