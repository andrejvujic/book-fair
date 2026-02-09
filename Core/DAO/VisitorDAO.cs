
using Core.Helpers.Relations;
using Core.Models;
using System.Diagnostics;

namespace Core.DAO;

public class VisitorDAO : DAO<Visitor>
{
    private static string _fileName = "Visitors.txt";
    public override string Identifier { get { return "Visitor"; } }
    public override string[] EntityProperties
    {
        get
        {
            return new string[] {
                "ID", "First name", "Last name", "Date of birth",
                "Phone number", "Email address", "Address ID",
                "Membership card number", "Membership year", "Status", "AverageReviewRating"
            };
        }
    }

    private ManyToManyRelation<Visitor, Book> VisitorWishlistedBooks { get; }

    public VisitorDAO(
        ManyToManyRelation<Visitor, Book> visitorWishlistedBooks
    ) : base(_fileName) {
        VisitorWishlistedBooks = visitorWishlistedBooks;
    }

    public override void Add(Visitor visitor) {
        base.Add(visitor);
        VisitorWishlistedBooks.AddAll(visitor, visitor.WishlistedBooks);
    }

    public override Visitor? Remove(int id)
    {
        Visitor? visitor = base.Remove(id);

        if (visitor == null) return null;

        VisitorWishlistedBooks.RemoveAll(visitor);
        return visitor;
    }

    public override Visitor? Update(Visitor entity)
    {
        Visitor? oldEntity = GetById(entity.Id);
        if (oldEntity == null) return null;

        oldEntity.Id = entity.Id;
        oldEntity.FirstName = entity.FirstName;
        oldEntity.LastName = entity.LastName;
        oldEntity.AddressId = entity.AddressId;
        oldEntity.Address = entity.Address;
        oldEntity.EmailAddress = entity.EmailAddress;
        oldEntity.PhoneNumber = entity.PhoneNumber;
        oldEntity.DateOfBirth = entity.DateOfBirth;
        oldEntity.AverageReviewRating = entity.AverageReviewRating;
        oldEntity.MembershipCardNumber = entity.MembershipCardNumber;
        oldEntity.MembershipYear = entity.MembershipYear;
        oldEntity.Status = entity.Status;

        oldEntity.WishlistedBooks = new List<Book>(entity.WishlistedBooks);
        oldEntity.PurchasedBooks = new List<Book>(entity.PurchasedBooks);

        VisitorWishlistedBooks.RemoveAll(oldEntity);
        VisitorWishlistedBooks.AddAll(oldEntity, oldEntity.WishlistedBooks);

        _storage.Save(_entities);
        Subject.NotifyObservers();
        return oldEntity;   
    }

    protected override Dictionary<string, IOrderedEnumerable<Visitor>> SortCriteriaMapping {
        get
        {
            return new Dictionary<string, IOrderedEnumerable<Visitor>>
            {
                { "Id", _entities.OrderBy(x => x.Id) },
                { "FirstName", _entities.OrderBy(x => x.FirstName) },
                { "LastName", _entities.OrderBy(x => x.LastName) },
                { "DateOfBirth", _entities.OrderBy(x => x.DateOfBirth) },
                { "MembershipCardNumber", _entities.OrderBy(x => x.MembershipCardNumber) },
                { "MembershipYear", _entities.OrderBy(x => x.MembershipYear) },
                { "Status", _entities.OrderBy(x => x.Status) },
                { "AverageReviewRating", _entities.OrderBy(x => x.AverageReviewRating) }
            };
        }
    }

}

