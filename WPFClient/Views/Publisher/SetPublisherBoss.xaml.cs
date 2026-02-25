using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

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
                .GetWhere(a => a.Id != Publisher.BossId)
                .Select(a => new AuthorDTO(a))
                .ToList();
        }


        private void SetAsBossButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorDTO? selectedAuthor = (AuthorDTO?)AuthorsDataGrid.SelectedItem;

            if (selectedAuthor == null)
            {
                MessageBox.Show("Nije odabran autor koji se postavlja za šefa odabranog izdavača. Potrebno je odabrati autora iz liste.", "Nije odabran autor", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Core.Models.Author boss = selectedAuthor.ToModel();

            if (boss.YearsOfExperience < 5)
            {
                MessageBox.Show("Autor mora imati najmanje 5 godina iskustva da bi bio postavljen za šefa izdavača.", "Autor nema dovoljno iskustva", MessageBoxButton.OK, MessageBoxImage.Warning);
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
