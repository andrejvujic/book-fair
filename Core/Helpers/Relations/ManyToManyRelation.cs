using Core.Serialization;
using Core.Storage;

namespace Core.Helpers.Relations;
public class ManyToManyRelation<T1, T2>
   where T1 : IIdentifiable, IRelational
   where T2 : IIdentifiable, IRelational
{
    private List<RelationPair> allPairs = new List<RelationPair>();
    private readonly Storage<RelationPair> storage;

    public ManyToManyRelation(string storagePath) {
        storage = new Storage<RelationPair>(storagePath);
        allPairs = storage.Load();
    }

    public void Add(T1 firstEntity, T2 secondEntity, bool updateStorageImmediately = true) {
        if (Exists(firstEntity, secondEntity)) return;

        allPairs.Add(
            new RelationPair(firstEntity.Id, secondEntity.Id)
        );

        if (updateStorageImmediately)
            storage.Save(allPairs);
    }

    public void AddAll(T1 entity, List<T2> entities)
    {
        foreach (T2 e in entities)
            Add(entity, e, updateStorageImmediately: false);

        storage.Save(allPairs);
    }

    public void AddAll(T2 entity, List<T1> entities)
    {
        foreach (T1 e in entities)
            Add(e, entity, updateStorageImmediately: false);

        storage.Save(allPairs);
    }

    public void Connect(T1 entity, List<T2> allEntities, Action<T2> addCallback)
    {
        Dictionary<int, T2> allEntitesDictionary = new Dictionary<int, T2>();
        foreach (T2 e in allEntities)
            allEntitesDictionary[e.Id] = e;

        List<RelationPair> connectedPairs = allPairs.Where(pair => pair.FirstId == entity.Id).ToList();
        List<int> secondEntityIds = connectedPairs.ConvertAll(pair => pair.SecondId);

        foreach (int id in secondEntityIds)
            addCallback.Invoke(allEntitesDictionary[id]);
    }

    public void Connect(T2 entity, List<T1> allEntities, Action<T1> addCallback)
    {
        Dictionary<int, T1> allEntitesDictionary = new Dictionary<int, T1>();
        foreach (T1 e in allEntities)
            allEntitesDictionary[e.Id] = e;

        List<RelationPair> connectedPairs = allPairs.Where(pair => pair.SecondId == entity.Id).ToList();
        List<int> firstEntityIds = connectedPairs.ConvertAll(pair => pair.FirstId);

        foreach (int id in firstEntityIds)
            addCallback.Invoke(allEntitesDictionary[id]);
    }

    private bool Exists(T1 firstEntity, T2 secondEntity)
    {
        return Exists(firstEntity.Id, secondEntity.Id);
    }

    private bool Exists(int firstEntityId, int secondEntityId)
    {
        return allPairs.Exists(pair => pair.FirstId == firstEntityId && pair.SecondId == secondEntityId);
    }
}

