
using Core.Helpers;
using Core.Storage;
using Core.Serialization;
using Core.Observer;

namespace Core.DAO;

public enum SortingDirection { Ascending, Descending }

public abstract class DAO<T>
        where T : class, ISerializable, IIdentifiable, new()
{
    public Subject Subject { get; }

    public abstract string Identifier { get; }
    public abstract string[] EntityProperties { get; }

    protected readonly List<T> _entities;
    protected readonly Storage<T> _storage;

    public DAO(string fileName)
    {
        Subject = new Subject();
        _storage = new Storage<T>(fileName);
        _entities = _storage.Load();
    }

    protected int GenerateId()
    {
        if (_entities.Count == 0) return 0;
        int maxId = _entities.Max(e => e.Id);
        return maxId + 1; 
    }
    public virtual int Add(T entity)
    {
        entity.Id = GenerateId();
        _entities.Add(entity);
        _storage.Save(_entities);
        Subject.NotifyObservers();
        return entity.Id;
    }

    public virtual T? Remove(int id)
    {
        T? entity = _entities.Find(e => e.Id == id);
        if (entity == null) return null;

        _entities.Remove(entity);
        _storage.Save(_entities);
        Subject.NotifyObservers();

        return entity;
    }

    public abstract T? Update(T entity);

    public int Count() => _entities.Count;

    public List<T> GetAll()
    {
        return _entities;
    }

    public List<T> GetWhere(Predicate<T> predicate)
    {
        return _entities.Where((e) => predicate(e)).ToList();
    }

    public T? GetById(int id)
    {
        return _entities.Find(e => e.Id == id);
    }

    public void ForEach(Action<T> callback)
    {
        foreach (T entity in _entities)
            callback.Invoke(entity);
    }

    protected abstract Dictionary<string, IOrderedEnumerable<T>> SortCriteriaMapping { get; }

    public List<T> Get(int page, int pageSize, string sortingCriteria, SortingDirection sortingDirection)
    {
        IEnumerable<T> entities = _entities;

        if (SortCriteriaMapping.ContainsKey(sortingCriteria))
            entities = SortCriteriaMapping[sortingCriteria];
        else
            Console.WriteLine($"WARNING: '{Identifier}' can't be sorted by '{sortingCriteria}'.\n");

        if (sortingDirection == SortingDirection.Descending)
            entities = entities.Reverse();

        entities = entities.Skip((page - 1) * pageSize).Take(pageSize);

        return entities.ToList();
    }
}
