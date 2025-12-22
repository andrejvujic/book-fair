// See https://aka.ms/new-console-template for more information
using Core.Models;
using Core.Helpers.Relations;
using Core.DAO;
using ConsoleClient.Views;

ManyToManyRelation<Visitor, Book> visitorWhislistedBooks = new ManyToManyRelation<Visitor, Book>("VisitorWishlistedBooks.txt");
ManyToManyRelation<Author, Book> authorWrittenBooks = new ManyToManyRelation<Author, Book>("AuthorWrittenBooks.txt");

PublisherDAO publisherDAO = new PublisherDAO();
VisitorDAO visitorDAO = new VisitorDAO(visitorWhislistedBooks);
PurchaseDAO purchaseDAO = new PurchaseDAO();
AddressDAO addressDAO = new AddressDAO();
AuthorDAO authorDAO = new AuthorDAO(authorWrittenBooks);
BookDAO bookDAO = new BookDAO(authorWrittenBooks, visitorWhislistedBooks);

visitorDAO.ForEach(visitor => visitorWhislistedBooks.Connect(visitor, bookDAO.GetAll(), visitor.WishlistedBooks.Add));
authorDAO.ForEach(author => authorWrittenBooks.Connect(author, bookDAO.GetAll(), author.Books.Add));
bookDAO.ForEach(book => authorWrittenBooks.Connect(book, authorDAO.GetAll(), book.Authors.Add));
bookDAO.ForEach(book => visitorWhislistedBooks.Connect(book, visitorDAO.GetAll(), book.Buyers.Add));

VisitorView visitorView = new VisitorView(visitorDAO, bookDAO);
PublisherView publisherView = new PublisherView(publisherDAO);
PurchaseView purchaseView = new PurchaseView(purchaseDAO);
AddressView addressView = new AddressView(addressDAO);
AuthorView authorView = new AuthorView(authorDAO,addressDAO,bookDAO);
BookView bookView = new BookView(bookDAO);

const int EXIT = 0;
const int VISITOR_MENU = 1;
const int PUBLISHER_MENU = 2;
const int PURCHASE_MENU = 3;
const int ADDRESS_MENU = 4;
const int AUTHOR_MENU = 5;
const int BOOK_MENU = 6;

while (true)
{
    Console.WriteLine("Book Fair Console App - please select an option:\n");
    Console.WriteLine("1 -> Visitor...");
    Console.WriteLine("2 -> Publisher...");
    Console.WriteLine("3 -> Purchase...");
    Console.WriteLine("4 -> Address...");
    Console.WriteLine("5 -> Author...");
    Console.WriteLine("6 -> Book...");
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
        case ADDRESS_MENU: 
            addressView.Run();break;
        case AUTHOR_MENU:
            authorView.Run(); break;
        case BOOK_MENU:
            bookView.Run(); break;
        case EXIT:
            break;
        default:
            Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
            break;
    }

    if (selectedOption == EXIT) break;
}