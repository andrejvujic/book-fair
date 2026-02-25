using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WPFClient.Views.Publisher
{

    public partial class PublisherConnectedAuthors : Window
    {
        public PublisherDTO Publisher { get; }
        private HashSet<int> PublisherBookIds { get; }
		public string RawSearchQuery { get; set; } = string.Empty;
		public ObservableCollection<AuthorDTO> Authors { get; set; } = new ObservableCollection<AuthorDTO>();
		private AuthorController AuthorController { get; }

		public PublisherConnectedAuthors(PublisherDTO publisher)
        {
            InitializeComponent();

            Publisher = publisher;
			PublisherBookIds = new HashSet<int>(Publisher.Books.Select((b) => b.Id));
			AuthorController = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO, App.Context.BookDAO);
			DataContext = this;
			Update();
		}

		public void Update()
		{
			Render(
				AuthorController
				.PerformSerach(
					RawSearchQuery,
					(e) => e.Books.Any((b) => PublisherBookIds.Contains(b.Id) && !b.IsDeleted)
				)
				.PerformSorting()
				.GetAllWithoutPagination()
			);
		}

		private void Render(List<Core.Models.Author> authors)
		{
			Authors.Clear();
			foreach (Core.Models.Author a in authors)
				Authors.Add(new AuthorDTO(a));
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
