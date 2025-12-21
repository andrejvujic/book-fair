using Core.Models;
using Core.Helpers.Relations;
using System.Runtime.CompilerServices;

namespace Core.DAO;

public class BookDAO: DAO<Book>
{
	private static string _filename = "Books.txt";
    public override string Identifier { get { return "Book"; } }

	private ManyToManyRelation<Book, Author> AuthorsByBooks { get; }
	private ManyToManyRelation<Book, Visitor> Buyers { get; }
	private ManyToManyRelation<Book, Visitor> Wishers { get; }

	public BookDAO(
		ManyToManyRelation<Book, Author> authorsbybooks,
		ManyToManyRelation<Book, Visitor> buyers,
		ManyToManyRelation<Book, Visitor> wishers
		) : base(_filename)
	{
		AuthorsByBooks = authorsbybooks;
		Buyers = buyers;
		Wishers = wishers;
	}

    public override void Add(Book book)
    {
		base.Add(book);
		AuthorsByBooks.AddAll(book, book.Authors);
		Buyers.AddAll(book, book.Buyers);
		Wishers.AddAll(book, book.Wishlisters);
    }

    public override Book? Remove(int id)
    {
		Book? book = base.Remove(id);

		if (book == null) return null;

		AuthorsByBooks.RemoveAll(book);
		Buyers.RemoveAll(book);
		Wishers.RemoveAll(book);
		return book;
    }

    public override Book? Update(Book entity)
    {
		Book? oldEntity = GetById(entity.Id);
		if (oldEntity == null) return null;

		oldEntity.Id = entity.Id;
		oldEntity.Isbn = entity.Isbn;
		oldEntity.BookTitle = entity.BookTitle;
		oldEntity.Genre = entity.Genre;	
		oldEntity.ReleaseYear = entity.ReleaseYear;
		oldEntity.Price = entity.Price;
		oldEntity.NumberOfPages = entity.NumberOfPages;
		oldEntity.Authors = entity.Authors;
		oldEntity.PublisherId = entity.PublisherId;
		oldEntity.Publisher = entity.Publisher;
		oldEntity.Buyers = entity.Buyers;
		oldEntity.Wishlisters = entity.Wishlisters;

		AuthorsByBooks.RemoveAll(oldEntity);
		Buyers.RemoveAll(oldEntity);
		Wishers.RemoveAll(oldEntity);

		AuthorsByBooks.AddAll(oldEntity, oldEntity.Authors);
		Buyers.AddAll(oldEntity, oldEntity.Buyers);
		Wishers.AddAll(oldEntity, oldEntity.Wishlisters);

		_storage.Save(_entities);
		return oldEntity;
    }

	protected override Dictionary<string, IOrderedEnumerable<Book>> SortCriteriaMapping
	{
		get
		{
			return new Dictionary<string, IOrderedEnumerable<Book>>
			{
				{ "Id", _entities.OrderBy(x => x.Id) },
				{ "Isbn", _entities.OrderBy(x => x.Isbn) },
				{ "BookTitle", _entities.OrderBy(x => x.BookTitle) },
				{ "Genre", _entities.OrderBy(x => x.Genre) },
				{ "ReleaseYear", _entities.OrderBy(x => x.ReleaseYear) },
				{ "Price", _entities.OrderBy(x => x.Price) },
				{ "NumberOfPages", _entities.OrderBy(x => x.NumberOfPages) },
			};
		}
	}
}