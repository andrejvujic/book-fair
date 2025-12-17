using Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Relations;
public class RelationPair : ISerializable
{
    public int FirstId { get; private set; }
    public int SecondId { get; private set; }

    public RelationPair() {}

    public RelationPair(int firstId, int secondId) {
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

