namespace Core.Helpers.Search
{
    public interface ISearchable
    {
        public abstract bool IsMatch(SearchQuery query);
    }
}
