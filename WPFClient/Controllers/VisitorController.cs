using Core.DAO;
using Core.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WPFClient.DTO;

namespace WPFClient.Controllers
{
    public class VisitorController: QueryController<Visitor>
    {
        private readonly AddressDAO _addressDAO;
        private readonly PurchaseDAO _purchaseDAO;

        public VisitorController(VisitorDAO visitorDAO, AddressDAO addressDAO, PurchaseDAO purchaseDAO)
            : base(visitorDAO)
        {
            _addressDAO = addressDAO;
            _purchaseDAO = purchaseDAO;
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

            Address address = visitor.Address;
            _addressDAO.Remove(address.Id);

            _dao.Remove(visitor.Id);

            App.Context.RestoreRelations();
        }
    }
}
