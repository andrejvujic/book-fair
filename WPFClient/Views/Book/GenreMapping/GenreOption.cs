using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Views.Book.GenreMapping
{
    public class GenreOption
    {
        public BookGenre Value { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}
