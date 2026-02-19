using Core.DAO;
using Core.Models;
using System.Collections.Generic;

namespace WPFClient.Controllers
{
    public class VisitorController: QueryController<Visitor>
    {
        private readonly AddressDAO _addressDAO;

        public VisitorController(VisitorDAO visitorDAO, AddressDAO addressDAO) : base(visitorDAO) {
            _addressDAO = addressDAO;
        }

        // Pretraga za posjetioce moze imati maksimalno 3 tokena.
        public override int SearchQueryLength => 3;

        public List<Address> GetAllAddresses()
        {
            return _addressDAO.GetAll();
        }
    }
}
