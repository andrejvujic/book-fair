using Core.DAO;
using Core.Models;

namespace WPFClient.Controllers
{
    class PublisherController : QueryController<Publisher>
    {
        public PublisherController(PublisherDAO publisherDAO) : base(publisherDAO)
        {
        }

        public override int SearchQueryLength => 2;

        public void UpdateBoss(Publisher publisher, Author boss)
        {
            if (boss == null) return;
            publisher.BossId = boss.Id; 
            publisher.Boss = boss;
            Update(publisher);
        }
    }
}
