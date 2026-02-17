using System.Diagnostics;

namespace Core.Helpers.Sorting
{
    public static class SortingExtension
    {
        public static List<T> Sort<T>(this List<T> entities, string criteria, Direction direction) where T : ISortable
        {
            IEnumerable<T> sorted = entities.OrderBy((entity) => {
            
                if (!entity.SortFields.ContainsKey(criteria)) return entity.SortFields.Values.FirstOrDefault();
                return entity.SortFields[criteria];
            });
            if (direction == Direction.Descending) sorted = sorted.Reverse();
            return sorted.ToList();
        }
    }

    public enum Direction { Ascending, Descending }
}
