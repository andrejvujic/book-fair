using Core.Models;
namespace Core.Models;


public class Author
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Address Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string IdCardNumber { get; set; }
    public int YearsOfExperience { get; set; }
    public List<Book> Books { get; set; }

    public Author() { }
    public Author(int id, string name, string surname, DateOnly dateOfBirth, Address address, string phoneNumber, string email, string idCardNumber, int yearsOfExperience, List<Book> books)
    {
        Id = id;
        Name = name;
        Surname = surname;
        DateOfBirth = dateOfBirth;
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
            $"\n\tName: {Name}" +
            $"\n\tSurname: {Surname}" +
            $"\n\tDateOfBirth: {DateOfBirth}" +
            $"\n\tAddress: {Address}" +
            $"\n\tPhoneNumber: {PhoneNumber}" +
            $"\n\tEmail: {Email}" +
            $"\n\tIdCardNumber: {IdCardNumber}" +
            $"\n\tYearsOfExperience: {YearsOfExperience}" +
            $"\n\tBooks: {Books}";
    }
}
