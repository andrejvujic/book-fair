using Core.Serialization;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Helpers.Relations;
public class ManyToManyRelation<T1, T2>
   where T1 : IIdentifiable, IRelational
   where T2 : IIdentifiable, IRelational
{
    private List<RelationPair> relationPairs = new List<RelationPair>();
    private readonly Storage<RelationPair> storage;

    ManyToManyRelation(string storagePath) {
        storage = new Storage<RelationPair>(storagePath);
        relationPairs = storage.Load();
    }

    public void Add(T1 firstEntity, T2 secondEntity, bool updateStorageImmediately = true) {
        if (Exists(firstEntity, secondEntity)) return;

        relationPairs.Add(
            new RelationPair(firstEntity.Id, secondEntity.Id)
        );

        if (updateStorageImmediately)
            storage.Save(relationPairs);
    }

    public void AddAll(T1 entity, List<T2> entities)
    {
        foreach (T2 e in entities)
            Add(entity, e, updateStorageImmediately: false);

        storage.Save(relationPairs);
    }

    public void AddAll(T2 entity, List<T1> entities)
    {
        foreach (T1 e in entities)
            Add(e, entity, updateStorageImmediately: false);

        storage.Save(relationPairs);
    }

    private bool Exists(T1 firstEntity, T2 secondEntity)
    {
        return Exists(firstEntity.Id, secondEntity.Id);
    }

    private bool Exists(int firstEntityId, int secondEntityId)
    {
        return relationPairs.Exists(pair => pair.FirstId == firstEntityId && pair.SecondId == secondEntityId);
    }
}

