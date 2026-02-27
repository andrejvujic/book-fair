using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Publisher
{
    public partial class SetPublisherBoss : Window
    {
        private PublisherController PublisherController { get; }
        private PublisherDTO Publisher { get; }

        private AuthorController AuthorController { get; }
        public List<AuthorDTO> Authors { get; }


        public SetPublisherBoss(PublisherDTO publisher)
        {
            InitializeComponent();

            PublisherController = new PublisherController(App.Context.PublisherDAO);
            Publisher = publisher;
            AuthorController = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO, App.Context.BookDAO);
            Authors = GetAvailableAuthors();

            DataContext = this;
        }

        List<AuthorDTO> GetAvailableAuthors()
        {
            // Dobavlja sve autore koji ce u korisnickom interfejsu
            // predstavljati kandidate za sefa izdavaca.
            return AuthorController
                .GetWhere(a => a.Id != Publisher.BossId && !a.IsDeleted)
                .Select(a => new AuthorDTO(a))
                .ToList();
        }


        private void SetAsBossButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorDTO? selectedAuthor = (AuthorDTO?)AuthorsDataGrid.SelectedItem;

            if (selectedAuthor == null)
            {
                MessageBox.Show(TranslationSource.Instance["AuthorBossText"], TranslationSource.Instance["EditAuthorCaption"], MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Core.Models.Author boss = selectedAuthor.ToModel();

            if (boss.YearsOfExperience < 5)
            {
                MessageBox.Show(TranslationSource.Instance["AuthorNoExperience"], TranslationSource.Instance["AuthorNoExperienceCaption"], MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Publisher.BossId = boss.Id;
            Publisher.Boss = boss;
            PublisherController.UpdateBoss(
                Publisher.ToModel(),
                boss
            );

            Close();
        }
    }
}
