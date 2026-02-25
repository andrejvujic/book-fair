using Core.DAO;
using Core.Helpers;
using Core.Helpers.Pagination;
using Core.Helpers.Search;
using Core.Helpers.Sorting;
using Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace WPFClient.Controllers
{
    public abstract class QueryController<T> : Controller<T>, INotifyPropertyChanged
        where T : class, ISerializable, IIdentifiable, ISearchable, ISortable, IDeletable, new()
    {
        public QueryController(DAO<T> dao) : base(dao) { }
        private List<T> FilteredEntities { get; set; } = new List<T>();


        private string PreviousRawSearchQuery { get; set; } = string.Empty;
        public abstract int SearchQueryLength { get; }


        private const int PageSize = 16;
        public int MaxPageNumber { get => FilteredEntities.GetNumberOfPages(PageSize); }
        public int PageNumber { get; private set; } = 1;
        
        public bool PreviousPageExists { get => PageNumber - 1 >= 1; }
        public bool NextPageExists { get => PageNumber + 1 <= MaxPageNumber; }

        private void Search(string rawSearchQuery, Predicate<T>? additionalPredicate = null, bool excludeDeleted = true)
        {
            FilteredEntities = _dao
                .GetWhere((e) => (!e.IsDeleted || !excludeDeleted) && (additionalPredicate == null || additionalPredicate(e)))
                .Search(
                    new SearchQuery(
                        rawSearchQuery ?? string.Empty,
                        SearchQueryLength
                    )
                );
        }

        public QueryController<T> PerformSerach(
            string rawSearchQuery,
            Predicate<T>? additionalPredicate = null,
            bool excludeDeleted = true
        ) {
            // Promjena pretrage resetuje paginaciju na pocetnu stranicu.
            if (!string.Equals(PreviousRawSearchQuery, rawSearchQuery)) PageNumber = 1;
            PreviousRawSearchQuery = rawSearchQuery;

            Search(rawSearchQuery, additionalPredicate, excludeDeleted);

            // Ukoliko je zbog promjene search query-ja ili liste entiteta
            // doslo do smanjenja maksimalnog broja stranica,
            // potrebno je provjeriti da trenutni broj stranice
            // ne prelazi novi maksimalni broj stranice.
            if (PageNumber > MaxPageNumber) PageNumber = MaxPageNumber;

            OnPropertyChanged(nameof(NextPageExists));
            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(PageNumber));
            OnPropertyChanged(nameof(MaxPageNumber));
            OnPropertyChanged(nameof(CurrentPageText));
            return this;
        }

        private string PreviousSortingCriteria { get; set; } = string.Empty;
        private string SortingCriteria { get; set; } = string.Empty;

        private Direction PreviousSortingDirection { get; set; } = Direction.Ascending;
        private Direction SortingDirection { get; set; } = Direction.Ascending;
        
        
        private void Sort() => FilteredEntities = FilteredEntities.Sort(SortingCriteria, SortingDirection);

        public bool SetSortingParameters(string sortingCriteria, Direction sortingDirection)
        {
            if (string.IsNullOrWhiteSpace(sortingCriteria)) return false;
            SortingCriteria = sortingCriteria;
            SortingDirection = sortingDirection;
            return true;
        }

        public QueryController<T> PerformSorting()
        {
            // Promjena sortiranja resetuje paginaciju na pocetnu stranicu.
            if (!string.Equals(PreviousSortingCriteria, SortingCriteria) || PreviousSortingDirection != SortingDirection)
                PageNumber = 1;

            PreviousSortingDirection = SortingDirection;
            PreviousSortingCriteria = SortingCriteria;

            Sort();

            OnPropertyChanged(nameof(NextPageExists));
            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(PageNumber));
            OnPropertyChanged(nameof(MaxPageNumber));
            OnPropertyChanged(nameof(CurrentPageText));
            return this;
        }

        private List<T> Paginate(int pageNumber)
        {
            return FilteredEntities
                .Paginate(pageNumber, PageSize);
        }

        public List<T> GetWithPagination() => Paginate(PageNumber);

        public List<T> GetAllWithoutPagination() => FilteredEntities;

        public bool GoToNextPage()
        {
            if (!NextPageExists) return false;
            PageNumber = PageNumber + 1;
            OnPropertyChanged(nameof(PageNumber));
            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(NextPageExists));
            OnPropertyChanged(nameof(CurrentPageText));
            return true;
        }

        public bool GoToPreviousPage()
        {
            if (!PreviousPageExists) return false;
            PageNumber = PageNumber - 1;
            OnPropertyChanged(nameof(PageNumber));
            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(NextPageExists));
            OnPropertyChanged(nameof(CurrentPageText));
            return true;
        }

        public string CurrentPageText
        {
            get
            {
                if (MaxPageNumber > 0)
                    return $"{PageNumber} / {MaxPageNumber}";

                return "/";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
