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

        // Prva kategorija posjetilaca - posjetioci koji na listi zelja imaju
        // i prvu i drugu knjigu.
        public List<Core.Models.Visitor> VisitorsInFirstCategory { get; set; } = new List<Core.Models.Visitor>();

        // Druga kategorija posjetilaca - posjetioci koji su kupili prvu knjigu
        // ali nisu kupili drugu knjigu.
        public List<Core.Models.Visitor> VisitorsInSecondCategory { get; set; } = new List<Core.Models.Visitor>();

        public BookComparison(BookDTO firstBook, BookDTO secondBook)
        {
            InitializeComponent();

            FirstBook = firstBook;
            SecondBook = secondBook;

            PopulateVisitorCategories();

            DataContext = this;
        }

        private void PopulateVisitorCategories()
        {
            foreach (var visitor in FirstBook.Wishlisters)
            {
                if (SecondBook.Wishlisters.Any((v) => visitor.Id == v.Id))
                    VisitorsInFirstCategory.Add(visitor);
            }

            foreach (var visitor in FirstBook.Buyers)
                if (!SecondBook.Buyers.Any((v) => visitor.Id == v.Id))
                    VisitorsInSecondCategory.Add(visitor);
        }
    }
}
