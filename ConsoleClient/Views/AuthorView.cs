using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Helpers.TablePrinter;
using Core.Models;

namespace ConsoleClient.Views;

public class AuthorView : View<Author>
{
	private readonly AddressDAO _addressDAO;
	private readonly BookDAO _bookDAO;

	public AuthorView(AuthorDAO authorDAO, AddressDAO addressDAO, BookDAO bookDAO) : base(authorDAO) { _addressDAO = addressDAO; _bookDAO = bookDAO; }
	protected override void ShowOptions()
	{
		Console.WriteLine("[Author] Available options:\n");

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


	protected override Author InputEntity()
	{
		Console.Write("Enter name: ");
		string name = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter surname: ");
		string surname = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter date of birth (yyyy-MM-dd): ");
		DateOnly dateOfBirth = DateOnly.Parse(ConsoleUtilities.ReadStringRequired());

		Console.Write("Enter address ID: ");
		int addressId = ConsoleUtilities.ReadIntRequired();
		Address address = new Address();
		address = InputAddress();

		Console.Write("Enter phone number: ");
		string phoneNumber = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter email: ");
		string email = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter ID card number: ");
		string idCardNumber = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter years of experience: ");
		int yearsOfExperience = ConsoleUtilities.ReadIntRequired();

		List<Book> books = new List<Book>();


		Author author = new Author(
			name,
			surname,
			dateOfBirth,
			address,
			phoneNumber,
			email,
			idCardNumber,
			yearsOfExperience,
			books
		);

		return author;
	}

	private void ShowDetailsAction()
	{
		Console.WriteLine($"[{_dao.Identifier}] Showing details:\n");

		Console.Write("Enter Author ID: ");
		int id = ConsoleUtilities.ReadIntRequired();
		Author? author = _dao.GetById(id);

		Console.Write('\n');

		if (author == null)
		{
			Console.Clear();
			Console.WriteLine($"ERROR: Author with ID '{id}' doesn't exist.\n");
			return;
		}

		// Prikazuje osnovne podatke o autoru za kojeg prikazujemo detalje.
		new TablePrinter<Author>(_dao.EntityProperties, new List<Author>() { author }).Print();

		// Prikazuje knjige koje se je autor napisao:
		if (author.Books.Count > 0)
		{
			Console.WriteLine($"{author.FirstName} has written {author.Books.Count} book(s):\n");
			new TablePrinter<Book>(_bookDAO.EntityProperties, author.Books).Print();
		}
		else
			Console.WriteLine($"{author.FirstName} hasn't written any books.\n");

		Console.WriteLine($"{author.FirstName}'s address:\n");

		if (author.Address == null)
			Console.WriteLine("Address is unknown.\n");
		else
			new TablePrinter<Address>(_addressDAO.EntityProperties, new List<Address> { author.Address }).Print();

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

