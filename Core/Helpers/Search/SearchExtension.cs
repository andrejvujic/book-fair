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
            if (q.Empty) return entities;
            return entities.Where((entity) => entity.IsMatch(q)).ToList();
        }
    }
}
