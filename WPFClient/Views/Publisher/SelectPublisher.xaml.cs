using Core.Models;
using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Publisher
{
    public partial class SelectPublisher : Window, IObserver
    {
        public string RawSearchQuery { get; set; } = string.Empty;

        public PublisherDTO? SelectedPublisher { get; set; }
        public ObservableCollection<PublisherDTO> Publishers { get; set; } = new ObservableCollection<PublisherDTO>();
        private PublisherController PublisherController { get; }

        public SelectPublisher()
        {
            InitializeComponent();

            PublisherController = new PublisherController(App.Context.PublisherDAO);
            PublisherController.Subscribe(this);

            DataContext = this;

            Update();
        }

        public void Update()
        {
            Render(
                PublisherController
                .PerformSerach(RawSearchQuery)
                .PerformSorting()
                .GetAllWithoutPagination()
            );
        }

        private void Render(List<Core.Models.Publisher> publishers)
        {
            Publishers.Clear();
            foreach (var p in publishers)
                Publishers.Add(new PublisherDTO(p));
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
            PublisherDTO? publisher = PublishersDataGrid.SelectedItem as PublisherDTO;

            if (publisher == null)
            {
                MessageBox.Show(
                    TranslationSource.Instance["NoSelectedPublisherText"],
					TranslationSource.Instance["NoSelectedPublisherCaption"],
					MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            SelectedPublisher = publisher;
            Close();
        }
    }
}
