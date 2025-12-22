using Core.Helpers;
using Core.Serialization;
namespace Core.Models;

public class Publisher : ISerializable, IIdentifiable
{
    public int Id { get; set; } = -1;

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public int BossId { get; set; } = 0;
    public Author Boss { get; set; } // TODO: Najmanje 5 godina iskustva

    public List<Author> Authors { get; set; } = new List<Author>();
    public List<Book> Books { get; set; } = new List<Book>();

    public Publisher() { }

    public Publisher(string firstName, string lastName, Author boss, List<Author> authors, List<Book> books)
    {
        FirstName = firstName;
        LastName = lastName;
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
            $"\n\tFirst name: {FirstName}" +
            $"\n\tLast name: {LastName}" +
            $"\n\tBoss: Id: {Boss.Id} Name: {Boss.FirstName} Surname: {Boss.LastName}" +
            $"\n\tAuthors: {Authors}" +
            $"\n\tBooks: {Books}";

    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            FirstName,
            LastName,
            BossId.ToString()
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        BossId = int.Parse(values[3]);
    }
}