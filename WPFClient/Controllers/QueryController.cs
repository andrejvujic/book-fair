using Core.DAO;
using Core.Helpers;
using Core.Helpers.Pagination;
using Core.Helpers.Search;
using Core.Helpers.Sorting;
using Core.Serialization;
using System.Collections.Generic;
using System.ComponentModel;


namespace WPFClient.Controllers
{
    public abstract class QueryController<T> : Controller<T>, INotifyPropertyChanged
        where T : class, ISerializable, IIdentifiable, ISearchable, ISortable, IDeletable, new()
    {
        public QueryController(DAO<T> dao) : base(dao) { }

        private List<T> FilteredEntities { get; set; } = new List<T>();

        public abstract int SearchQueryLength { get; }

        private const int PageSize = 16;
        public int MaxPageNumber { get => FilteredEntities.GetNumberOfPages(PageSize); }
        public int PageNumber { get; private set; } = 1;
        
        public bool PreviousPageExists { get => PageNumber - 1 >= 1; }
        public bool NextPageExists { get => PageNumber + 1 <= MaxPageNumber; }

        private void Search(string rawSearchQuery, bool excludeDeleted = true) {
            // Promjena pretrage resetuje paginaciju na pocetnu stranicu.
            PageNumber = 1;

            FilteredEntities = _dao
                .GetWhere((e) => !e.IsDeleted || !excludeDeleted)
                .Search(
                    new SearchQuery(
                        rawSearchQuery ?? string.Empty,
                        SearchQueryLength
                    )
                );

            OnPropertyChanged(nameof(NextPageExists));
            OnPropertyChanged(nameof(PreviousPageExists));
            OnPropertyChanged(nameof(PageNumber));
            OnPropertyChanged(nameof(MaxPageNumber));
            OnPropertyChanged(nameof(CurrentPageText));
        }

        public QueryController<T> PerformSerach(string rawSearchQuery, bool excludeDeleted = true) {
            Search(rawSearchQuery, excludeDeleted);

            // Ukoliko je zbog promjene search query-ja ili liste entiteta
            // doslo do smanjenja maksimalnog broja stranica,
            // potrebno je provjeriti da trenutni broj stranice
            // ne prelazi novi maksimalni broj stranice.
            if (PageNumber > MaxPageNumber) PageNumber = MaxPageNumber;

            return this;
        }


        private string SortingCriteria { get; set; } = string.Empty;
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
            // Sortiranje resetuje paginaciju na pocetnu stranicu.
            PageNumber = 1;

            Sort();

            OnPropertyChanged(nameof(PageNumber));
            OnPropertyChanged(nameof(CurrentPageText));
            return this;
        }

        private List<T> Paginate(int pageNumber)
        {
            return FilteredEntities
                .Paginate(pageNumber, PageSize);
        }

        public List<T> Get() => Paginate(PageNumber);

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
                    return $"Trenutna stranica — {PageNumber} / {MaxPageNumber}";

                return "Nema entiteta za prikazivanje";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
