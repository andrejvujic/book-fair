
using Core.Models;

namespace Core.DAO;

public class VisitorDAO : DAO<Visitor>
{
    private static string _fileName = "Visitors.txt";
    public VisitorDAO() : base(_fileName) {}

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

        _storage.Save(_entities);
        return oldEntity;
    }
}

