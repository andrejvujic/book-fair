using Core.Models;
namespace Core.Models;


public class Author
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public Date DateOfBirth { get; set; }
    public Address Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string IdCardNumber { get; set; }
    public int YearsOfExperience { get; set; }

    //missing list of book

    public Author() { }
    public Author(int id, string name, string surname, string dateofbirth, Address address, string phonenumber, string email, string idcardnumber, string yearsofexperience)
    {
        Id = id;
        Name = name;
        Surname = surname;
        DateOfBirth = dateofbirth;
        Address = address;
        PhoneNumber = phonenumber;
        Email = email;
        IdCardNumber = idcardnumber;
        YearsOfExperience = yearsofexperience;
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
            $"\n\tYearsOfExperience: {YearsOfExperience}";
    }

}
