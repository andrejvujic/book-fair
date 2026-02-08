using Core.DAO;
using Core.Models;
using System.Collections.Generic;
using System.Windows.Documents;

namespace WPFClient.Controllers
{
    public class VisitorController: Controller<Visitor>
    {
        private readonly AddressDAO _addressDAO;
        public VisitorController(VisitorDAO visitorDAO, AddressDAO addressDAO) : base(visitorDAO) {
            _addressDAO = addressDAO;
        }

        public List<Address> GetAllAddresses()
        {
            return _addressDAO.GetAll();
        }
    }
}
