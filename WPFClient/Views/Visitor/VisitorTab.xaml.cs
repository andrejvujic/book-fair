using Core.DAO;
using Core.Helpers.Sorting;
using Core.Helpers.Search;
using Core.Observer;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFClient.DTO;
using Core.Helpers.Pagination;
using System.Diagnostics;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorTab : UserControl, IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private List<Core.Models.Visitor> FilteredVisitors { get; set; } = new List<Core.Models.Visitor>();

        private VisitorDAO VisitorDAO { get; set; }
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
            control.PageNumber = 1; // Promjena search query-ja resetuje paginaciju na prvu stranicu.
            control.Update();
        }

        public VisitorTab()
        {
            InitializeComponent();

            VisitorDAO = App.Context.VisitorDAO;
            VisitorDAO.Subject.Subscribe(this);
            PageNumber = 1;

            Update();
        }

        public void Update()
        {
            // Treba pozivati samo kada se promijeni search query ili kada
            // dodje do promjene liste entiteta.
            FetchVisitors();

            // Ukoliko je zbog promjene search query-ja ili liste entiteta
            // doslo do smanjenja maksimalnog broja stranica,
            // potrebno je provjeriti da trenutni broj stranice
            // ne prelazi novi maksimalni broj stranice.
            if (PageNumber > MaxPageNumber) PageNumber = MaxPageNumber;

            UpdateSorting();
            UpdatePagination();
            UpdateCurrentPageIndicator();
        }

        private void FetchVisitors()
        {
            FilteredVisitors = new List<Core.Models.Visitor>(
                VisitorDAO
                .GetWhere((e) => !e.IsDeleted)
                .Search(new SearchQuery(RawSearchQuery ?? string.Empty, 3))
            );

            MaxPageNumber = FilteredVisitors.GetNumberOfPages(PageSize);
            OnPropertyChanged(nameof(MaxPageNumber));
            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(NextPageExists));
        }

        private void UpdateSorting() =>
            FilteredVisitors = FilteredVisitors
                .Sort(SortingCriteria, SortingDirection);

        private void UpdatePagination()
        {
            List<Core.Models.Visitor> visitors = FilteredVisitors
                .Paginate(PageNumber, PageSize);

            Visitors.Clear();
            foreach (Core.Models.Visitor v in visitors)
                Visitors.Add(new VisitorDTO(v));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string SortingCriteria { get; set; } = string.Empty;
        private Direction SortingDirection { get; set; } = Direction.Ascending;

        private int PageNumber { get; set; } = 1;
        private int MaxPageNumber { get; set; } = 1;
        private const int PageSize = 16;

        private void VisitorsDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            string NewSortingCriteria = e.Column.SortMemberPath ?? string.Empty;
            if (string.IsNullOrWhiteSpace(NewSortingCriteria)) return;
            SortingCriteria = NewSortingCriteria;
            SortingDirection = e.Column.SortDirection == ListSortDirection.Ascending ? Direction.Descending : Direction.Ascending;

            PageNumber = 1; // Sortiranje resetuje paginaciju na prvu stranicu.

            UpdateSorting();
            // Nakon sortiranja, potrebno je ponovo obaviti paginaciju
            // da bi se prikazali ispravno sortirani rezultati.
            UpdatePagination();

            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(NextPageExists));
            UpdateCurrentPageIndicator();
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PreviousPageExists) return;

            // Potrebno je samo obaviti paginaciju, bez ponovnog sortiranja.
            PageNumber = PageNumber - 1;
            UpdatePagination();

            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(NextPageExists));
            UpdateCurrentPageIndicator();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NextPageExists) return;

            // Potrebno je samo obaviti paginaciju, bez ponovnog sortiranja.
            PageNumber = PageNumber + 1;
            UpdatePagination();

            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(NextPageExists));
            UpdateCurrentPageIndicator();
        }

        private void UpdateCurrentPageIndicator() {
            if (MaxPageNumber > 0)
            {
                CurrentPageIndicator.Text = $"Trenutna stranica — {PageNumber} / {MaxPageNumber}";
                return;
            }

            CurrentPageIndicator.Text = "Nema posjetilaca za prikazivanje";
        }

        public bool PreviousPageExists { get => PageNumber - 1 >= 1; }
        public bool NextPageExists { get => PageNumber + 1 <= MaxPageNumber; }
    }
}
