using Core.Helpers;
using Core.Serialization;
namespace Core.Models;

public class Purchase : IIdentifiable, ISerializable
{
    public int Id { get; set; }

    public int VisitorId { get; set;  }
    public Visitor Visitor { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    public DateOnly Date { get; set; }
    public int Rating { get; set; } // TODO: Rating mora biti [1, 5]
    public string Comment { get; set; }

    public Purchase() { }

    public Purchase(Visitor visitor, Book book, DateOnly date, int rating, string comment)
    {
        Visitor = visitor;
        VisitorId = visitor.Id;
        Book = book;
        BookId = book.Id;
        Date = date;
        Rating = rating;
        Comment = comment;
    }

    public override string ToString()
    {
        return
            "[Purchase]:" +
            $"\n\tVisitor: Id: {Visitor.Id}, Name: {Visitor.FirstName}, Surname: {Visitor.LastName}" +
            $"\n\tBook: Id: {Book.Id}, Title: {Book.Title}" +
            $"\n\tDate: {Date}" +
            $"\n\tRating: {Rating}" +
            $"\n\tComment: {Comment}";
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            VisitorId.ToString(),
            BookId.ToString(),
            Date.ToString(),
            Rating.ToString(),
            Comment,
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        VisitorId = int.Parse(values[1]);
        BookId = int.Parse(values[2]);
        Date = DateOnly.Parse(values[3]);
        Rating = int.Parse(values[4]);
        Comment = values[5];
    }
}