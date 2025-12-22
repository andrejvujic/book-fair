using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Helpers.TablePrinter;
using Core.Models;

namespace ConsoleClient.Views;

public class PublisherView : View<Publisher>
{
    private readonly AuthorDAO _authorDAO;
    private readonly BookDAO _bookDAO;

    public PublisherView(PublisherDAO publisherDAO, AuthorDAO authorDAO, BookDAO bookDAO) : base(publisherDAO) {
        _authorDAO = authorDAO;
        _bookDAO = bookDAO;
    }

    protected override void ShowOptions()
    {
        Console.WriteLine("[Publisher] Available options:\n");

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

    protected override Publisher InputEntity()
    {
        Console.Write("Enter name: ");
        string name = ConsoleUtilities.ReadStringRequired();

        Author boss = InputBoss();

        List<Author> authors = new List<Author>();
        List<Book> books = new List<Book>();

        Publisher publisher = new Publisher(
            name,
            boss,
            authors,
            books
        );

        return publisher;
    }

    private void ShowDetailsAction()
    {
        Console.WriteLine($"[{_dao.Identifier}] Showing details:\n");

        Console.Write("Enter Publisher ID: ");
        int id = ConsoleUtilities.ReadIntRequired();
        Publisher? publisher = _dao.GetById(id);

        Console.Write('\n');

        if (publisher == null)
        {
            Console.Clear();
            Console.WriteLine($"ERROR: Publisher with ID '{id}' doesn't exist.\n");
            return;
        }

        // Prikazuje osnovne podatke o izdavacu za kojeg prikazujemo detalje.
        new TablePrinter<Publisher>(_dao.EntityProperties, new List<Publisher>() { publisher }).Print();

        // Prikazuje knjige koje je izdavac izdao.
        if (publisher.Books.Count > 0)
        {
            Console.WriteLine($"{publisher.Name} has published {publisher.Books.Count} book(s):\n");
            new TablePrinter<Book>(_bookDAO.EntityProperties, publisher.Books).Print();
        }
        else
            Console.WriteLine($"{publisher.Name} hasn't published any books.\n");

        // Prikazuje autore sa kojima je izdavac saradjivao (autore cije knjige je izdao).
        if (publisher.Authors.Count > 0)
        {
            Console.WriteLine($"{publisher.Name} has worked with {publisher.Authors.Count} author(s):\n");
            new TablePrinter<Author>(_authorDAO.EntityProperties, publisher.Authors).Print();
        }
        else
            Console.WriteLine($"{publisher.Name} hasn't worked with any authors.\n");

        if (publisher.Boss == null)
            Console.WriteLine($"{publisher.Name} doesn't have a boos.\n");
        else {
            Console.WriteLine($"The boss of {publisher.Name} is:\n");
            new TablePrinter<Author>(_authorDAO.EntityProperties, new List<Author>{ publisher.Boss }).Print();
        }
    }

    private Author InputBoss()
    {
        Author boss = new Author();

        while (true)
        {
            Console.Write("Enter boss ID: ");
            int bossId = ConsoleUtilities.ReadIntRequired();
            if (ParseBossId(bossId, out boss)) break;
        }

        return boss;
    }

    private bool ParseBossId(int bossId, out Author boss)
    {
        const int BOSS_MIN_YEARS_OF_EXPERIENCE = 5;
        boss = new Author();

        Author? bossCandidate = _authorDAO.GetById(bossId);
        if (bossCandidate == null)
        {
            Console.WriteLine($"ERROR: Author with ID '{bossId}' doesn't exist.\n");
            return false;
        }

        if (bossCandidate.YearsOfExperience < BOSS_MIN_YEARS_OF_EXPERIENCE)
        {
            Console.WriteLine($"ERROR: Minimum years of experience for a boss is 5. Author with ID '{bossId}' has only {bossCandidate.YearsOfExperience}.\n");
            return false;   
        }

        boss = bossCandidate;
        return true;
    }

    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
    private const int SHOW_DETAILS = 6;
}

