using Core.Helpers;
using Core.Serialization;
namespace Core.Models;

public class Purchase : IIdentifiable, ISerializable, IDeletable
{
    public int Id { get; set; }

    public int VisitorId { get; set;  }
    public Visitor Visitor { get; set; } = new Visitor();

    public int BookId { get; set; }
    public Book Book { get; set; } = new Book();

    public DateTime? Date { get; set; }
    public int Rating { get; set; } // TODO: Rating mora biti [1, 5]
    public string Comment { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public Purchase() { }

    public Purchase(Visitor visitor, Book book, DateTime date, int rating, string comment)
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
            (Date ?? DateTime.Now).ToString(),
            Rating.ToString(),
            Comment,
            IsDeleted.ToString(),
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        VisitorId = int.Parse(values[1]);
        BookId = int.Parse(values[2]);
        Date = DateTime.Parse(values[3]);
        Rating = int.Parse(values[4]);
        Comment = values[5];
        IsDeleted = values.Length > 6 ? bool.Parse(values[6]) : false;
    }
}