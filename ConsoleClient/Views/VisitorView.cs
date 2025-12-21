using Core.DAO;
using Core.Helpers.TablePrinter;
using Core.Models;

namespace ConsoleClient.Views;
public class VisitorView
{
    private readonly VisitorDAO _visitorDAO;
    public VisitorView(VisitorDAO visitorDAO)
    {
        _visitorDAO = visitorDAO;
    }

    public void Run() {
        while (true)
        {
            ShowMenu();

            int selectedOption = -1;

            Console.Write("> ");
            if (!int.TryParse(Console.ReadLine(), out selectedOption))
            {
                Console.Clear();
                Console.WriteLine("ERROR: Invalid input! Please select one of the available options.\n");
                continue;
            }

            Console.Clear();
            if (selectedOption == BACK)
                break;

            HandleAction(selectedOption);
        }
    }

    private void ShowMenu() {
        Console.WriteLine("[Visitor] Available options:\n");

        Console.WriteLine("1 -> Show all");
        Console.WriteLine("2 -> Add");
        Console.WriteLine("3 -> Update");
        Console.WriteLine("4 -> Remove");
        Console.WriteLine("5 -> Show (with pagination and sorting)");
        Console.WriteLine("0 -> Back\n");
    }

    private void HandleAction(int selectedOption)
    {
        switch(selectedOption)
        {
            case SHOW_ALL:
                ShowAllVisitors();
                break;
            case ADD:
                AddVisitor();
                break;
            case UPDATE:
                UpdateVisitor();
                break;
            case REMOVE:
                RemoveVisitor();
                break;
            case SHOW_WITH_PAGINATION_AND_SORTING:
                ShowPaginatedAndSorted();
                break;
            default:
                Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
                break;
        }
    }

    private void ShowAllVisitors()
    {
        Console.WriteLine("[Visitor] All visitors:\n");

        string[] visitorProperties = {
            "ID", "First name", "Last name", "Date of birth",
            "Phone number", "Email address", "Address ID",
            "Membership card number", "Membership year", "Status"
        };

        new TablePrinter<Visitor>(
            visitorProperties,
            _visitorDAO.GetAll()
        ).Print();
    }

    private void AddVisitor()
    {
        Console.WriteLine("[Visitor] Adding visitor:\n");

        Visitor visitor = InputVisitor();
        _visitorDAO.Add(visitor);

        Console.Clear();
        Console.WriteLine("SUCCESS: Action 'ADD' successful.\n");
    }

    private void UpdateVisitor()
    {
        Console.WriteLine("[Visitor] Updating visitor:\n");

        Console.Write("Enter ID: ");
        int id = -1;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Clear();
            Console.WriteLine("ERROR: Visitor ID invalid.\n");
            return;
        }

        Console.Write("\n");

        Visitor visitor = InputVisitor();
        visitor.Id = id;

        Console.Clear();

        if (_visitorDAO.Update(visitor) == null)
        {
            Console.WriteLine($"ERROR: Visitor with ID '{id}' doesn't exist.\n");
            return;
        }

        Console.WriteLine("SUCCESS: Action 'UPDATE' successful.\n");
    }

    private void RemoveVisitor()
    {
        Console.WriteLine("[Visitor] Removing visitor:\n");

        Console.Write("Enter ID: ");
        int id = -1;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Clear();
            Console.WriteLine("ERROR: Visitor ID invalid.\n");
            return;
        }

        Console.Clear();

        if (_visitorDAO.Remove(id) == null)
        {
            Console.WriteLine($"ERROR: Visitor with ID '{id}' doesn't exist.\n");
            return;
        }

        Console.WriteLine("SUCCESS: Action 'REMOVE' successful.\n");
    }

    private void ShowPaginatedAndSorted()
    {
        Console.WriteLine("[Visitor] Showing with pagination and sorting:\n");

        Console.Write("Enter page size: ");
        int pageSize = -1;
        if (!int.TryParse(Console.ReadLine(), out pageSize)) {
            Console.WriteLine($"ERROR: Invalid page size.\n");
            return;
        }

        if (pageSize < 1)
        {
            Console.WriteLine($"ERROR: Page size must be at least 1.\n");
            return;
        }

        int totalPageCount = (int)Math.Ceiling(_visitorDAO.Count() / (double)pageSize);

        Console.Write("Enter sorting criteria: ");
        string sortingCriteria = Console.ReadLine();

        Console.Write("Sorting direction:\n0 -> Ascending\n1 -> Descending\n");
        Console.Write("Enter sorting direction: ");

        int sortingDirection = -1;
        if (!int.TryParse(Console.ReadLine(), out sortingDirection))
        {
            Console.WriteLine($"ERROR: Invalid sorting direction.\n");
            return;
        }

        if (sortingDirection != 0 && sortingDirection != 1)
            Console.WriteLine("\nWARNING: Invalid sorting direction. Sorting direction will be set to ASCENDING.");

        Console.Write("\n");

        int pageNumber = 1;
        while (true)
        {
            List<Visitor> currentPageEntities = _visitorDAO.Get(
                pageNumber,
                pageSize,
                sortingCriteria,
                sortingDirection == 1 ? SortingDirection.Descending : SortingDirection.Ascending
            );

            Console.WriteLine($"Page: {pageNumber} / {totalPageCount}");
            Console.WriteLine($"Showing: {currentPageEntities.Count()} / {_visitorDAO.Count()} entities\n");

            new TablePrinter<Visitor>(
                new string[] {
                    "ID", "First name", "Last name", "Date of birth",
                    "Phone number", "Email address", "Address ID",
                    "Membership card number", "Membership year", "Status"
                },
                currentPageEntities
            ).Print();

            Console.Write("\n");

            Console.WriteLine("1 -> Previous page");
            Console.WriteLine("2 -> Next page");
            Console.WriteLine("0 -> Back");

            const int PREVIOUS_PAGE = 1;
            const int NEXT_PAGE = 2;

            int selectedOption = -1;

            Console.Write("> ");
            if (!int.TryParse(Console.ReadLine(), out selectedOption))
            {
                Console.Clear();
                Console.WriteLine("ERROR: Invalid input! Please select one of the available options.\n");
                continue;
            }

            Console.Clear();
            if (selectedOption == BACK)
                break;

            switch(selectedOption) {
                case PREVIOUS_PAGE:
                    pageNumber = pageNumber - 1 < 1 ? totalPageCount : pageNumber - 1;
                    break;
                case NEXT_PAGE:
                    pageNumber = pageNumber + 1 > totalPageCount ? 1 : pageNumber + 1;
                    break;
            }

        }
    }

    private Visitor InputVisitor()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();

        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();

        Console.Write("Enter date of birth (yyyy-MM-dd): ");
        DateOnly dateOfBirth = DateOnly.Parse(Console.ReadLine());

        Console.Write("Enter phone number: ");
        string phoneNumber = Console.ReadLine();

        Console.Write("Enter email address: ");
        string emailAddress = Console.ReadLine();

        Console.Write("Enter address ID: ");
        int addressId = int.Parse(Console.ReadLine());

        Console.Write("Enter membership card number: ");
        string membershipCardNumber = Console.ReadLine();

        Console.Write("Enter membership year: ");
        int membershipYear = int.Parse(Console.ReadLine());

        Console.Write("Enter member status (R = Redovan, V = VIP): ");
        MemberStatus status = Enum.Parse<MemberStatus>(Console.ReadLine(), true);

        Address address = new Address();
        List<Book> purchasedBooks = new List<Book>();
        List<Book> wishlistedBooks = new List<Book>();

        Visitor visitor = new Visitor(
            firstName,
            lastName,
            dateOfBirth,
            address,
            phoneNumber,
            emailAddress,
            membershipCardNumber,
            membershipYear,
            status,
            purchasedBooks,
            wishlistedBooks
        );

        return visitor;
    }


    private const int BACK = 0;
    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}
