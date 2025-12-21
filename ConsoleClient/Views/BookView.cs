using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Models;

namespace ConsoleClient.Views;

public class BookView : View<Book>
{
	public BookView(BookDAO bookDAO) : base(bookDAO) { }

	protected override string[] EntityProperties
	{
		get
		{
			return new string[] {
				"ID", "ISBN", "Book title", "Genre", "Release Year", "Price", "Number of pages", "Publisher ID"
			};
		}
	}

	protected override void ShowOptions()
	{
		Console.WriteLine("[Book] Available options:\n");

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


	protected override Book InputEntity()
	{
		Console.Write("Enter isbn: ");
		string isbn = SafeInput.ReadStringRequired();

		Console.Write("Enter book title: ");
		string bookTitle = SafeInput.ReadStringRequired();

		Console.Write("Enter genre: ");
		string genre = SafeInput.ReadStringRequired();

		Console.Write("Enter release year: ");
		int releaseYear = SafeInput.ReadIntRequired();

		Console.Write("Enter price: ");
		int price = SafeInput.ReadIntRequired();

		Console.Write("Enter number of pages: ");
		int numberOfPages = SafeInput.ReadIntRequired();

		Console.Write("Enter publisher ID: ");
		int publisherId = SafeInput.ReadIntRequired();
		Publisher publisher = new Publisher();

		List<Author> authors = new List<Author>();
		List<Visitor> buyers = new List<Visitor>();
		List<Visitor> wishers = new List<Visitor>();

		Book book = new Book(
			isbn, 
			bookTitle,
			genre,
			releaseYear, 
			price, 
			numberOfPages,
			authors,
			publisher,
			buyers,
			wishers
		);

		return book;
	}

	private const int SHOW_ALL = 1;
	private const int ADD = 2;
	private const int UPDATE = 3;
	private const int REMOVE = 4;
	private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}

