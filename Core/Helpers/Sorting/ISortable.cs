using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Sorting
{
    public interface ISortable
    {
        public abstract Dictionary<string, dynamic> SortFields { get; }
    }
}
