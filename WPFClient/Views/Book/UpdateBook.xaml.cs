using Core.Models;
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
        private AuthorController AuthorController { get; } = new AuthorController(
                App.Context.AuthorDAO, App.Context.AddressDAO, App.Context.BookDAO
            );

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
            foreach (var a in Book.Authors)
                AuthorController.Update(a);

            Close();
        }

        private void SelectAuthorButton_Click(object sender, RoutedEventArgs e)
        {
			var window = new SelectAuthor { Owner = this };
			if (window.ShowDialog() != true) return;

			var author = window.SelectedAuthor!.ToModel();
            author.Books.Add(Book.ToModel());
			Book.Authors.Add(author);
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
                    if (Book.Authors.Count > 0)
                        Book.Authors.Clear();
                    break;
				case MessageBoxResult.No:
					break;
				default:
					break;
			}
		}
	}
}
