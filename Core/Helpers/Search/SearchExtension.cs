using Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Search
{
    public static class SearchExtension
    {
        public static List<T> Search<T>(this List<T> entities, SearchQuery q) where T : ISearchable
        {
            if (q == null || q.IsEmpty) return entities;

            return entities.Where((entity) => {
                List<string> f = entity.SearchFields.Select((e) => e.ToLower().Trim()).ToList();

                for (int index = 0; index < Math.Min(q.Length, f.Count); ++index)
                    if (!f[index].Contains(q.Tokens[index])) return false;

                return true;
            }).ToList();
        }
    }
}
