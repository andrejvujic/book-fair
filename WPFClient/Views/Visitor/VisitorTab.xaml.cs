using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Core.Helpers.Sorting;
using Core.Observer;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Author;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorTab : UserControl, IObserver
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        public VisitorController VisitorController { get; }

        public VisitorDTO? SelectedVisitor => VisitorsDataGrid.SelectedItem as VisitorDTO;

        public static readonly DependencyProperty RawSearchQueryProperty =
            DependencyProperty.Register(
                nameof(RawSearchQuery),
                typeof(string),
                typeof(VisitorTab),
                new PropertyMetadata(null, OnSearchQueryPropertyChanged)
            );


        public string? RawSearchQuery
        {
            get => (string?)GetValue(RawSearchQueryProperty);
            set => SetValue(RawSearchQueryProperty, value);
        }

        private static void OnSearchQueryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorTab control = (VisitorTab)d;
            control.Update();
        }

        public VisitorTab()
        {
            InitializeComponent();

            VisitorController = new VisitorController(
                App.Context.VisitorDAO,
                App.Context.AddressDAO,
                App.Context.BookDAO,
                App.Context.PurchaseDAO
            );
            VisitorController.Subscribe(this);

            Update();
        }

        public void Update()
        {
            Render(
                VisitorController
                .PerformSerach(RawSearchQuery ?? string.Empty)
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

        private void VisitorsDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            string criteria = e.Column.SortMemberPath ?? string.Empty;

            var inverseDireciton = e.Column.SortDirection != ListSortDirection.Ascending
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            foreach (var column in VisitorsDataGrid.Columns)
                column.SortDirection = null;
            e.Column.SortDirection = inverseDireciton;

            var direction = inverseDireciton == ListSortDirection.Ascending
                ? Direction.Ascending
                : Direction.Descending;
            if (!VisitorController.SetSortingParameters(criteria, direction)) return;

            Render(
                VisitorController
                .PerformSorting()
                .GetWithPagination()
            );
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VisitorController.GoToPreviousPage()) return;
            Render(VisitorController.GetWithPagination());
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VisitorController.GoToNextPage()) return;
            Render(VisitorController.GetWithPagination());
        }

        private void OpenAuthorsWindowButton_Click(object sender, RoutedEventArgs e)
        {
			VisitorDTO? targetVisitor = VisitorsDataGrid.SelectedItem as VisitorDTO;
			if (targetVisitor == null)
			{
				MessageBox.Show(
					"Nije odabran posetilac za prikaz autora. Potrebno je odabrati posetioca iz liste.",
					"Nije odabran posetilac",
					MessageBoxButton.OK,
					MessageBoxImage.Warning
				);
				return;
			}

			VisitorConnectedAuthors dialog = new VisitorConnectedAuthors(targetVisitor);
			dialog.Owner = Window.GetWindow(this);
			dialog.ShowDialog();
		}
    }
}
