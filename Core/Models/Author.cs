using System.Net.NetworkInformation;
using Core.Helpers;
using Core.Helpers.Relations;
using Core.Helpers.Search;
using Core.Helpers.Sorting;
using Core.Serialization;

namespace Core.Models;


public class Author : ISerializable, IIdentifiable, IDeletable, ISearchable, ISortable
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get => $"{FirstName} {LastName}";}
    public DateTime? DateOfBirth { get; set; }
    public int AddressId { get; set; } = 0;
    public Address Address { get; set; } = new Address();
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdCardNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; } = 0;
    public List<Book> Books { get; set; } = new List<Book>();
	public bool IsDeleted { get; set; } = false;

	public bool IsMatch(SearchQuery query)
	{
		if (query.Empty) return true;

		switch (query.Length)
		{
			case 1:
				return query.IsMatch(new string[] { LastName });
			case 2:
				return query.IsMatch(new string[] { FirstName, LastName });
		}

		return false;
	}

	public Dictionary<string, dynamic> SortFields
	{
		get => new Dictionary<string, dynamic>
		{
			{ nameof(Id), Id },
			{ nameof(FirstName), FirstName },
			{ nameof(LastName), LastName },
            { nameof(Email), Email },
		};
	}

	public Author() { }
    public Author(string firstname, string lastName, DateTime dateOfBirth, Address address, string phoneNumber, string email, string idCardNumber, int yearsOfExperience, List<Book> books, bool isDeleted = false)
    {
        FirstName = firstname;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        AddressId = address.Id;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        IdCardNumber = idCardNumber;
        YearsOfExperience = yearsOfExperience;
        Books = books;
		IsDeleted = isDeleted;
	}

	public override string ToString()
    {
        return
            "[Author]:" +
            $"\n\tId: {Id}" +
            $"\n\tFirst name: {FirstName}" +
            $"\n\tLast name: {LastName}" +
            $"\n\tDateOfBirth: {DateOfBirth}" +
            $"\n\tAddress Id: {AddressId}" +
            $"\n\tPhoneNumber: {PhoneNumber}" +
            $"\n\tEmail: {Email}" +
            $"\n\tIdCardNumber: {IdCardNumber}" +
            $"\n\tYearsOfExperience: {YearsOfExperience}" +
            $"\n\tBooks: {Books}" +
			$"\n\tIsDeleted: {IsDeleted}";
	}

	public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            FirstName,
            LastName,
            DateOfBirth.ToString(),
            AddressId.ToString(),
            PhoneNumber,
            Email,
            IdCardNumber,
            YearsOfExperience.ToString(),
			IsDeleted.ToString()
		};
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        DateOfBirth = DateTime.Parse(values[3]);
        AddressId = int.Parse(values[4]);
        PhoneNumber = values[5];
        Email = values[6];
        IdCardNumber = values[7];
        YearsOfExperience = int.Parse(values[8]);
		IsDeleted = bool.Parse(values[9]);
	}
}
