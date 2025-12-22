using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views;

public class PublisherView : View<Publisher>
{
    public PublisherView(PublisherDAO publisherDAO) : base(publisherDAO) { }

    protected override void ShowOptions()
    {
        Console.WriteLine("[Publisher] Available options:\n");

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


    protected override Publisher InputEntity()
    {
        Console.Write("Enter name: ");
        string name = ConsoleUtilities.ReadStringRequired();

        Console.Write("Enter surname: ");
        string surname = ConsoleUtilities.ReadStringRequired();

        Console.Write("Enter boss ID: ");
        int bossId = ConsoleUtilities.ReadIntRequired();
        Author boss = new Author();

        List<Author> authors = new List<Author>();
        List<Book> books = new List<Book>();

        Publisher publisher = new Publisher(
            name,
            surname,
            boss,
            authors,
            books
        );

        return publisher;
    }

    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}

