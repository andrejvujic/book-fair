using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views;

public class AuthorView : View<Author>
{
	public AuthorView(AuthorDAO authorDAO) : base(authorDAO) { }

	protected override string[] EntityProperties
	{
		get
		{
			return new string[] {
				"ID", "First name", "Last name", "Date of birth", "Address ID", "Phone number", "Email", "ID card number", "Years of experience"
			};
		}
	}

	protected override void ShowOptions()
	{
		Console.WriteLine("[Author] Available options:\n");

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


	protected override Author InputEntity()
	{
		Console.Write("Enter name: ");
		string name = SafeInput.ReadStringRequired();

		Console.Write("Enter surname: ");
		string surname = SafeInput.ReadStringRequired();

		Console.Write("Enter date of birth (yyyy-MM-dd): ");
		DateOnly dateOfBirth = DateOnly.Parse(SafeInput.ReadStringRequired());

		Console.Write("Enter address ID: ");
		int addressId = SafeInput.ReadIntRequired();
		Address address = new Address();

		Console.Write("Enter phone number: ");
		string phoneNumber = SafeInput.ReadStringRequired();

		Console.Write("Enter email: ");
		string email = SafeInput.ReadStringRequired();

		Console.Write("Enter ID card number: ");
		string idCardNumber = SafeInput.ReadStringRequired();

		Console.Write("Enter years of experience: ");
		int yearsOfExperience = SafeInput.ReadIntRequired();

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

	private const int SHOW_ALL = 1;
	private const int ADD = 2;
	private const int UPDATE = 3;
	private const int REMOVE = 4;
	private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}

