using Core.Helpers;
using Core.Helpers.Relations;
using Core.Serialization;
namespace Core.Models;

public class Publisher : ISerializable, IIdentifiable, IRelational
{
    public int RelationId { get; } = RelationIds.Publisher;
    public int Id { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public Author Boss { get; set; } // TODO: Najmanje 5 godina iskustva
    public List<Author> Authors { get; set; }
    public List<Book> Books { get; set; }

    public Publisher() { }

    public Publisher(int id, string name, string surname, Author boss, List<Author> authors, List<Book> books)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Boss = boss;
        Authors = authors;
        Books = books;
    }

    public override string ToString()
    {
        return
            "[Publisher]:" +
            $"\n\tId: {Id}" +
            $"\n\tName: {Name}" +
            $"\n\tSurname: {Surname}" +
            $"\n\tBoss: Name: {Boss.Name} Surname: {Boss.Surname}" +
            $"\n\tAuthors: {Authors}" +
            $"\n\tBooks: {Books}";

    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            Name,
            Surname,
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Name = values[1];
        Surname = values[2];
    }
}