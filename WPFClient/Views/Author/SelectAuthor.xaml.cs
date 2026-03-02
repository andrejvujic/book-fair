using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Author
{
    public partial class SelectAuthor : Window, IObserver
    {
        public string RawSearchQuery { get; set; } = string.Empty;

        public AuthorDTO? SelectedAuthor { get; set; }
        public ObservableCollection<AuthorDTO> Authors { get; set; } = new ObservableCollection<AuthorDTO>();
        private AuthorController AuthorController { get; }
		public SelectAuthor()
        {
            InitializeComponent();

            AuthorController = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO);
            AuthorController.Subscribe(this);
            DataContext = this;
            Update();
		}

        public void Update()
        {
            Render(
                AuthorController
                .PerformSerach(RawSearchQuery)
                .PerformSorting()
                .GetAllWithoutPagination()
            );
        }

        private void Render(List<Core.Models.Author> authors)
        {
            Authors.Clear();
            foreach (var a in authors)
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

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorDTO? author = AuthorsDataGrid.SelectedItem as AuthorDTO;

            if (author == null)
            {
				MessageBox.Show(
                   TranslationSource.Instance["NoAuthorText"],
                   TranslationSource.Instance["NoAuthorSelectedCaption"],  
				   MessageBoxButton.OK,
				   MessageBoxImage.Warning
			   );
				return;
			}

            SelectedAuthor = author;
            DialogResult = true;
		}
    }
}
