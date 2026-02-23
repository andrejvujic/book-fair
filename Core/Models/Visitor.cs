using Core.Serialization;
using Core.Helpers;
using Core.Helpers.Search;
using Core.Helpers.Sorting;

namespace Core.Models;

public enum MemberStatus
{
    R, // Redovan
    V // VIP
}

public class Visitor : ISerializable, IIdentifiable, IDeletable, ISearchable, ISortable
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; } // Podrazumijevana vrijednost za kada se prikazuje u UI-u.
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;

    public int AddressId { get; set; } = 0;
    public Address Address { get; set; } = new Address();

    public string MembershipCardNumber { get; set; } = string.Empty; // npr. CK-1-2023
    public int MembershipYear { get; set; } = 0;
    public MemberStatus Status { get; set; }

    public double AverageReviewRating { get; set; } = 0;
    public List<Book> PurchasedBooks { get; set; } = new List<Book>();
    public List<Book> WishlistedBooks { get; set; } = new List<Book>();
    public bool IsDeleted { get; set; } = false;

    public bool IsMatch(SearchQuery query)
    {
        if (query.Empty) return true;

        switch (query.Length)
        {
            case 1:
                return query.IsMatch(new string[] { LastName });
            case 2:
                return query.IsMatch(new string[] { LastName, FirstName });
            case 3:
                return query.IsMatch(new string[] { MembershipCardNumber, FirstName, LastName });
        }

        return false;
    }
    public Dictionary<string, dynamic> SortFields {
        get => new Dictionary<string, dynamic>
        {
            { nameof(Id), Id },
            { nameof(FirstName), FirstName },
            { nameof(LastName), LastName },
            { nameof(MembershipCardNumber), MembershipCardNumber  },
            { nameof(MembershipYear), MembershipYear },
            { nameof(Status), Status },
            { nameof(AverageReviewRating), AverageReviewRating },
        };
    }

    public Visitor() {}

    public Visitor(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        Address address,
        string phoneNumber,
        string emailAddress,
        string membershipCardNumber,
        int membershipYear,
        MemberStatus status,
        List<Book> purchasedBooks,
        List<Book> wishlistedBooks,
        bool isDeleted = false
    )
    {
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
        IsDeleted = isDeleted;
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
            $"\n\tWishlisted books: ${WishlistedBooks}" +
            $"\n\tDeleted: ${IsDeleted}";
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            FirstName,
            LastName,
            DateOfBirth?.ToString() ?? DateTime.Now.ToString(),
            PhoneNumber,
            EmailAddress,
            AddressId.ToString(),
            MembershipCardNumber,
            MembershipYear.ToString(),
            Status.ToString(),
            AverageReviewRating.ToString(),
            IsDeleted.ToString(),
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        DateOfBirth = DateTime.Parse(values[3]);
        PhoneNumber = values[4];
        EmailAddress = values[5];
        AddressId = int.Parse(values[6]);
        MembershipCardNumber = values[7];
        MembershipYear = int.Parse(values[8]);
        Status = Enum.Parse<MemberStatus>(values[9]);
        AverageReviewRating = double.Parse(values[10]);
        IsDeleted = bool.Parse(values[11]);
    }
}
