using Core.DAO;
using Core.Models;
using System;

namespace ConsoleClient.Views;

public class PurchaseView : View<Purchase>
{
    public PurchaseView(PurchaseDAO purchaseDAO): base(purchaseDAO) {}

    protected override string[] EntityProperties
    {
        get
        {
            return new string[] {
                "ID", "Visitor ID", "Book ID", "Date", "Rating", "Comment"
            };
        }
    }

    protected override void ShowOptions()
    {
        Console.WriteLine("[Purchase] Available options:\n");

        Console.WriteLine("1 -> Show all");
        Console.WriteLine("2 -> Add");
        Console.WriteLine("3 -> Update");
        Console.WriteLine("4 -> Remove");
        Console.WriteLine("5 -> Show (with pagination and sorting)");
        Console.WriteLine("0 -> Back\n");
    }

    protected override void HandleAction(int selectedOption)
    {
        switch (selectedOption)
        {
            case SHOW_ALL:
                ShowAllAction();
                break;
            case ADD:
                AddEntityAction();
                break;
            case UPDATE:
                UpdateEntityAction();
                break;
            case REMOVE:
                RemoveEntityAction();
                break;
            case SHOW_WITH_PAGINATION_AND_SORTING:
                ShowPaginatedAndSortedAction();
                break;
            default:
                Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
                break;
        }
    }


    protected override Purchase InputEntity()
    {
        Console.Write("Enter visitor ID: ");
        int visitorId = int.Parse(Console.ReadLine());

        Console.Write("Enter book ID: ");
        int bookId = int.Parse(Console.ReadLine());

        Console.Write("Enter purchase date (yyyy-MM-dd): ");
        DateOnly date = DateOnly.Parse(Console.ReadLine());

        Console.Write("Enter rating (1-5): ");
        int rating = int.Parse(Console.ReadLine());

        Console.Write("Enter comment: ");
        string comment = Console.ReadLine();

        Visitor visitor = new Visitor();
        visitor.Id = visitorId;

        Book book = new Book();
        book.Id = bookId;

        Purchase purchase = new Purchase(
            visitor,
            book,
            date,
            rating,
            comment
        );

        return purchase;
    }

    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}
