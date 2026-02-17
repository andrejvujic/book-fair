namespace Core.Helpers.Pagination
{
    public static class PaginationExtension
    {
        public static List<T> Paginate<T>(this List<T> list, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Broj stranice mora biti veci od 0.");
            if (pageSize < 1) throw new ArgumentException("Broj elemenata po stranici mora biti veci 0.");
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static int GetNumberOfPages<T>(this List<T> list, int pageSize)
        {
            if (pageSize < 1) throw new ArgumentException("Broj elemenata po stranici mora biti veci 0.");
            return (int)Math.Ceiling(list.Count / (double)pageSize);
        }
    }
}
