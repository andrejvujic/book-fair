using Core.DAO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Controllers
{
    public class BookController : QueryController<Book>
    {
        public BookController(BookDAO bookDAO) : base(bookDAO) { }

        public override int SearchQueryLength => 1;
    }
}
