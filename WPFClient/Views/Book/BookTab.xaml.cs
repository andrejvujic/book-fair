using Core.DAO;
using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFClient.DTO;
using WPFClient.Views.Book.Comparison;

namespace WPFClient.Views.Book
{

    public partial class BookTab : UserControl, IObserver
    {
        public ObservableCollection<BookDTO> Books { get; set; } = new ObservableCollection<BookDTO>();
        private BookDAO BookDAO { get; set; }
        public BookDTO? SelectedBook => BooksDataGrid.SelectedItem as BookDTO;

        public BookTab()
        {
            InitializeComponent();

            BookDAO = App.Context.BookDAO;
            BookDAO.Subject.Subscribe(this);
            Update();

            DataContext = this;
        }

        public void Update()
        {
            Books.Clear();
            foreach (var b in BookDAO.GetAll())
                Books.Add(new BookDTO(b));
        }

        private void CompareButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Potrebno je selektovati tacno 2 knjige da bi se otvorio prozor za upoređivanje.
            if (BooksDataGrid.SelectedItems.Count != 2)
            {
                MessageBox.Show("Potrebno je selektovati tačno dvije knjige iz liste kako bi se izvršilo upoređivanje knjiga.", "Neispravna selekcija knjiga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kada su selektovane tacno 2 knjige, otvara se prozor za upoređivanje.
            List<BookDTO> selectedBooks = BooksDataGrid.SelectedItems.Cast<BookDTO>().ToList();
            BookComparison window = new BookComparison(selectedBooks.First(), selectedBooks.Last());
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }
    }
}
