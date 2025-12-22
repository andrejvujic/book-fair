using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Helpers.TablePrinter;
using Core.Models;

namespace ConsoleClient.Views;

public class BookView : View<Book>
{
	private readonly AuthorDAO _authorDAO;
	private readonly PublisherDAO _publisherDAO;
	private readonly VisitorDAO _visitorDAO;
	public BookView(BookDAO bookDAO, AuthorDAO authorDAO, PublisherDAO publisherDAO, VisitorDAO visitorDAO) : base(bookDAO) {
		_authorDAO = authorDAO;
		_publisherDAO = publisherDAO;
		_visitorDAO = visitorDAO;
	}
	protected override void ShowOptions()
	{
		Console.WriteLine("[Book] Available options:\n");

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


	protected override Book InputEntity()
	{
		Console.Write("Enter ISBN: ");
		string isbn = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter book title: ");
		string bookTitle = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter genre: ");
		string genre = ConsoleUtilities.ReadStringRequired();

		Console.Write("Enter release year: ");
		int releaseYear = ConsoleUtilities.ReadIntRequired();

		Console.Write("Enter price: ");
		int price = ConsoleUtilities.ReadIntRequired();

		Console.Write("Enter number of pages: ");
		int numberOfPages = ConsoleUtilities.ReadIntRequired();

		Publisher publisher = new Publisher();
		publisher = InputPublisher();

		List<Author> authors = new List<Author>();
		authors = InputAuthors();
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

	private void ShowDetailsAction()
	{
		Console.WriteLine($"[{_dao.Identifier}] Showing details:\n");

		Console.Write("Enter Book ID: ");
		int id = ConsoleUtilities.ReadIntRequired();
		Book? book = _dao.GetById(id);

		Console.Write('\n');

		if (book == null)
		{
			Console.Clear();
			Console.WriteLine($"ERROR: Book with ID '{id}' doesn't exist.\n");
			return;
		}

		// Prikazuje osnovne podatke o knjigama za kojeg prikazujemo detalje.
		new TablePrinter<Book>(_dao.EntityProperties, new List<Book>() { book }).Print();

		// Prikazuje autore koji su napisali knjige:
		if (book.Authors.Count > 0)
		{
			Console.WriteLine($"'{book.BookTitle}' was written by {book.Authors.Count} author(s):\n");
			new TablePrinter<Author>(_authorDAO.EntityProperties, book.Authors).Print();
		}
		else
			Console.WriteLine($"{book.BookTitle} doesn't have any authors");

		Console.WriteLine($"'{book.BookTitle}' publisher:\n");

		if (book.Publisher == null)
			Console.WriteLine("Publisher is unknown.\n");
		else
			new TablePrinter<Publisher>(_publisherDAO.EntityProperties, new List<Publisher> { book.Publisher }).Print();

        if (book.Buyers.Count > 0)
        {
            Console.WriteLine($"'{book.BookTitle}' was purchased by {book.Buyers.Count} visitors(s):\n");
            new TablePrinter<Visitor>(_visitorDAO.EntityProperties, book.Buyers).Print();
        }
        else
            Console.WriteLine($"No visitors purchased '{book.BookTitle}'.");

        if (book.Wishlisters.Count > 0)
        {
            Console.WriteLine($"'{book.BookTitle}' was wishlisted by {book.Wishlisters.Count} visitors(s):\n");
            new TablePrinter<Visitor>(_visitorDAO.EntityProperties, book.Wishlisters).Print();
        }
        else
            Console.WriteLine($"No visitors wishlisted '{book.BookTitle}'.");

    }

    private List<Author> InputAuthors()
	{
		List<Author> authors;

		while (true)
		{
			Console.Write("Enter IDs of author`s: ");
			List<int> idsOfAuthors = ConsoleUtilities.ReadIntListRequired();

			if (ParseAuthorIds(idsOfAuthors, out authors)) break;
		}

		return authors;
	}

	private bool ParseAuthorIds(List<int> idsOfAuthors, out List<Author> authors)
	{
		authors = new List<Author>();

		foreach (int id in idsOfAuthors)
		{
			Author? author = _authorDAO.GetById(id);
			if (author == null)
			{
				authors.Clear();
				Console.WriteLine($"ERROR: Author with ID '{id}' doesn't exist.\n");
				return false;
			}

			authors.Add(author);
		}

		return true;
	}

	private Publisher InputPublisher()
	{
		Publisher publisher = new Publisher();

		while (true)
		{
			Console.Write("Enter publisher ID: ");
			int publisherId = ConsoleUtilities.ReadIntRequired();
			if (ParsePublisherId(publisherId, out publisher)) break;
		}

		return publisher;
	}

	private bool ParsePublisherId(int publisherId, out Publisher publisher)
	{
		publisher = new Publisher();

		Publisher? publisherCandidate = _publisherDAO.GetById(publisherId);
		if (publisherCandidate == null)
		{
			Console.WriteLine($"ERROR: Publisher with ID '{publisherId}' doesn't exist.\n");
			return false;
		}

		publisher = publisherCandidate;
		return true;
	}


	private const int SHOW_ALL = 1;
	private const int ADD = 2;
	private const int UPDATE = 3;
	private const int REMOVE = 4;
	private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
	private const int SHOW_DETAILS = 6;
}

