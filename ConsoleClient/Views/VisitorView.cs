using Core.DAO;
using Core.Helpers.TablePrinter;
using Core.Models;

namespace ConsoleClient.Views;
public class VisitorView : View<Visitor>
{
    public VisitorView(VisitorDAO visitorDAO) : base(visitorDAO) { }

    protected override string[] EntityProperties {
        get {
            return new string[] {
                "ID", "First name", "Last name", "Date of birth",
                "Phone number", "Email address", "Address ID",
                "Membership card number", "Membership year", "Status"
            };        
        }
    }

    protected override void ShowOptions() {
        Console.WriteLine("[Visitor] Available options:\n");

        Console.WriteLine("1 -> Show all");
        Console.WriteLine("2 -> Add");
        Console.WriteLine("3 -> Update");
        Console.WriteLine("4 -> Remove");
        Console.WriteLine("5 -> Show (with pagination and sorting)");
        Console.WriteLine("0 -> Back\n");
    }

    protected override void HandleAction(int selectedOption)
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

        new TablePrinter<Visitor>(
            EntityProperties,
            _dao.GetAll()
        ).Print();
    }

    private void AddVisitor()
    {
        Console.WriteLine("[Visitor] Adding visitor:\n");

        Visitor visitor = InputVisitor();
        _dao.Add(visitor);

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

        if (_dao.Update(visitor) == null)
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

        if (_dao.Remove(id) == null)
        {
            Console.WriteLine($"ERROR: Visitor with ID '{id}' doesn't exist.\n");
            return;
        }

        Console.WriteLine("SUCCESS: Action 'REMOVE' successful.\n");
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

    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}
