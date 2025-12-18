using Core.Models;
using Core.Serialization;
using Core.Helpers;

namespace Core.Models;

public class Book : ISerializable, IIdentifiable
{
    public int Id { get; set; }
    public string Isbn { get; set; }
    public string BookTitle { get; set; }
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public int Price { get; set; }
    public int NumberOfPages { get; set; }
    public List<Author> Authors { get; set; }
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }
    public List<Visitor> Buyers { get; set; }
    public List<Visitor> Wishlisters { get; set; }

    public Book() { }

    public Book(int id,string isbn, string bookTitle, string genre, int releaseYear, int price, int numberOfPages, List<Author> authors, int publisherId,Publisher publisher, List<Visitor> buyers, List<Visitor> wishlisters)
    {
        Id = id;
        Isbn = isbn;
        BookTitle = bookTitle;
        Genre = genre;
        ReleaseYear = releaseYear;
        Price = price;
        NumberOfPages = numberOfPages;
        Authors = authors;
        PublisherId = publisherId;
        Publisher = publisher;
        Buyers = buyers;
        Wishlisters = wishlisters;
    }

    public override string ToString()
    {
        return
            $"[Book]:" +
            $"\n\tId: {Id}" +  
            $"\n\tISBN: {Isbn}" +
            $"\n\tTitle: {BookTitle}" +
            $"\n\tGenre: {Genre}" +
            $"\n\tReleaseYear: {ReleaseYear}" +
            $"\n\tPrice: {Price}" +
            $"\n\tPages: {NumberOfPages}" +
            $"\n\tPublisher ID: {PublisherId}" +
            $"\n\tAuthors: {Authors}" +
            $"\n\tBuyers: {Buyers}" +
            $"\n\tWishlisters: {Wishlisters}";
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id,
            Isbn,
            BookTitle,
            Genre,
            ReleaseYear.ToString(),
            Price.ToString(),
            NumberOfPages.ToString(),
            PublisherId.ToString(),

        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Isbn = values[1];
        BookTitle = values[2];
        Genre = values[3];
        ReleaseYear = int.Parse(values[4]);
        Price = int.Parse(values[5]);
        NumberOfPages = int.Parse(values[6]);
        PublisherId = int.Parse(values[7]);
    }
}