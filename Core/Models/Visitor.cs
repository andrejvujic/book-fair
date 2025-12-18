using Core.Serialization;
using Core.Helpers;

namespace Core.Models;

public enum MemberStatus
{
    R, // Redovan
    V // VIP
}

public class Visitor : ISerializable, IIdentifiable
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }

    public string MembershipCardNumber { get; set; } // npr. CK-1-2023
    public int MembershipYear { get; set; }
    public MemberStatus Status { get; set; }

    public double AverageReviewRating { get; set; }
    public List<Book> PurchasedBooks { get; set; }
    public List<Book> WishlistedBooks { get; set; }

    public Visitor() { }

    public Visitor(
        int id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        Address address,
        string phoneNumber,
        string emailAddress,
        string membershipCardNumber,
        int membershipYear,
        MemberStatus status,
        List<Book> purchasedBooks,
        List<Book> wishlistedBooks
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Address = address;
        AddressId = address.Id;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        MembershipCardNumber = membershipCardNumber;
        MembershipYear = membershipYear;
        Status = status;
        PurchasedBooks = purchasedBooks;
        WishlistedBooks = wishlistedBooks;
    }

    public override string ToString()
    {
        return
            "[Visitor]:" +
            $"\n\tId: {Id}" +
            $"\n\tFirst name: {FirstName}" +
            $"\n\tLast name: {LastName}" +
            $"\n\tDate of birth: {DateOfBirth}" +
            $"\n\tPhone number: {PhoneNumber}" +
            $"\n\tEmail address: {EmailAddress}" +
            $"\n\tAddress: {Address}" +
            $"\n\tMembership card number: {MembershipCardNumber}" +
            $"\n\tMembership year: {MembershipYear}" +
            $"\n\tStatus: {Status}" +
            $"\n\tAverage review rating: {AverageReviewRating}" +
            $"\n\tPurchased books: ${PurchasedBooks}" +
            $"\n\tWishlisted books: ${WishlistedBooks}";
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            FirstName,
            LastName,
            DateOfBirth.ToString(),
            PhoneNumber,
            EmailAddress,
            AddressId.ToString(),
            MembershipCardNumber,
            MembershipYear.ToString(),
            Status.ToString(),
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        DateOfBirth = DateOnly.Parse(values[3]);
        PhoneNumber = values[4];
        EmailAddress = values[5];
        AddressId = int.Parse(values[6]);
        MembershipCardNumber = values[6];
        MembershipYear = int.Parse(values[7]);
        Status = Enum.Parse<MemberStatus>(values[8]);
    }
}
