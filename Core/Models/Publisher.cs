using Core.Helpers;
using Core.Serialization;
using System.Reflection.Metadata.Ecma335;
namespace Core.Models;

public class Publisher : ISerializable, IIdentifiable, IDeletable
{
    public int Id { get; set; } = 0;

    public bool IsDeleted { get; set; }

    public int Code { get; set; } = -1;

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
            BossId.ToString(),
            Code.ToString(),
            IsDeleted.ToString()
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Name = values[1];
        BossId = int.Parse(values[2]);
        Code = int.Parse(values[3]);
        IsDeleted = values.Length > 4 ? bool.Parse(values[4]) : false;
    }
}