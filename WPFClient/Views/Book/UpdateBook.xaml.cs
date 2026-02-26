using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;
using WPFClient.Views.Author;
using WPFClient.Views.Publisher;

namespace WPFClient.Views.Book
{
    public partial class UpdateBook : Window
    {
        public BookDTO Book { get; set; }
        private BookController BookController { get; }

        public UpdateBook(BookDTO book, BookController bookController)
        {
            InitializeComponent();

            Book = book.Copy();
            BookController = bookController;

            DataContext = this;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BookController.Update(Book.ToModel());
            Close();
        }

        private void SelectAuthorButton_Click(object sender, RoutedEventArgs e)
        {
			var window = new SelectAuthor { Owner = this };
			if (window.ShowDialog() != true) return;

			var selected = window.SelectedAuthor!.ToModel();

			if (Book.Authors == null) Book.Authors = new List<Core.Models.Author>();

			Book.Authors.Add(selected);
			Book.Authors = Book.Authors.ToList();
		}

		private void DeselectAuthorButton_Click(object sender, RoutedEventArgs e)
        {
			MessageBoxResult result = MessageBox.Show(
	            TranslationSource.Instance["DeleteAuthorConfirmationBody"],
                TranslationSource.Instance["DeleteAuthor"],
	            MessageBoxButton.YesNo,
	            MessageBoxImage.Question
            );

			switch (result)
			{
				case MessageBoxResult.Yes:
					Book.Authors = new List<Core.Models.Author>();
					break;
				case MessageBoxResult.No:
					break;
				default:
					break;
			}
		}
	}
}
