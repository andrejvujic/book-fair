
using Core.Helpers;
using Core.Storage;
using Core.Serialization;

namespace Core.DAO;

public abstract class DAO<T>
        where T : class, ISerializable, IIdentifiable, new()
{
    protected readonly List<T> _entities;
    protected readonly Storage<T> _storage;

    public DAO(string fileName)
    {
        _storage = new Storage<T>(fileName);
        _entities = _storage.Load();
    }

    protected int GenerateId()
    {
        if (_entities.Count == 0) return 0;
        int maxId = _entities.Max(e => e.Id);
        return maxId + 1; 
    }
    public void Add(T entity)
    {
        entity.Id = GenerateId();
        _entities.Add(entity);
        _storage.Save(_entities);
    }

    public T? Remove(int id)
    {
        T? entity = _entities.Find(e => e.Id == id);
        if (entity == null) return null;

        _entities.Remove(entity);
        _storage.Save(_entities);

        return entity;
    }

    public abstract T? Update(T entity);

    public List<T> GetAll()
    {
        return _entities;
    }

    public T? GetById(int id)
    {
        return _entities.Find(e => e.Id == id);
    }
    
}
