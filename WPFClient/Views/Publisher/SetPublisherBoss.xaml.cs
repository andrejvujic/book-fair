using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;

namespace WPFClient.Views.Publisher
{
    public partial class SetPublisherBoss : Window
    {
        public string RawSearchQuery { get; set; } = string.Empty;

        private PublisherController PublisherController { get; }
        private PublisherDTO Publisher { get; }

        private AuthorController AuthorController { get; }
        public ObservableCollection<AuthorDTO> Authors { get; } = new ObservableCollection<AuthorDTO>();


        public SetPublisherBoss(PublisherDTO publisher)
        {
            InitializeComponent();

            PublisherController = new PublisherController(App.Context.PublisherDAO);
            Publisher = publisher;
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
                    (a) => a.Id != Publisher.BossId
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
