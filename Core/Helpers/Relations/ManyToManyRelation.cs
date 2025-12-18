using Core.Serialization;
using Core.Storage;

namespace Core.Helpers.Relations;
public class ManyToManyRelation<T1, T2>
   where T1 : IIdentifiable
   where T2 : IIdentifiable
{

    // 'allPairsSet' se koristi za brzo pretrazivanje postojecih relacija
    // prilikom provjere da li je relacija vec zabiljezena (potrebno
    // radi sprecavanja dupliciranja podataka).
    private HashSet<(int, int)> allPairsSet = new HashSet<(int, int)>();
    private List<RelationPair> allPairs = new List<RelationPair>();
    private readonly Storage<RelationPair> storage;

    private List<T1> AllFirstEntities { get; }
    private List<T2> AllSecondEntities { get; }

    public ManyToManyRelation(string storagePath, List<T1> allFirstEntities, List<T2> allSecondEntities) {
        AllFirstEntities = allFirstEntities;
        AllSecondEntities = allSecondEntities;
        storage = new Storage<RelationPair>(storagePath);
        allPairs = storage.Load();
    }

    public void Add(T1 firstEntity, T2 secondEntity, bool updateStorageImmediately = true) {

        // Dodaje relaciju u 'allPairsSet', te istovremeno provjerava
        // da li je relacija vec zabiljezena. Nova relacija se dodaje samo ukoliko
        // vec nije dodana.
        if (!allPairsSet.Add((firstEntity.Id, secondEntity.Id)))
            return;

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

    public void Connect(T1 entity, Action<T2> addCallback)
    {
        Dictionary<int, T2> allSecondEntitiesDictionary = new Dictionary<int, T2>();
        foreach (T2 e in AllSecondEntities)
            allSecondEntitiesDictionary[e.Id] = e;

        List<RelationPair> connectedPairs = allPairs.Where(pair => pair.FirstId == entity.Id).ToList();
        List<int> secondEntityIds = connectedPairs.ConvertAll(pair => pair.SecondId);

        foreach (int id in secondEntityIds)
            addCallback.Invoke(allSecondEntitiesDictionary[id]);
    }

    public void Connect(T2 entity, Action<T1> addCallback)
    {
        Dictionary<int, T1> allFirstEntitiesDictionary = new Dictionary<int, T1>();
        foreach (T1 e in AllFirstEntities)
            allFirstEntitiesDictionary[e.Id] = e;

        List<RelationPair> connectedPairs = allPairs.Where(pair => pair.SecondId == entity.Id).ToList();
        List<int> firstEntityIds = connectedPairs.ConvertAll(pair => pair.FirstId);

        foreach (int id in firstEntityIds)
            addCallback.Invoke(allFirstEntitiesDictionary[id]);
    }
}

