using Core.Models;
using System.Collections.Generic;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Publisher
{
    public partial class SetPublisherBoss : Window
    {
        public List<Author> Authors { get; } = new List<Author>(App.Context.AuthorDAO.GetAll());
        private PublisherDTO Publisher { get; }
        private PublisherController PublisherController { get; }

        public SetPublisherBoss(PublisherDTO publisher)
        {
            InitializeComponent();

            Publisher = publisher;
            PublisherController = new PublisherController(App.Context.PublisherDAO);

            DataContext = this;
        }

        private void SetAsBossButton_Click(object sender, RoutedEventArgs e)
        {
            Author? boss = (Author?)AuthorsDataGrid.SelectedItem;

            if (boss == null)
            {
                MessageBox.Show("Nije odabran autor koji se postavlja za šefa odabranog izdavača. Potrebno je odabrati autora iz liste.", "Nije odabran autor", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
