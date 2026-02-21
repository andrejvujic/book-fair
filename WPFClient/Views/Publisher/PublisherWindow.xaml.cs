using Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Publisher
{
    public partial class PublisherWindow : Window, IObserver
    {
        public ObservableCollection<PublisherDTO> Publishers { get; set; } = new ObservableCollection<PublisherDTO>();
        private PublisherController PublisherController { get; }

        public PublisherWindow()
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
            foreach (var p in PublisherController.GetAll())
                Publishers.Add(new PublisherDTO(p));
        }

        private void SetBossButton_Click(object sender, RoutedEventArgs e)
        {
            PublisherDTO? targetPublisher = (PublisherDTO?)PublishersDataGrid.SelectedItem;

            if (targetPublisher == null)
            {
                MessageBox.Show(
                    "Nije odabran izdavač za kojeg se postavlja šef. Potrebno je odabrati izdavača iz liste.",
                    "Nije odabran izdavač",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            SetPublisherBoss window = new SetPublisherBoss(targetPublisher!);
            window.Owner = this;
            window.ShowDialog();
        }

        private void AddPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            AddPublisher window = new AddPublisher();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
