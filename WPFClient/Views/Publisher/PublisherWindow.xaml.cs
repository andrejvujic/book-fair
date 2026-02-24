using System.Collections.ObjectModel;
using System.Windows;
using Core.Observer;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Author;

namespace WPFClient.Views.Publisher
{
    public partial class PublisherWindow : Window, IObserver
    {
        public ObservableCollection<PublisherDTO> Publishers { get; set; } = new ObservableCollection<PublisherDTO>();
        private PublisherController PublisherController { get; }
        private BookController BookController { get; }

        public PublisherWindow()
        {
            InitializeComponent();

            BookController = new BookController(App.Context.BookDAO);
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

        private void EditPublisherButton_Click(object sender, RoutedEventArgs e)
        {
            PublisherDTO? targetPublisher = (PublisherDTO?)PublishersDataGrid.SelectedItem;
            if (targetPublisher == null)
            {
                MessageBox.Show(
                    "Nije odabran izdavač za brisanje. Potrebno je odabrati izdavača iz liste.",
                    "Nije odabran izdavač",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            EditPublisher window = new EditPublisher(targetPublisher);
            window.Owner = this;
            window.ShowDialog();
        }

        private void DeletePublisherButton_Click(object sender, RoutedEventArgs e)
        {
            PublisherDTO? targetPublisher = (PublisherDTO?)PublishersDataGrid.SelectedItem;
            if (targetPublisher == null)
            {
                MessageBox.Show(
                    "Nije odabran izdavač za brisanje. Potrebno je odabrati izdavača iz liste.",
                    "Nije odabran izdavač",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (BookController.GetWhere(
                    b => b.Publisher != null && b.Publisher.Id == targetPublisher.Id &&
                         !b.IsDeleted
                )
                .Count > 0
            )
            {
                MessageBox.Show(
                    "Brisanje odabranog izdavača nije moguće zato što u sistemu postoje knjige koje je izdao. Potrebno je prvo obrisati knjige ovog izdavača.",
                    "Brisanje nije moguće",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite da obrišete izdavača?",
                "Brisanje izdavača",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            switch (result)
            {
                case MessageBoxResult.Yes:
                    targetPublisher.IsDeleted = true;
                    PublisherController.Update(targetPublisher.ToModel());
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void OpenAuthorsWindowButton_Click(object sender, RoutedEventArgs e)
        {
			PublisherDTO? targetPublisher = PublishersDataGrid.SelectedItem as PublisherDTO;
			if (targetPublisher == null)
			{
				MessageBox.Show(
					"Nije odabran izdavač za prikaz autora. Potrebno je odabrati izdavača iz liste.",
					"Nije odabran izdavač",
					MessageBoxButton.OK,
					MessageBoxImage.Warning
				);
				return;
			}

			//AuthorConnectedVisitors dialog = new AuthorConnectedVisitors(targetAuthor);
			//dialog.Owner = Window.GetWindow(this);
			//dialog.ShowDialog();
		}
    }
}
