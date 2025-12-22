using Core.Models;
using Core.Serialization;

namespace Core.Helpers.Relations;

public class RelationPair : ISerializable
{
    public int FirstId { get; private set; }
    public int SecondId { get; private set; }

    public RelationPair() { }

    public RelationPair(int firstId, int secondId)
    {
        FirstId = firstId;
        SecondId = secondId;
    }

    public string[] ToCSV()
    {
        string[] csv = { FirstId.ToString(), SecondId.ToString() };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        FirstId = int.Parse(values[0]);
        SecondId = int.Parse(values[1]);
    }
}