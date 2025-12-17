using Core.Models;
namespace Core.Models;

public class Book
{
    public string Isbn { get; set; }
    public string BookTitle { get; set; }
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public int Price { get; set; }
    public int NumberOfPages { get; set; }
    public List<Author> Authors { get; set; }
    public Publisher Publisher { get; set; }
    public List<Visitor> Buyers { get; set; }
    public List<Visitor> Wishlisters { get; set; }

    public Book() { }

    public Book(string isbn, string bookTitle, string genre, int releaseYear, int price, int numberOfPages, List<Author> authors, Publisher publisher, List<Visitor> buyers, List<Visitor> wishlisters)
    {
        Isbn = isbn;
        BookTitle = bookTitle;
        Genre = genre;
        ReleaseYear = releaseYear;
        Price = price;
        NumberOfPages = numberOfPages;
        Authors = authors;
        Publisher = publisher;
        Buyers = buyers;
        Wishlisters = wishlisters;
    }

    public override string ToString()
    {
        return
            $"[Book]:" +
            $"\n\tISBN: {Isbn}" +
            $"\n\tTitle: {BookTitle}" +
            $"\n\tGenre: {Genre}" +
            $"\n\tReleaseYear: {ReleaseYear}" +
            $"\n\tPrice: {Price}" +
            $"\n\tPages: {NumberOfPages}" +
            $"\n\tPublisher: {Publisher}" +
            $"\n\tAuthors: {Authors}" +
            $"\n\tBuyers: {Buyers}" +
            $"\n\tWishlisters: {Wishlisters}";
    }
}