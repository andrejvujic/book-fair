using Core.DAO;
using Core.Observer;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WPFClient.DTO;

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
    }
}
