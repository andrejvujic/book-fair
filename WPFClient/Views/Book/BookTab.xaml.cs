using Core.DAO;
using Core.Helpers.Sorting;
using Core.Models;
using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;
using WPFClient.Views.Author;
using WPFClient.Views.Book.Comparison;

namespace WPFClient.Views.Book
{

    public partial class BookTab : UserControl, IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<BookDTO> Books { get; set; } = new ObservableCollection<BookDTO>();
        public BookController BookController { get; }

        public BookDTO? SelectedBook => BooksDataGrid.SelectedItem as BookDTO;

		public BookDTO? FirstBook { get; set; }
		public BookDTO? SecondBook { get; set; }

		public static readonly DependencyProperty RawSearchQueryProperty =
           DependencyProperty.Register(
	           nameof(RawSearchQuery),
	           typeof(string),
	           typeof(BookTab),
	           new PropertyMetadata(null, OnSearchQueryPropertyChanged)
           );

		public string? RawSearchQuery
		{
			get => (string?)GetValue(RawSearchQueryProperty);
			set => SetValue(RawSearchQueryProperty, value);
		}
		private static void OnSearchQueryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			BookTab control = (BookTab)d;
			control.Update();
		}

		public BookTab()
        {
            InitializeComponent();
            BookController = new BookController(App.Context.BookDAO, App.Context.VisitorDAO, App.Context.AuthorDAO, App.Context.PurchaseDAO);
            BookController.Subscribe(this);
            Update();
        }

        public void Update()
        {
			Render(
				BookController
				.PerformSerach(RawSearchQuery ?? string.Empty)
				.PerformSorting()
				.GetWithPagination()
			);
		}

		private void Render(List<Core.Models.Book> books)
		{
			Books.Clear();
			foreach (Core.Models.Book b in books)
				Books.Add(new BookDTO(b));
		}

        private void BooksDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            string criteria = e.Column.SortMemberPath ?? string.Empty;

            var inverseDireciton = e.Column.SortDirection != ListSortDirection.Ascending
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            foreach (var column in BooksDataGrid.Columns)
                column.SortDirection = null;
            e.Column.SortDirection = inverseDireciton;

            var direction = inverseDireciton == ListSortDirection.Ascending
                ? Direction.Ascending
                : Direction.Descending;

            if (!BookController.SetSortingParameters(criteria, direction))
                return;

            Render(
                BookController
                    .PerformSorting()
                    .GetWithPagination()
            );
        }


        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            // Potrebno je selektovati dvije knjige da bi se otvorio prozor za uporedjivanje.
            if (FirstBook == null || SecondBook == null)
            {
<<<<<<< HEAD
                MessageBox.Show(TranslationSource.Instance["NoSelectedBookCompareText"], TranslationSource.Instance["NoSelectedBookCompareCaption"], MessageBoxButton.OK, MessageBoxImage.Warning);
=======
                MessageBox.Show(
                    TranslationSource.Instance["CompareBooksErrorBody1"],
                    TranslationSource.Instance["CompareBooksErrorTitle1"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
>>>>>>> 798ef598d4e40333bf9755cba79437ee4ad310da
                return;
            }

            // Kada su selektovane dvije knjige, otvara se prozor za uporedjivanje.
            BookComparison window = new BookComparison(FirstBook!, SecondBook!);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();

            FirstBook = null;
            OnPropertyChanged(nameof(FirstBook));
            OnPropertyChanged(nameof(IsSelectFirstBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectFirstBookButtonEnabled));

            SecondBook = null;
            OnPropertyChanged(nameof(SecondBook));
            OnPropertyChanged(nameof(IsSelectSecondBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectSecondBookButtonEnabled));
        }

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
<<<<<<< HEAD
                MessageBox.Show(TranslationSource.Instance["NoSelectedBookText"], TranslationSource.Instance["NoSelectedBookCaption"], MessageBoxButton.OK, MessageBoxImage.Warning);
=======
                MessageBox.Show(
                    TranslationSource.Instance["BookNotSelectedBody"],
                    TranslationSource.Instance["BookNotSelectedTitle"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
>>>>>>> 798ef598d4e40333bf9755cba79437ee4ad310da
                return;
            }

            if (SecondBook != null && SecondBook.Id == book.Id)
            {
<<<<<<< HEAD
                MessageBox.Show(TranslationSource.Instance["SameBookImpossible"], TranslationSource.Instance["Error"], MessageBoxButton.OK, MessageBoxImage.Warning);
=======
                MessageBox.Show(
                    TranslationSource.Instance["CompareBooksErrorBody2"],
                    TranslationSource.Instance["CompareBooksErrorTitle2"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
>>>>>>> 798ef598d4e40333bf9755cba79437ee4ad310da
                return;
            }

            FirstBook = book;
            OnPropertyChanged(nameof(FirstBook));
            OnPropertyChanged(nameof(IsSelectFirstBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectFirstBookButtonEnabled));
        }

        private void SelectSecondBookButton_Click(object sender, RoutedEventArgs e)
        {
            BookDTO? book = (BookDTO?)BooksDataGrid.SelectedItem;
            if (book == null)
            {
<<<<<<< HEAD
                MessageBox.Show(TranslationSource.Instance["NoSelectedBookText"], TranslationSource.Instance["NoSelectedBookCaption"], MessageBoxButton.OK, MessageBoxImage.Warning);
=======
                MessageBox.Show(
                    TranslationSource.Instance["BookNotSelectedBody"],
                    TranslationSource.Instance["BookNotSelectedTitle"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
>>>>>>> 798ef598d4e40333bf9755cba79437ee4ad310da
                return;
            }

            if (FirstBook != null && FirstBook.Id == book.Id)
            {
<<<<<<< HEAD
                MessageBox.Show(TranslationSource.Instance["SameBookImpossible"], TranslationSource.Instance["Error"], MessageBoxButton.OK, MessageBoxImage.Warning);
=======
                MessageBox.Show(
                    TranslationSource.Instance["CompareBooksErrorBody2"],
                    TranslationSource.Instance["CompareBooksErrorTitle2"],
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
>>>>>>> 798ef598d4e40333bf9755cba79437ee4ad310da
                return;
            }

            SecondBook = book;
            OnPropertyChanged(nameof(SecondBook));
            OnPropertyChanged(nameof(IsSelectSecondBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectSecondBookButtonEnabled));
        }

        private void DeselectFirstBookButton_Click(object sender, RoutedEventArgs e)
        {
            FirstBook = null;
            OnPropertyChanged(nameof(FirstBook));
            OnPropertyChanged(nameof(IsSelectFirstBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectFirstBookButtonEnabled));
        }

        private void DeselectSecondBookButton_Click(object sender, RoutedEventArgs e)
        {
            SecondBook = null;
            OnPropertyChanged(nameof(SecondBook));
            OnPropertyChanged(nameof(IsSelectSecondBookButtonEnabled));
            OnPropertyChanged(nameof(IsDeselectSecondBookButtonEnabled));
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
			if (!BookController.GoToPreviousPage()) return;
			Render(BookController.GetWithPagination());
		}

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
			if (!BookController.GoToNextPage()) return;
			Render(BookController.GetWithPagination());
		}
	}
}
