using ConsoleClient.Views.Utilities;
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


    protected override Visitor InputEntity()
    {
        Console.Write("Enter first name: ");
        string firstName = SafeInput.ReadStringRequired();

        Console.Write("Enter last name: ");
        string lastName = SafeInput.ReadStringRequired();

        Console.Write("Enter date of birth (yyyy-MM-dd): ");
        DateOnly dateOfBirth = DateOnly.Parse(SafeInput.ReadStringRequired());

        Console.Write("Enter phone number: ");
        string phoneNumber = SafeInput.ReadStringRequired();

        Console.Write("Enter email address: ");
        string emailAddress = SafeInput.ReadStringRequired();

        Console.Write("Enter address ID: ");
        int addressId = SafeInput.ReadIntRequired();

        Console.Write("Enter membership card number: ");
        string membershipCardNumber = SafeInput.ReadStringRequired();

        Console.Write("Enter membership year: ");
        int membershipYear = SafeInput.ReadIntRequired();

        Console.Write("Enter member status (R = Redovan, V = VIP): ");
        MemberStatus status = Enum.Parse<MemberStatus>(SafeInput.ReadStringRequired(), true);

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
