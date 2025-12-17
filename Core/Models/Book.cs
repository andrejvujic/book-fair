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

    public Book(string isbn, string bookTitle, string genre, int releaseYear, int price, int numberOfPages, List<Author> authors, Publisher publisher, List<Visitor> buyers, List<Visitor> Wishlisters)
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
        Wishlisters = Wishlisters;
    }

    public override string ToString()
    {
        return
            $"[Book]:\n" +
            $"ISBN: {Isbn}, " +
            $"Title: {BookTitle}, " +
            $"Genre: {Genre}, " +
            $"ReleaseYear: {ReleaseYear}, " +
            $"Price: {Price}, " +
            $"Pages: {NumberOfPages}, " +
            $"Publisher: {Publisher}, " +
            $"Authors: {Authors}," +
            $"Buyers: {Buyers} " +
            $"Wishlisters: {Wishlisters} ";
    }
}