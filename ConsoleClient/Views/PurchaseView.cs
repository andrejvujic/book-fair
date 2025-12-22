using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Helpers.TablePrinter;
using Core.Models;
using System;

namespace ConsoleClient.Views;

public class PurchaseView : View<Purchase>
{
    private readonly VisitorDAO _visitorDAO;
    private readonly BookDAO _bookDAO;
    public PurchaseView(PurchaseDAO purchaseDAO, VisitorDAO visitorDAO, BookDAO bookDAO): base(purchaseDAO) {
        _visitorDAO = visitorDAO;
        _bookDAO = bookDAO;
    }

    protected override void ShowOptions()
    {
        Console.WriteLine("[Purchase] Available options:\n");

        Console.WriteLine("1 -> Show all");
        Console.WriteLine("2 -> Add");
        Console.WriteLine("3 -> Update");
        Console.WriteLine("4 -> Remove");
        Console.WriteLine("5 -> Show (with pagination and sorting)");
        Console.WriteLine("6 -> Show details");
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
            case SHOW_DETAILS:
                ShowDetailsAction();
                break;
            default:
                Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
                break;
        }
    }


    protected override Purchase InputEntity()
    {
        Visitor visitor = InputVisitor();

        Book book = InputBook();

        Console.Write("Enter purchase date (yyyy-MM-dd): ");
        DateOnly date = DateOnly.Parse(ConsoleUtilities.ReadStringRequired());

        Console.Write("Enter rating (1-5): ");
        int rating = ConsoleUtilities.ReadIntRequired();

        Console.Write("Enter comment: ");
        string comment = ConsoleUtilities.ReadStringRequired();

        Purchase purchase = new Purchase(
            visitor,
            book,
            date,
            rating,
            comment
        );

        return purchase;
    }

    private void ShowDetailsAction()
    {
        Console.WriteLine($"[{_dao.Identifier}] Showing details:\n");

        Console.Write("Enter purchase ID: ");
        int id = ConsoleUtilities.ReadIntRequired();
        Purchase? purchase = _dao.GetById(id);

        Console.Write('\n');

        if (purchase == null)
        {
            Console.Clear();
            Console.WriteLine($"ERROR: Purchase with ID '{id}' doesn't exist.\n");
            return;
        }

        // Prikazuje osnovne podatke o kupovini za koju prikazujemo detalje.
        new TablePrinter<Purchase>(_dao.EntityProperties, new List<Purchase>() { purchase }).Print();

        new TablePrinter<Book>(_bookDAO.EntityProperties, new List<Book>() { purchase.Book }).Print();

        new TablePrinter<Visitor>(_visitorDAO.EntityProperties, new List<Visitor>() { purchase.Visitor }).Print();

    }

    private Visitor InputVisitor()
    {
        Visitor visitor = new Visitor();

        while (true)
        {
            Console.Write("Enter visitor ID: ");
            int visitorId = ConsoleUtilities.ReadIntRequired();
            if (ParseVisitorId(visitorId, out visitor)) break;
        }

        return visitor;
    }
    private bool ParseVisitorId(int visitorId, out Visitor visitor)
    {
        visitor = new Visitor();

        Visitor? visitorCandidate = _visitorDAO.GetById(visitorId);
        if (visitorCandidate == null)
        {
            Console.WriteLine($"ERROR: Visitor with ID '{visitorId}' doesn't exist.\n");
            return false;
        }

        visitor = visitorCandidate;
        return true;
    }

    private Book InputBook()
    {
        Book book = new Book();

        while (true)
        {
            Console.Write("Enter book ID: ");
            int bookId = ConsoleUtilities.ReadIntRequired();
            if (ParseBookId(bookId, out book)) break;
        }

        return book;
    }
    private bool ParseBookId(int bookId, out Book book)
    {
        book = new Book();

        Book? bookCandidate = _bookDAO.GetById(bookId);
        if (bookCandidate == null)
        {
            Console.WriteLine($"ERROR: Book with ID '{bookId}' doesn't exist.\n");
            return false;
        }

        book = bookCandidate;
        return true;
    }

    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
    private const int SHOW_DETAILS = 6;
}
