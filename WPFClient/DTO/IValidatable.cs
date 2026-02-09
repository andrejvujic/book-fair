using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.DTO
{
    public interface IValidatable
    {
        public abstract bool AllFieldsValid { get; }
    }
}
