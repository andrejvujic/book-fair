using Core.DAO;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using WPFClient.DTO;

namespace WPFClient.Controllers
{
    public class VisitorController: QueryController<Visitor>
    {
        private readonly AddressDAO _addressDAO;
        private readonly PurchaseDAO _purchaseDAO;
        private readonly BookDAO _bookDAO;

        public VisitorController(VisitorDAO visitorDAO, AddressDAO addressDAO, BookDAO bookDAO, PurchaseDAO purchaseDAO)
            : base(visitorDAO)
        {
            _addressDAO = addressDAO;
            _purchaseDAO = purchaseDAO;
            _bookDAO = bookDAO;
        }

        // Pretraga za posjetioce moze imati maksimalno 3 tokena.
        public override int SearchQueryLength => 3;

        public List<Address> GetAllAddresses()
        {
            return _addressDAO.GetAll();
        }

        public void Delete(VisitorDTO visitor)
        {
            List<Purchase> purchases = _purchaseDAO.GetWhere(p => p.VisitorId == visitor.Id);
            foreach (int id in purchases.Select(p => p.Id))
                _purchaseDAO.Remove(id);


            foreach (Book book in visitor.WishlistedBooks)
            {
                book.Wishlisters.RemoveAll(v => v.Id == visitor.Id);
                _bookDAO.Update(book);
            }


            foreach (Book book in visitor.PurchasedBooks)
            {
                book.Buyers.RemoveAll(v => v.Id == visitor.Id);
                _bookDAO.Update(book);
            }

            _addressDAO.Remove(visitor.AddressId);

            base.Delete(visitor.Id);
        }
    }
}
