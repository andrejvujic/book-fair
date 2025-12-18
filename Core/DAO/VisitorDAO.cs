
using Core.Helpers.Relations;
using Core.Models;

namespace Core.DAO;

public class VisitorDAO : DAO<Visitor>
{

    private static string _fileName = "Visitors.txt";

    private ManyToManyRelation<Visitor, Book> VisitorPurchasedBooks { get; }
    private ManyToManyRelation<Visitor, Book> VisitorWishlistedBooks { get; }

    public VisitorDAO(
        ManyToManyRelation<Visitor, Book> visitorPurchasedBooks,
        ManyToManyRelation<Visitor, Book> visitorWishlistedBooks
    ) : base(_fileName) {
        VisitorPurchasedBooks = visitorPurchasedBooks;
        VisitorWishlistedBooks = visitorWishlistedBooks;
    }

    public override void Add(Visitor visitor) {
        base.Add(visitor);

        VisitorPurchasedBooks.AddAll(visitor, visitor.PurchasedBooks);
        VisitorWishlistedBooks.AddAll(visitor, visitor.WishlistedBooks);
    }

    public override Visitor? Remove(int id)
    {
        Visitor? visitor =  base.Remove(id);

        if (visitor == null) return null;

        VisitorPurchasedBooks.RemoveAll(visitor);
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
        oldEntity.WishlistedBooks = entity.WishlistedBooks;
        oldEntity.PurchasedBooks = entity.PurchasedBooks;

        VisitorPurchasedBooks.RemoveAll(oldEntity);
        VisitorWishlistedBooks.RemoveAll(oldEntity);

        VisitorPurchasedBooks.AddAll(oldEntity, oldEntity.PurchasedBooks);
        VisitorWishlistedBooks.AddAll(oldEntity, oldEntity.WishlistedBooks);

        _storage.Save(_entities);
        return oldEntity;
    }
}

