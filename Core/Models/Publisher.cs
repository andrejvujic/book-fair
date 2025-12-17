using Core.Models;
using System.Collection.Generic;

namespace Core.Models;

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Author Boss { get; set; } // TODO: Najmanje 5 godina iskustva
    public List<Author> Authors { get; set; }

    public Publisher() { }

    public Publisher(int id, string name, string surname, Author boss, List<Author> authors)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Boss = boss;
        Authors = authors;
    }

    // TODO:
    // 1. Dodati spisak knjiga
    // 2. Ispisati spisak knjiga
    // 3. Ispisati spisak autora

    public override string ToString()
    {
        return
            "[Publisher]:" +
            $"\n\tId: {Id}" +
            $"\n\tName: {Name}" +
            $"\n\tSurname: {Surname}" +
            $"\n\tBoss: Name: {Boss.Name} Surname: {Boss.Surname}";

    }

}