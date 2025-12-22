using Core.DAO;
using Core.Models;

namespace ConsoleClient;

public class RelationManager
{
    private readonly AuthorDAO _authorDAO;
    private readonly BookDAO _bookDAO;
    private readonly PublisherDAO _publisherDAO;
    private readonly PurchaseDAO _purchaseDAO;
    private readonly AddressDAO _addressDAO;
    private readonly VisitorDAO _visitorDAO;

    public RelationManager(AuthorDAO authorDAO, BookDAO bookDAO, PublisherDAO publisherDAO, VisitorDAO visitorDAO, AddressDAO addressDAO, PurchaseDAO purchaseDAO)
    {
        _authorDAO = authorDAO;
        _bookDAO = bookDAO;
        _publisherDAO = publisherDAO;
        _visitorDAO = visitorDAO;
        _purchaseDAO = purchaseDAO;
        _addressDAO = addressDAO;
    }

    public void ConnectAll()
    {
        ConnectAuthorsWithAddresses(_authorDAO, _addressDAO);
        ConnectBooksWithPublishers(_bookDAO, _publisherDAO);
        ConnectPurchasesWithVisitorsAndBooks(_purchaseDAO, _visitorDAO, _bookDAO);
        ConnectVisitorsWithPurchasedBooks(_purchaseDAO, _visitorDAO); // Ovo se mora pozvati nakon sto se kupovine povezu sa knjigama.
        ConnectBooksWithBuyers(_bookDAO, _purchaseDAO);
        ConnectPublishersWithBosses(_publisherDAO, _authorDAO);
        ConnectPublishersWithAuthorsAndBooks(_publisherDAO, _bookDAO); // Ovo se mora pozvati nakon sto se knjige povezu sa autorima.
        ConnectVisitorsWithAddresses(_visitorDAO, _addressDAO);
    }
    private void ConnectPurchasesWithVisitorsAndBooks(PurchaseDAO purchaseDAO, VisitorDAO visitorDAO, BookDAO bookDAO)
    {
        List<Visitor> allVisitors = visitorDAO.GetAll();
        List<Book> allBooks = bookDAO.GetAll();

        purchaseDAO.ForEach(purchase =>
            {
                Visitor? visitor = allVisitors.Find(visitor => visitor.Id == purchase.VisitorId);
                if (visitor == null)
                    throw new Exception($"FATAL ERROR: Visitor with ID '{purchase.VisitorId}' doesn't exist. Resolving relations failed.");

                Book? book = allBooks.Find(book => book.Id == purchase.BookId);
                if (book == null)
                    throw new Exception($"FATAL ERROR: Book with ID '{purchase.BookId}' doesn't exist. Resolving relations failed.");

                purchase.Book = book;
                purchase.Visitor = visitor;
            }
        );
    }

    private void ConnectVisitorsWithPurchasedBooks(PurchaseDAO purchaseDAO, VisitorDAO visitorDAO)
    {
        List<Purchase> allPurchases = purchaseDAO.GetAll();
        visitorDAO.ForEach(visitor =>
            {
                visitor.PurchasedBooks = new List<Book>();
                List<Purchase> visitorPurchases = allPurchases.Where(purchase => purchase.VisitorId == visitor.Id).ToList();

                int sumOfAllRatings = 0;
                foreach (Purchase visitorPurchase in visitorPurchases)
                {
                    sumOfAllRatings = sumOfAllRatings + visitorPurchase.Rating;
                    visitor.PurchasedBooks.Add(visitorPurchase.Book);
                }
                int numberOfPurchasedBooks = visitor.PurchasedBooks.Count;
                visitor.AverageReviewRating = Math.Round(numberOfPurchasedBooks > 0 ? sumOfAllRatings / (double)(visitor.PurchasedBooks.Count) : 0, 2);
            }
        );
    }

    private void ConnectPublishersWithBosses(PublisherDAO publisherDAO, AuthorDAO authorDAO)
    {
        List<Author> allAuthors = authorDAO.GetAll();

        publisherDAO.ForEach(publisher =>
            {
                Author? boss = allAuthors.Find(author => author.Id == publisher.BossId);

                if (boss == null)
                    throw new Exception($"FATAL ERROR: Author with ID '{publisher.BossId}' doesn't exist. Resolving relations failed.");

                publisher.Boss = boss;
            }
        );
    }

    private void ConnectPublishersWithAuthorsAndBooks(PublisherDAO publisherDAO, BookDAO bookDAO)
    {
        List<Book> allBooks = bookDAO.GetAll();

        publisherDAO.ForEach(publisher =>
            {
                publisher.Authors = new List<Author>();
                publisher.Books = allBooks.Where(book => book.PublisherId == publisher.Id).ToList();
                foreach (Book book in publisher.Books)
                {
                    foreach (Author author in book.Authors)
                    {
                        if (publisher.Authors.Contains(author)) continue;
                        publisher.Authors.Add(author);
                    }
                }
            }
        );
    }

    private void ConnectVisitorsWithAddresses(VisitorDAO visitorDAO, AddressDAO addressDAO)
    {
        visitorDAO.ForEach(visitor =>
            {
                List<Address> allAddresses = addressDAO.GetAll();
                Address? address = allAddresses.Find(address => address.Id == visitor.AddressId);
                if (address == null)
                    throw new Exception($"FATAL ERROR: Address with ID '{visitor.AddressId}' doesn't exist. Resolving relations failed.");

                visitor.Address = address;
            }
        );
    }

    private void ConnectAuthorsWithAddresses(AuthorDAO authorDAO, AddressDAO addressDAO)
    {
        List<Address> allAddresses = addressDAO.GetAll();

        authorDAO.ForEach(author =>
            {

                Address? address = allAddresses.Find(address => address.Id == author.AddressId);

                if (address == null)
                    throw new Exception($"FATAL ERROR: Address with ID '{author.AddressId}' doesn't exist. Resolving relations failed.");

                author.Address = address;
            }
        );
    }


    private void ConnectBooksWithPublishers(BookDAO bookDAO, PublisherDAO publisherDAO)
    {
        List<Publisher> allPublishers = publisherDAO.GetAll();

        bookDAO.ForEach(book =>
            {
                Publisher? publisher = allPublishers.Find(publisher => publisher.Id == book.PublisherId);

                if (publisher == null)
                    throw new Exception($"FATAL ERROR: Address with ID '{book.PublisherId}' doesn't exist. Resolving relations failed.");

                book.Publisher = publisher;
            }
        );
    }

    private void ConnectBooksWithBuyers(BookDAO bookDAO, PurchaseDAO purchaseDAO)
    {
        List<Purchase> allPurchases = purchaseDAO.GetAll();

        bookDAO.ForEach(book =>
            {
                book.Buyers = new List<Visitor>();
                List<Purchase> bookPurchases = allPurchases.Where(purchase => purchase.BookId == book.Id).ToList();
                foreach (Purchase purchase in bookPurchases)
                {
                    if (book.Buyers.Contains(purchase.Visitor)) continue;
                    book.Buyers.Add(purchase.Visitor);
                }
            }
        );
    }

}
