using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core.DAO;
using Core.Models;
using WPFClient.DTO;

namespace WPFClient.Controllers
{
    public class AuthorController: QueryController<Author>
    {
		private readonly AddressDAO _addressDAO;

		public AuthorController(AuthorDAO authorDAO, AddressDAO addressDAO): base(authorDAO) {
            _addressDAO = addressDAO;
        }

        public override int SearchQueryLength => 2;

        public void Delete(AuthorDTO author)
        {
            Address address = author.Address;
            _addressDAO.Remove(address.Id);
            _dao.Remove(author.Id);
            App.Context.RestoreRelations();

		}
    }
}
