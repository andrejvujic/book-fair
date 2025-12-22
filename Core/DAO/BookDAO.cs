using Core.Models;
using Core.Helpers.Relations;
using System.Runtime.CompilerServices;

namespace Core.DAO;

public class BookDAO: DAO<Book>
{
	private static string _filename = "Books.txt";
    public override string Identifier { get { return "Book"; } }

	public override string[] EntityProperties
	{
		get
		{
			return new string[] {
				"ID", "ISBN", "Book title", "Genre", "Release Year", "Price", "Number of pages", "Publisher ID"
			};
		}
	}

	private ManyToManyRelation<Author, Book> BookAuthors { get; }
	private ManyToManyRelation<Visitor, Book> BookWishlisters { get; }

	public BookDAO(
		ManyToManyRelation<Author, Book> bookAuthors,
		ManyToManyRelation<Visitor, Book> bookWishlisters
		) : base(_filename)
	{
		BookAuthors = bookAuthors;
        BookWishlisters = bookWishlisters;
	}

    public override void Add(Book book)
    {
		base.Add(book);
		BookAuthors.AddAll(book, book.Authors);
        BookWishlisters.AddAll(book, book.Wishlisters);
    }

    public override Book? Remove(int id)
    {
		Book? book = base.Remove(id);

		if (book == null) return null;

		BookAuthors.RemoveAll(book);
        BookWishlisters.RemoveAll(book);
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

		BookAuthors.RemoveAll(oldEntity);
        BookWishlisters.RemoveAll(oldEntity);

		BookAuthors.AddAll(oldEntity, oldEntity.Authors);
        BookWishlisters.AddAll(oldEntity, oldEntity.Wishlisters);

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