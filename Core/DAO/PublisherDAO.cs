using Core.Models;

namespace Core.DAO;

public class PublisherDAO : DAO<Publisher>
{
    private static string _fileName = "Publishers.txt";
    public PublisherDAO() : base(_fileName)
    {
    }

    public override Publisher? Update(Publisher entity)
    {
        Publisher? oldEntity = _entities.Find(e => e.Id == entity.Id);
        if (oldEntity == null) return null;

        oldEntity.Id = entity.Id;
        oldEntity.Name = entity.Name;
        oldEntity.Surname = entity.Surname;
        oldEntity.Boss = entity.Boss;
        oldEntity.BossId = entity.BossId;
        oldEntity.Authors = entity.Authors;
        oldEntity.Books = entity.Books;

        _storage.Save(_entities);
        return oldEntity;
    }
}


