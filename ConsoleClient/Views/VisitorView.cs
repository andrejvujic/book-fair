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
                break;
            case SHOW_WITH_PAGINATION_AND_SORTING:
                 break;
            default:
                Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
                break;
        }
    }

    private void ShowAllVisitors()
    {
        Console.WriteLine("[Visitor] All visitors:\n");

        string[] tableHeadRow = {
            "ID", "First name", "Last name", "Date of birth",
            "Phone number", "Email address", "Address ID",
            "Membership card number", "Membership year","Status"
        };

        new TablePrinter<Visitor>(
            tableHeadRow,
            _visitorDAO.GetAll()
        ).Print();
    }

    private void AddVisitor()
    {
        Console.WriteLine("[Visitor] Adding visitor:\n");

        Visitor visitor = InputVisitor();
        _visitorDAO.Add(visitor);

        Console.Clear();
        Console.WriteLine("SUCCESS: Visitor ADDED.\n");
    }

    private void UpdateVisitor()
    {
        Console.WriteLine("[Visitor] Updating visitor:\n");
        Console.WriteLine("Note: Leave blank field blank to keep previous value.");

        Console.Write("Enter ID: ");
        int id = -1;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Clear();
            Console.WriteLine("ERROR: Visitor ID invalid.");
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

        Console.WriteLine("SUCCESS: Visitor UPDATED.\n");
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
