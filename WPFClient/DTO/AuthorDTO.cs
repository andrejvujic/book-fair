using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace WPFClient.DTO
{
	public class AuthorDTO : DTO<Author>, IValidatable
	{
		public AuthorDTO() : base(new Author()) { }

		public AuthorDTO(Author author) : base(author) { }

		public override DTO<Author> Copy()
		{
			return new AuthorDTO
			{
				Id = Id,
				FirstName = FirstName,
				LastName = LastName,
				DateOfBirth = DateOfBirth,
				AddressId = AddressId,
				AddressCity = AddressCity,
				AddressStreet = AddressStreet,
				AddressCountry = AddressCountry,
				AddressStreetNumber = AddressStreetNumber,
				YearsOfExperience = YearsOfExperience,
				Email = Email,
				PhoneNumber = PhoneNumber,
				IdCardNumber = IdCardNumber,
				Books = new List<Book>(Books),

			};
		}

		protected override void FromModel(Author author)
		{
			Id = author.Id;
			FirstName = author.FirstName;
			LastName = author.LastName;
			DateOfBirth = author.DateOfBirth;
			AddressId = author.AddressId;
			addressCity = author.Address?.City ?? string.Empty;
			addressCountry = author.Address?.Country ?? string.Empty;
			addressStreetNumber = author.Address?.StreetNumber ?? string.Empty;
			AddressStreet = author.Address?.Street ?? string.Empty;
			YearsOfExperience = author.YearsOfExperience > 0 ? author.YearsOfExperience.ToString() : string.Empty;
			Email = author.Email;
			PhoneNumber = author.PhoneNumber;
			IdCardNumber = author.IdCardNumber;
			Books = author.Books;
		}

		public override Author ToModel()
		{
			return new Author
			{
				Id = Id,
				FirstName = FirstName,
				LastName = LastName,
				DateOfBirth	= DateOfBirth,
				AddressId = AddressId,
				Address = Address,
				YearsOfExperience = int.Parse(YearsOfExperience),
				Email = Email,
				PhoneNumber = PhoneNumber,
				Books = Books,
				IdCardNumber = IdCardNumber,
			};
		}

		public int Id { get; set; }


		private string firstName = string.Empty;
		public string FirstName
		{
			get => firstName;
			set
			{
				if (firstName == value) return;
				firstName = value;
				OnPropertyChanged(nameof(FirstName));
				OnPropertyChanged(nameof(AllFieldsValid));

			}
		}

		private string lastName = string.Empty;
		public string LastName
		{
			get => lastName;
			set
			{
				if (lastName == value) return;
				lastName = value;
				OnPropertyChanged(nameof(LastName));
				OnPropertyChanged(nameof(AllFieldsValid));

			}
		}

		private DateOnly? dateOfBirth;
		public DateOnly? DateOfBirth
		{
			get => dateOfBirth;
			set
			{
				if (dateOfBirth == value) return;
				dateOfBirth = value;
				OnPropertyChanged(nameof(DateOfBirth));
				OnPropertyChanged(nameof(AllFieldsValid));

			}
		}

		public int AddressId { get; set; }

		private string addressCity = string.Empty;
		public string AddressCity
		{
			get => addressCity;
			set
			{
				if (addressCity == value) return;
				addressCity = value;
				OnPropertyChanged(nameof(AddressCity));
				OnPropertyChanged(nameof(AddressText));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private string addressCountry = string.Empty;
		public string AddressCountry
		{
			get => addressCountry;
			set
			{
				if (addressCountry == value) return;
				addressCountry = value;
				OnPropertyChanged(nameof(AddressCountry));
				OnPropertyChanged(nameof(AddressText));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private string addressStreet = string.Empty;
		public string AddressStreet
		{
			get => addressStreet;
			set
			{
				if (addressStreet == value) return;
				addressStreet = value;
				OnPropertyChanged(nameof(AddressStreet));
				OnPropertyChanged(nameof(AddressText));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private string addressStreetNumber = string.Empty;
		public string AddressStreetNumber
		{
			get => addressStreetNumber;
			set
			{
				if (addressStreetNumber == value) return;
				addressStreetNumber = value;
				OnPropertyChanged(nameof(AddressStreetNumber));
				OnPropertyChanged(nameof(AddressText));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		public Address Address => new Address(AddressStreet, AddressStreetNumber, AddressCity, AddressCountry);
		public string AddressText => $"{AddressStreet} {AddressStreetNumber}, {AddressCity}, {AddressCountry}";

		private string phoneNumber = string.Empty;
		public string PhoneNumber
		{
			get => phoneNumber;
			set
			{
				if (phoneNumber == value) return;
				phoneNumber = value;
				OnPropertyChanged(nameof(PhoneNumber));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private string email = string.Empty;
		public string Email
		{
			get => email;
			set
			{
				if (email == value) return;
				email = value;
				OnPropertyChanged(nameof(Email));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private string idCardNumber = string.Empty;
		public string IdCardNumber
		{
			get => idCardNumber;
			set
			{
				if (idCardNumber == value) return;
				idCardNumber = value;
				OnPropertyChanged(nameof(IdCardNumber));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private string yearsOfExperience;

		public string YearsOfExperience
		{
			get => yearsOfExperience;
			set
			{
				if (yearsOfExperience == value) return;
				yearsOfExperience = value;
				OnPropertyChanged(nameof(YearsOfExperience));
				OnPropertyChanged(nameof(AllFieldsValid));
			}
		}

		private List<Book> books = new List<Book>();

		public List<Book> Books
		{
			get => books;
			set
			{
				if (books == value) return;
				books = value;
				OnPropertyChanged(nameof(Books));
			}
		}

		public bool AllFieldsValid
		{
			get
			{
				return
					!string.IsNullOrWhiteSpace(FirstName) &&
					!string.IsNullOrWhiteSpace(LastName) &&
					DateOfBirth != null &&
					DateOfBirth <= DateOnly.FromDateTime(DateTime.Now) &&
					!string.IsNullOrWhiteSpace(PhoneNumber) &&
					!string.IsNullOrWhiteSpace(Email) &&
					!string.IsNullOrWhiteSpace(IdCardNumber) &&
					Address != null &&
					!string.IsNullOrWhiteSpace(addressStreet) &&
					!string.IsNullOrWhiteSpace(addressCity) &&
					!string.IsNullOrWhiteSpace(addressStreetNumber) &&
					!string.IsNullOrWhiteSpace(addressCountry) &&
					!string.IsNullOrWhiteSpace(YearsOfExperience);
			}
		}
	}
}
