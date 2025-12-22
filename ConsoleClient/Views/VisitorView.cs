using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Helpers.Relations;
using Core.Helpers.TablePrinter;
using Core.Models;

namespace ConsoleClient.Views;
public class VisitorView : View<Visitor>
{
    private readonly BookDAO _bookDAO;
    private readonly AddressDAO _addressDAO;

    public VisitorView(VisitorDAO visitorDAO, BookDAO bookDAO, AddressDAO addressDAO) : base(visitorDAO) {
        _bookDAO = bookDAO;
        _addressDAO = addressDAO;
    }

    protected override void ShowOptions() {
        Console.WriteLine("[Visitor] Available options:\n");

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
            case SHOW_DETAILS:
                ShowDetailsAction();
                break;
            default:
                Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
                break;
        }
    }


    protected override Visitor InputEntity()
    {
        Console.Write("Enter first name: ");
        string firstName = ConsoleUtilities.ReadStringRequired();

        Console.Write("Enter last name: ");
        string lastName = ConsoleUtilities.ReadStringRequired();

        Console.Write("Enter date of birth (yyyy-MM-dd): ");
        DateOnly dateOfBirth = DateOnly.Parse(ConsoleUtilities.ReadStringRequired());

        Console.Write("Enter phone number: ");
        string phoneNumber = ConsoleUtilities.ReadStringRequired();

        Console.Write("Enter email address: ");
        string emailAddress = ConsoleUtilities.ReadStringRequired();

        Address address = InputAddress();

        Console.Write("Enter membership card number: ");
        string membershipCardNumber = ConsoleUtilities.ReadStringRequired();

        Console.Write("Enter membership year: ");
        int membershipYear = ConsoleUtilities.ReadIntRequired();

        Console.Write("Enter member status (R = Redovan, V = VIP): ");
        MemberStatus status = Enum.Parse<MemberStatus>(ConsoleUtilities.ReadStringRequired(), true);

        List<Book> purchasedBooks = new List<Book>();
        List<Book> wishlistedBooks = InputWishlistedBooks();

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

    private void ShowDetailsAction() {
        Console.WriteLine($"[{_dao.Identifier}] Showing details:\n");

        Console.Write("Enter Visitor ID: ");
        int id = ConsoleUtilities.ReadIntRequired();
        Visitor? visitor = _dao.GetById(id);

        Console.Write('\n');

        if (visitor == null)
        {
            Console.Clear();
            Console.WriteLine($"ERROR: Visitor with ID '{id}' doesn't exist.\n");
            return;
        }

        // Prikazuje osnovne podatke o posjetiocu za kojeg prikazujemo detalje.
        new TablePrinter<Visitor>(_dao.EntityProperties, new List<Visitor>() { visitor }).Print();

        // Prikazuje knjige koje se nalaze na listi zelja:
        if (visitor.WishlistedBooks.Count > 0)
        {
            Console.WriteLine($"{visitor.FirstName} has wishlisted {visitor.WishlistedBooks.Count} book(s):\n");
            new TablePrinter<Book>(_bookDAO.EntityProperties, visitor.WishlistedBooks).Print();
        }
        else
            Console.WriteLine($"{visitor.FirstName} hasn't purchased any books.\n");

        // Prikazuje knjige koje se nalaze na listi zelja:
        if (visitor.PurchasedBooks.Count > 0)
        {
            Console.WriteLine($"{visitor.FirstName} has purchased {visitor.PurchasedBooks.Count} book(s):\n");
            new TablePrinter<Book>(_bookDAO.EntityProperties, visitor.PurchasedBooks).Print();
        } else
            Console.WriteLine($"{visitor.FirstName} hasn't purchased any books.\n");
    }

    private List<Book> InputWishlistedBooks()
    {
        List<Book> wishlistedBooks;

        while (true)
        {
            Console.Write("Enter IDs of wishlisted books: ");
            List<int> idsOfWishlistedBooks = ConsoleUtilities.ReadIntListRequired();

            if (ParseWishlistedBookIds(idsOfWishlistedBooks, out wishlistedBooks)) break;
        }

        return wishlistedBooks;
    }

    private bool ParseWishlistedBookIds(List<int> idsOfWishlistedBooks, out List<Book> wishlistedBooks)
    {
        wishlistedBooks = new List<Book>();

        foreach (int id in idsOfWishlistedBooks)
        {
            Book? wishlistedBook = _bookDAO.GetById(id);
            if (wishlistedBook == null)
            {
                wishlistedBooks.Clear();
                Console.WriteLine($"ERROR: Book with ID '{id}' doesn't exist.\n");
                return false;
            }

            wishlistedBooks.Add(wishlistedBook);
        }

        return true;
    }

    private Address InputAddress()
    {
        Address address = new Address();

        while (true)
        {
            Console.Write("Enter address ID: ");
            int addressId = ConsoleUtilities.ReadIntRequired();
            if (ParseAddressId(addressId, out address)) break;
        }

        return address;
    }

    private bool ParseAddressId(int addressId, out Address address)
    {
        address = new Address();

        Address? addressCandidate = _addressDAO.GetById(addressId);
        if (addressCandidate == null)
        {
            Console.WriteLine($"ERROR: Address with ID '{addressId}' doesn't exist.\n");
            return false;
        }

        address = addressCandidate;
        return true;
    }

    private const int SHOW_ALL = 1;
    private const int ADD = 2;
    private const int UPDATE = 3;
    private const int REMOVE = 4;
    private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
    private const int SHOW_DETAILS = 6;
}
