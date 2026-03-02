using Core.DAO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClient.DTO;
using WPFClient.Views.Publisher;

namespace WPFClient.Controllers
{
    public class BookController : QueryController<Book>
    {
        private readonly PurchaseDAO _purchaseDAO;  
		public BookController(BookDAO bookDAO, PurchaseDAO purchaseDAO) : base(bookDAO) {
            _purchaseDAO = purchaseDAO;
		}

        public override int SearchQueryLength => 1;

        public void Delete(BookDTO book)
        {
            List<Purchase> purchases = _purchaseDAO.GetWhere(p => p.BookId == book.Id);
            foreach (int id in purchases.Select(p => p.Id))
                _purchaseDAO.Remove(id);
            _dao.Remove(book.Id);
            App.Context.RestoreRelations();
        }
    }
}
