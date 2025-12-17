using Core.Models;

namespace Core.Models;

public enum MemberStatus
{
    R, // Redovan
    V // VIP
}

public class Visitor
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public Address Address { get; set; }

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
        MemberStatus status
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Address = address;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        MembershipCardNumber = membershipCardNumber;
        MembershipYear = membershipYear;
        Status = status;
    }

    public string MembershipCardNumber { get; set; } // npr. CK-1-2023
    public int MembershipYear { get; set; }
    public MemberStatus Status { get; set; }

    public double AverageReviewRating { get; set; }

    // TODO:
    // 1. Dodati listu zelja
    // 2. Dodati listu kupljenih knjiga

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
            $"\n\tAverage review rating: {AverageReviewRating}";
    }
}
