using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WPFClient.DTO;

namespace WPFClient.Views.Book.Comparison
{
    public partial class BookComparison : Window
    {
        public BookDTO FirstBook { get; }
        public BookDTO SecondBook { get; }

        public BookComparison(BookDTO firstBook, BookDTO secondBook)
        {
            InitializeComponent();

            FirstBook = firstBook;
            SecondBook = secondBook;

            DataContext = this;
        }
    }
}
