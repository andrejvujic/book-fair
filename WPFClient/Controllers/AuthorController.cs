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
        private readonly BookDAO _bookDAO;
		public AuthorController(AuthorDAO authorDAO, AddressDAO addressDAO, BookDAO bookDAO): base(authorDAO) {
            _addressDAO = addressDAO;
            _bookDAO = bookDAO;
        }

        public override int SearchQueryLength => 2;

        public void Delete(AuthorDTO author)
        {
            foreach(Book book in author.Books)
            {
                book.Authors.RemoveAll(a => a.Id == author.Id);
                _bookDAO.Update(book);
            }

            Address address = author.Address;
            address.IsDeleted = true;
            _addressDAO.Update(address);

            author.IsDeleted = true;
            base.Update(author.ToModel());

		}
    }
}
