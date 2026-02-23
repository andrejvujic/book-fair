using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Author
{
    public partial class AuthorConnectedVisitors : Window
    {
        public AuthorDTO Author { get; }
        private HashSet<int> AuthorBookIds { get; }

        public string RawSearchQuery { get; set; } = string.Empty;

        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private VisitorController VisitorController { get; }

        public AuthorConnectedVisitors(AuthorDTO author)
        {
            InitializeComponent();

            Author = author;
            AuthorBookIds = new HashSet<int>(Author.Books.Select((b) => b.Id));

            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);

            DataContext = this;

            Update();
        }

        public void Update()
        {
            Render(
                VisitorController
                .PerformSerach(
                    RawSearchQuery,
                    // Potrebno je prikazati samo posjetioce koji na listi zelja imaju
                    // barem jednu knjiga zadanog autora.
                    (e) => e.WishlistedBooks.Any((b) => AuthorBookIds.Contains(b.Id))
                )
                .PerformSorting()
                .GetWithPagination()
            );
        }

        private void Render(List<Core.Models.Visitor> visitors)
        {
            Visitors.Clear();
            foreach (Core.Models.Visitor v in visitors)
                Visitors.Add(new VisitorDTO(v));
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
