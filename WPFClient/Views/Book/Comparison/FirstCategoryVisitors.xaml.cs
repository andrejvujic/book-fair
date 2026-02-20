using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Book.Comparison
{
    public partial class FirstCategoryVisitors : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private VisitorController VisitorController { get; }

        private string rawSearchQuery = string.Empty;
        public string RawSearchQuery
        {
            get => rawSearchQuery;
            set
            {
                if (string.Equals(rawSearchQuery, value)) return;
                rawSearchQuery = value;
                OnPropertyChanged(nameof(RawSearchQuery));
            }
        }

        public static readonly DependencyProperty FirstBookIdProperty =
            DependencyProperty.Register(
                nameof(FirstBookId),
                typeof(int?),
                typeof(FirstCategoryVisitors),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public int? FirstBookId
        {
            get => (int?)GetValue(FirstBookIdProperty);
            set => SetValue(FirstBookIdProperty, value);
        }


        public static readonly DependencyProperty SecondBookIdProperty =
            DependencyProperty.Register(
                nameof(SecondBookId),
                typeof(int?),
                typeof(FirstCategoryVisitors),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public int? SecondBookId
        {
            get => (int?)GetValue(SecondBookIdProperty);
            set => SetValue(SecondBookIdProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FirstCategoryVisitors control = (FirstCategoryVisitors)d;
            control.Update();
        }

        public void Update()
        {
            if (FirstBookId == null || SecondBookId == null) return;
            if (VisitorController == null) return;

            Render(
                VisitorController
                .PerformSerach(
                    RawSearchQuery,
                    // Prva kategorija predstavlja posjetioce koji na listi zelja
                    // imaju i prvu knjigu i drugu knjigu.
                    (e) => e.WishlistedBooks.Any((b) => b.Id == FirstBookId) && e.WishlistedBooks.Any((b) => b.Id == SecondBookId)
                )
                .PerformSorting()
                .GetWithPagination()
            );
        }

        public FirstCategoryVisitors()
        {
            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);
            InitializeComponent();
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSearchQuery();
        }

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            RawSearchQuery = string.Empty;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
