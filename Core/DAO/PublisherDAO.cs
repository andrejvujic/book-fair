using Core.Models;

namespace Core.DAO;

public class PublisherDAO : DAO<Publisher>
{
    private static string _fileName = "Publishers.txt";

    public override string Identifier { get { return "Publisher"; } }

    public PublisherDAO() : base(_fileName)
    {
    }

    public override Publisher? Update(Publisher entity)
    {
        Publisher? oldEntity = _entities.Find(e => e.Id == entity.Id);
        if (oldEntity == null) return null;

        oldEntity.Id = entity.Id;
        oldEntity.FirstName = entity.FirstName;
        oldEntity.LastName = entity.LastName;
        oldEntity.Boss = entity.Boss;
        oldEntity.BossId = entity.BossId;
        oldEntity.Authors = entity.Authors;
        oldEntity.Books = entity.Books;

        _storage.Save(_entities);
        return oldEntity;
    }


    protected override Dictionary<string, IOrderedEnumerable<Publisher>> SortCriteriaMapping
    {
        get
        {
            return new Dictionary<string, IOrderedEnumerable<Publisher>>
            {
                { "Id", _entities.OrderBy(x => x.Id) },
                { "Name", _entities.OrderBy(x => x.FirstName) },
                { "Surname", _entities.OrderBy(x => x.LastName) },
                { "BossId", _entities.OrderBy(x => x.BossId) }
            };
        }
    }
}


