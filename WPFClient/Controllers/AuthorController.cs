using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO;
using Core.Models;

namespace WPFClient.Controllers
{
    public class AuthorController: QueryController<Author>
    {
		private readonly AddressDAO _addressDAO;
		public AuthorController(AuthorDAO authorDAO, AddressDAO addressDAO): base(authorDAO) {
            _addressDAO = addressDAO;
        }

        public override int SearchQueryLength => 2;
    }
}
