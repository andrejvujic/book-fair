using Core.Models;
using Core.Helpers.Relations;

namespace Core.DAO;

public class AuthorDAO: DAO<Author>
{
	private static string _filename = "Authors.txt";

	public override string Identifier { get { return "Author"; } }

	public override string[] EntityProperties
	{
		get
		{
			return new string[] {
				"ID", "First name", "Last name", "Date of birth", "Address ID", "Phone number", "Email", "ID card number", "Years of experience"
			};
		}
	}

	private ManyToManyRelation<Author, Book> BooksByAuthor { get; }

	public AuthorDAO(ManyToManyRelation<Author, Book> booksByAuthor) : base(_filename)
    {
        BooksByAuthor = booksByAuthor;
    }

    public override int Add(Author author)
    {
        int id = base.Add(author);
        BooksByAuthor.AddAll(author, author.Books);
        return id;
    }

    public override Author? Remove(int id)
    {
        Author? author = base.Remove(id);

        if(author == null) return null;

        BooksByAuthor.RemoveAll(author);
        return author;
    }

    public override Author? Update(Author entity)
    {
        Author? oldEntity = GetById(entity.Id);
        if(oldEntity == null) return null;

        oldEntity.Id = entity.Id;
        oldEntity.FirstName = entity.FirstName;
        oldEntity.LastName = entity.LastName;   
        oldEntity.DateOfBirth = entity.DateOfBirth;
        oldEntity.AddressId = entity.AddressId; 
        oldEntity.Address = entity.Address;
        oldEntity.PhoneNumber = entity.PhoneNumber;
        oldEntity.Email = entity.Email;
        oldEntity.IdCardNumber = entity.IdCardNumber;   
        oldEntity.YearsOfExperience = entity.YearsOfExperience; 
        oldEntity.Books = entity.Books;

		oldEntity.IsDeleted = entity.IsDeleted;

		BooksByAuthor.RemoveAll(oldEntity);
        BooksByAuthor.AddAll(oldEntity, oldEntity.Books);

        _storage.Save(_entities);
        Subject.NotifyObservers();
        return oldEntity;
    }

    protected override Dictionary<string, IOrderedEnumerable<Author>> SortCriteriaMapping
    {
        get
        {
            return new Dictionary<string, IOrderedEnumerable<Author>>
            {
                { "Id", _entities.OrderBy(x=>x.Id) },
			    { "FirstName", _entities.OrderBy(x=>x.FirstName) },
				{ "LastName", _entities.OrderBy(x=>x.LastName) },
				{ "DateOfBirth", _entities.OrderBy(x=>x.DateOfBirth) },
                { "IdCardNumber", _entities.OrderBy(x=>x.IdCardNumber) },
				{ "YearsOfExperience", _entities.OrderBy(x=>x.YearsOfExperience) },
			};
        }
    }
}

