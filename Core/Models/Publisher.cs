using Core.Helpers;
using Core.Serialization;
using System.Reflection.Metadata.Ecma335;
namespace Core.Models;

public class Publisher : ISerializable, IIdentifiable
{
    public int Id { get; set; } = 0;

    public string Name { get; set; } = string.Empty;

    public int BossId { get; set; } = -1;
    public Author Boss { get; set; } = new Author(); // Najmanje 5 godina iskustva

    public List<Author> Authors { get; set; } = new List<Author>();
    public List<Book> Books { get; set; } = new List<Book>();

    public Publisher() { }

    public Publisher(string name, Author boss, List<Author> authors, List<Book> books)
    {
        Name = name;
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
            $"\n\tBoss: Id: {Boss.Id} Name: {Boss.FirstName} Surname: {Boss.LastName}" +
            $"\n\tAuthors: {Authors}" +
            $"\n\tBooks: {Books}";

    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            Name,
            BossId.ToString()
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Name = values[1];
        BossId = int.Parse(values[2]);
    }
}