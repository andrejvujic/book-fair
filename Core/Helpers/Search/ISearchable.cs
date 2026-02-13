using Core.Serialization;

namespace Core.Helpers.Search
{
    public interface ISearchable
    {
        public abstract List<string> SearchFields { get; }
    }
}
