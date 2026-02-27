using Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Publisher
{
    public partial class SelectPublisher : Window, IObserver
    {
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
            Publishers.Clear();
            foreach (var p in PublisherController.GetWhere((e) => !e.IsDeleted))
                Publishers.Add(new PublisherDTO(p));
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
