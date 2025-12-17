using Core.Models;
namespace Core.Models;

public class Purchase
{
    public Visitor Visitor { get; set; }
    public Book Book { get; set; }
    public DateOnly Date { get; set; }
    public int Rating { get; set; } // TODO: Rating mora biti [1, 5]
    public string Comment { get; set; }

    public Purchase() { }

    public Purchase(Visitor visitor, DateOnly date, int rating, string comment)
    {
        Visitor = visitor;

        Date = date;
        Rating = rating;
        Comment = comment;
    }

    public override string ToString()
    {
        return
            "[Purchase]:" +
            $"\n\tVisitor: Id: {Visitor.Id}, Name: {Visitor.FirstName}, Surname: {Visitor.LastName}" +
            $"\n\tBook: ISBN: {Book.Isbn}, Title: {Book.BookTitle}" +
            $"\n\tDate: {Date}" +
            $"\n\tRating: {Rating}" +
            $"\n\tComment: {Comment}";
    }
}