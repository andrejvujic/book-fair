using Core.Models;
namespace Core.Models;

public class Purchase
{
    public Visitor Visitor { get; set; }
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

    // TODO:
    // 1. Add Book property

    public override string ToString()
    {
        return
            "[Purchase]:" +
            $"\n\tVisitor: Id: {Visitor.Id}, Name: {Visitor.FirstName}, Surname: {Visitor.LastName}" +
            $"\n\tDate: {Date}" +
            $"\n\tRating: {Rating}" +
            $"\n\tComment: {Comment}";
    }
}