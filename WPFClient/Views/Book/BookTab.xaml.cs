using Core.DAO;
using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFClient.DTO;
using WPFClient.Views.Book.Comparison;

namespace WPFClient.Views.Book
{

    public partial class BookTab : UserControl, IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<BookDTO> Books { get; set; } = new ObservableCollection<BookDTO>();
        private BookDAO BookDAO { get; set; }
        public BookDTO? SelectedBook => BooksDataGrid.SelectedItem as BookDTO;


        private BookDTO? FirstBook { get; set; }
        private BookDTO? SecondBook { get; set; }

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

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            // Potrebno je selektovati dvije knjige da bi se otvorio prozor za uporedjivanje.
            if (FirstBook == null || SecondBook == null)
            {
                MessageBox.Show("Potrebno je selektovati dvije knjige iz liste kako bi se izvršilo upoređivanje knjiga.", "Neispravna selekcija knjiga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kada su selektovane dvije knjige, otvara se prozor za uporedjivanje.
            BookComparison window = new BookComparison(FirstBook!, SecondBook!);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();

            FirstBook = null;
            OnPropertyChanged(nameof(FirstBookSelectionText));
            OnPropertyChanged(nameof(IsSelectFirstBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectFirstBookButtonEnabled));

            SecondBook = null;
            OnPropertyChanged(nameof(SecondBookSelectionText));
            OnPropertyChanged(nameof(IsSelectSecondBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectSecondBookButtonEnabled));
        }

        public string FirstBookSelectionText { get => FirstBook == null ? "Nije odabrana" : FirstBook!.Title; }
        public string SecondBookSelectionText { get => SecondBook == null ? "Nije odabrana" : SecondBook!.Title; }

        public bool IsSelectFirstBookButtonEnabled => FirstBook == null;
        public bool IsSelectSecondBookButtonEnabled => SecondBook == null;

        public bool IsDeselectFirstBookButtonEnabled => !IsSelectFirstBookButtonEnabled;
        public bool IsDeselectSecondBookButtonEnabled => !IsSelectSecondBookButtonEnabled;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SelectFirstBookButton_Click(object sender, RoutedEventArgs e)
        {
            BookDTO? book = (BookDTO?)BooksDataGrid.SelectedItem;
            if (book == null)
            {
                MessageBox.Show("Potrebno je odabrati knjigu iz liste.", "Knjiga nije odabrana", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SecondBook != null && SecondBook.Id == book.Id)
            {
                MessageBox.Show("Nije moguće odabrati dva puta istu knjigu. Potrebno je odabrati dvije različite knjige.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FirstBook = book;
            OnPropertyChanged(nameof(FirstBookSelectionText));
            OnPropertyChanged(nameof(IsSelectFirstBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectFirstBookButtonEnabled));
        }

        private void SelectSecondBookButton_Click(object sender, RoutedEventArgs e)
        {
            BookDTO? book = (BookDTO?)BooksDataGrid.SelectedItem;
            if (book == null)
            {
                MessageBox.Show("Potrebno je odabrati knjigu iz liste.", "Knjiga nije odabrana", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (FirstBook != null && FirstBook.Id == book.Id)
            {
                MessageBox.Show("Nije moguće odabrati dva puta istu knjigu. Potrebno je odabrati dvije različite knjige iz liste kako bi se izvršilo poređenje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SecondBook = book;
            OnPropertyChanged(nameof(SecondBookSelectionText));
            OnPropertyChanged(nameof(IsSelectSecondBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectSecondBookButtonEnabled));
        }

        private void DeselectFirstBookButton_Click(object sender, RoutedEventArgs e)
        {
            FirstBook = null;
            OnPropertyChanged(nameof(FirstBookSelectionText));
            OnPropertyChanged(nameof(IsSelectFirstBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectFirstBookButtonEnabled));
        }

        private void DeselectSecondBookButton_Click(object sender, RoutedEventArgs e)
        {
            SecondBook = null;
            OnPropertyChanged(nameof(SecondBookSelectionText));
            OnPropertyChanged(nameof(IsSelectSecondBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectSecondBookButtonEnabled));
        }
    }
}
