using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
using Core.Models;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Publisher
{
    /// <summary>
    /// Interaction logic for PublisherConnectedBooks.xaml
    /// </summary>
    public partial class PublisherConnectedBooks : Window
    {
		public PublisherDTO Publisher { get; }
		private HashSet<int> PublisherBookIds { get; }
		public string RawSearchQuery { get; set; } = string.Empty;
		public ObservableCollection<BookDTO> Books { get; set; } = new ObservableCollection<BookDTO>();
		private BookController BookController { get; }


		public PublisherConnectedBooks(PublisherDTO publisher)
        {
            InitializeComponent();

			Publisher = publisher;
			PublisherBookIds = new HashSet<int>(Publisher.Books.Select((b) => b.Id));
			BookController = new BookController(App.Context.BookDAO);
			DataContext = this;
			Update();
		}
		public void Update()
		{
			Render(
				BookController
				.PerformSerach(
					RawSearchQuery,
					(b => PublisherBookIds.Contains(b.Id) && !b.IsDeleted)
				)
				.PerformSorting()
				.GetAllWithoutPagination()
			);
		}

		private void Render(List<Core.Models.Book> books)
		{
			Books.Clear();
			foreach (Core.Models.Book b in books )
				Books.Add(new BookDTO(b));
		}

		private void UpdateSearchQuery()
		{
			SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
			Update();
		}
		private void SearchButton_Click(object sender, RoutedEventArgs e) =>
		   UpdateSearchQuery();

		private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
		{
			RawSearchQuery = string.Empty;
			SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
			Update();
		}
		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox target = (TextBox)sender;
			if (!string.IsNullOrWhiteSpace(target.Text)) return;
			UpdateSearchQuery();
		}
		private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				UpdateSearchQuery();
				return;
			}
		}
	}
}
