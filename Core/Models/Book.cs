using Core.Models;
using Core.Serialization;
using Core.Helpers;

namespace Core.Models;

public enum BookGenre { FANTASY, SCIENCE_FICTION, MYSTERY, THRILLER, ROMANCE, HORROR, COMEDY, NOVEL, POETRY, DRAMA}

public class Book : ISerializable, IIdentifiable, IDeletable
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public BookGenre Genre { get; set; }
    public int ReleaseYear { get; set; } = 0;
    public double Price { get; set; } = 0;
    public int NumberOfPages { get; set; } = 0;
    public List<Author> Authors { get; set; } = new List<Author>();
    public int PublisherId { get; set; } = -1;
    public Publisher? Publisher { get; set; }
    public List<Visitor> Buyers { get; set; } = new List<Visitor>();
    public List<Visitor> Wishlisters { get; set; } = new List<Visitor>();
    public bool IsDeleted { get; set; } = false;

	public Book() { }

    public Book(string isbn, string title, BookGenre genre, int releaseYear, double price, int numberOfPages, List<Author> authors, Publisher publisher, List<Visitor> buyers, List<Visitor> wishlisters, bool isDeleted = false)
    {
        Isbn = isbn;
        Title = title;
        Genre = genre;
        ReleaseYear = releaseYear;
        Price = price;
        NumberOfPages = numberOfPages;
        Authors = authors;
        PublisherId = publisher.Id;
        Publisher = publisher;
        Buyers = buyers;
        Wishlisters = wishlisters;
        IsDeleted = isDeleted;
	}

    public override string ToString()
    {
        return
            $"[Book]:" +
            $"\n\tId: {Id}" +  
            $"\n\tISBN: {Isbn}" +
            $"\n\tTitle: {Title}" +
            $"\n\tGenre: {Genre}" +
            $"\n\tReleaseYear: {ReleaseYear}" +
            $"\n\tPrice: {Price}" +
            $"\n\tPages: {NumberOfPages}" +
            $"\n\tPublisher ID: {PublisherId}" +
            $"\n\tAuthors: {Authors}" +
            $"\n\tBuyers: {Buyers}" +
            $"\n\tWishlisters: {Wishlisters}" + 
            $"\n\tIsDeleted: {IsDeleted}";
	}

    public List<string> FirstAndLastNamesOfAllAuthors
    {
        get
        {
            List<string> names = new List<string>();
            foreach (Author author in Authors)
                names.Add($"{author.FirstName} {author.LastName}");
            return names;
        }
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            Isbn,
            Title,
            Genre.ToString(),
            ReleaseYear.ToString(),
            Price.ToString(),
            NumberOfPages.ToString(),
            PublisherId.ToString(),
            IsDeleted.ToString()
		};
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Isbn = values[1];
        Title = values[2];
        Genre = Enum.Parse<BookGenre>(values[3]);
		ReleaseYear = int.Parse(values[4]);
        Price = double.Parse(values[5]);
        NumberOfPages = int.Parse(values[6]);
        PublisherId = int.Parse(values[7]);
        IsDeleted = bool.Parse(values[8]);
	}
}