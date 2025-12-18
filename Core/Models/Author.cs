using Core.Serialization;
using Core.Helpers.Relations;
using Core.Helpers;

namespace Core.Models;


public class Author : ISerializable, IIdentifiable
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string IdCardNumber { get; set; }
    public int YearsOfExperience { get; set; }
    public List<Book> Books { get; set; }

    public Author() { }
    public Author(int id, string firstname, string surname, DateOnly dateOfBirth, int addressId, Address address, string phoneNumber, string email, string idCardNumber, int yearsOfExperience, List<Book> books)
    {
        Id = id;
        FirstName = firstname;
        Surname = surname;
        DateOfBirth = dateOfBirth;
        AddressId = addressId;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        IdCardNumber = idCardNumber;
        YearsOfExperience = yearsOfExperience;
        Books = books;
    }

    public override string ToString()
    {
        return
            "[Author]:" +
            $"\n\tId: {Id}" +
            $"\n\tName: {FirstName}" +
            $"\n\tSurname: {Surname}" +
            $"\n\tDateOfBirth: {DateOfBirth}" +
            $"\n\tAddress Id: {AddressId}" +
            $"\n\tPhoneNumber: {PhoneNumber}" +
            $"\n\tEmail: {Email}" +
            $"\n\tIdCardNumber: {IdCardNumber}" +
            $"\n\tYearsOfExperience: {YearsOfExperience}" +
            $"\n\tBooks: {Books}";
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            FirstName,
            Surname,
            DateOfBirth.ToString(),
            AddressId.ToString(),
            PhoneNumber,
            Email,
            IdCardNumber,
            YearsOfExperience.ToString()
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        Surname = values[2];
        DateOfBirth = DateOnly.Parse(values[3]);
        AddressId = int.Parse(values[4]);
        PhoneNumber = values[5];
        Email = values[6];
        IdCardNumber = values[7];
        YearsOfExperience = int.Parse(values[8]);
    }
}
