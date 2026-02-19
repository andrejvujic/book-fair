namespace Core.Helpers.Pagination
{
    public static class PaginationExtension
    {
        public static List<T> Paginate<T>(this List<T> list, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return new List<T>();
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static int GetNumberOfPages<T>(this List<T> list, int pageSize)
        {
            if (pageSize < 1) return 0;
            return (int)Math.Ceiling(list.Count / (double)pageSize);
        }
    }
}
