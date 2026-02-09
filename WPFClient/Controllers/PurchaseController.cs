using Core.DAO;
using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace WPFClient.Controllers
{
    public class PurchaseController : Controller<Purchase>
    {
        public PurchaseController(PurchaseDAO purchaseDAO) : base(purchaseDAO)
        {
        }

        public List<Purchase> GetByVisitorId(int visitorId)
        {
            List<Purchase> allPurchases = _dao.GetAll();
            return allPurchases.Where((entity) => entity.VisitorId == visitorId).ToList();
        }

        public double GetTotalSpentByVisitorId(int visitorId)
        {
            List<Purchase> purchases = GetByVisitorId(visitorId);
            return purchases.Sum(p => p.Book.Price);
        }

        public double GetAverageRatingByVisitorId(int visitorId)
        {
            List<Purchase> purchases = GetByVisitorId(visitorId);
            if (purchases.Count == 0) return 0;
            int totalRating = purchases.Sum(p => p.Rating);
            return totalRating / (double)purchases.Count;
        }
    }
}
