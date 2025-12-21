// See https://aka.ms/new-console-template for more information
using Core.Models;
using Core.Helpers.Relations;
using Core.DAO;
using ConsoleClient.Views;

ManyToManyRelation<Visitor, Book> visitorWhislistedBooks =
    new ManyToManyRelation<Visitor, Book>("VisitorWishlistedBooks.txt");

PublisherDAO publisherDAO = new PublisherDAO();
VisitorDAO visitorDAO = new VisitorDAO(visitorWhislistedBooks);
PurchaseDAO purchaseDAO = new PurchaseDAO();

visitorDAO.ForEach(visitor => visitorWhislistedBooks.Connect(visitor, visitor.WishlistedBooks, visitor.WishlistedBooks.Add));

VisitorView visitorView = new VisitorView(visitorDAO);
PublisherView publisherView = new PublisherView(publisherDAO);
PurchaseView purchaseView = new PurchaseView(purchaseDAO);

const int EXIT = 0;
const int VISITOR_MENU = 1;
const int PUBLISHER_MENU = 2;
const int PURCHASE_MENU = 3;

while (true)
{
    Console.WriteLine("Book Fair Console App - please select an option:\n");
    Console.WriteLine("1 -> Visitor...");
    Console.WriteLine("2 -> Publisher...");
    Console.WriteLine("3 -> Purchase...");
    Console.WriteLine("0 -> Exit\n");

    int selectedOption = -1;

    Console.Write("> ");
    if (!int.TryParse(Console.ReadLine(), out selectedOption))
    {
        Console.Clear();
        Console.WriteLine("ERROR: Invalid input! Please select one of the available options.\n");
        continue;
    }

    Console.Clear();

    switch (selectedOption)
    {
        case VISITOR_MENU:
            visitorView.Run(); break;
        case PUBLISHER_MENU:
            publisherView.Run(); break;
        case PURCHASE_MENU:
            purchaseView.Run(); break;
        case EXIT:
            break;
        default:
            Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
            break;
    }

    if (selectedOption == EXIT) break;
}