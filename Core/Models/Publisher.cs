using Core.Helpers;
using Core.Helpers.Relations;
using Core.Serialization;
namespace Core.Models;

public class Publisher : ISerializable, IIdentifiable
{
    public int Id { get; set; } = -1;

    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";

    public int BossId { get; set; } = 0;
    public Author Boss { get; set; } // TODO: Najmanje 5 godina iskustva

    public List<Author> Authors { get; set; }
    public List<Book> Books { get; set; }

    public Publisher() { }

    public Publisher(string name, string surname, Author boss, List<Author> authors, List<Book> books)
    {
        Name = name;
        Surname = surname;
        Boss = boss;
        BossId = boss.Id;
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
            $"\n\tBoss: Id: {Boss.Id} Name: {Boss.FirstName} Surname: {Boss.Surname}" +
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
            BossId.ToString()
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Name = values[1];
        Surname = values[2];
        BossId = int.Parse(values[3]);
    }
}