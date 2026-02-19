using Core.DAO;
using Core.Models;

namespace WPFClient.Controllers
{
    class PublisherController : Controller<Publisher>
    {
        public PublisherController(PublisherDAO publisherDAO) : base(publisherDAO)
        {
        }

        public void UpdateBoss(Publisher publisher, Author boss)
        {
            if (boss == null) return;
            publisher.BossId = boss.Id; 
            publisher.Boss = boss;
            Update(publisher);
        }
    }
}
