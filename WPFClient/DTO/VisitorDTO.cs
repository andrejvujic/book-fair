using Core.Models;
using System;
using System.Collections.Generic;

namespace WPFClient.DTO
{
    public class VisitorDTO : DTO<Visitor>, IValidatable
    {
        public VisitorDTO() : base(new Visitor()) { }

        public VisitorDTO(Visitor visitor) : base(visitor)
        {
        }

        public override VisitorDTO Copy()
        {
            return new VisitorDTO
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                PhoneNumber = PhoneNumber,
                EmailAddress = EmailAddress,
                AddressId = AddressId,
                AddressCity = AddressCity,
                AddressCountry = AddressCountry,
                AddressStreet = AddressStreet,
                AddressStreetNumber = AddressStreetNumber,
                MembershipCardNumber = MembershipCardNumber,
                MembershipYear = MembershipYear,
                Status = Status,
                AverageReviewRating = AverageReviewRating,
                WishlistedBooks = new List<Book>(WishlistedBooks),
                PurchasedBooks = new List<Book>(PurchasedBooks),
                IsDeleted = IsDeleted,
            };
        }

        protected override void FromModel(Visitor visitor)
        {
            Id = visitor.Id;
            FirstName = visitor.FirstName;
            LastName = visitor.LastName;
            DateOfBirth = visitor.DateOfBirth;
            PhoneNumber = visitor.PhoneNumber;
            EmailAddress = visitor.EmailAddress;
            AddressId = visitor.AddressId;
            AddressCity = visitor.Address?.City ?? string.Empty;
            AddressCountry = visitor.Address?.Country ?? string.Empty;
            AddressStreet = visitor.Address?.Street ?? string.Empty;
            AddressStreetNumber = visitor.Address?.StreetNumber ?? string.Empty;
            MembershipCardNumber = visitor.MembershipCardNumber;
            MembershipYear = visitor.MembershipYear;
            AverageReviewRating = visitor.AverageReviewRating;
            Status = visitor.Status;
            WishlistedBooks = visitor.WishlistedBooks;
            PurchasedBooks = visitor.PurchasedBooks;
            IsDeleted = visitor.IsDeleted;
        }

        public override Visitor ToModel()
        {
            return new Visitor
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                PhoneNumber = PhoneNumber,
                EmailAddress = EmailAddress,
                AddressId = AddressId,
                Address = Address,
                MembershipCardNumber = MembershipCardNumber,
                MembershipYear = MembershipYear,
                Status = Status,
                AverageReviewRating = AverageReviewRating,
                WishlistedBooks = WishlistedBooks,
                PurchasedBooks = PurchasedBooks,
                IsDeleted = IsDeleted,
            };
        }

        public int Id { get; set; }

        private bool isDeleted = false;
        public bool IsDeleted
        {
            get => isDeleted;
            set
            {
                if (isDeleted == value) return;
                isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

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

        private DateTime dateOfBirth = DateTime.Now; // Podrazumijevana vrijednost za prikaz u korisničkom interfejsu.
        public DateTime DateOfBirth
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

        public string phoneNumber = string.Empty;
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

        private string emailAddress = string.Empty;
        public string EmailAddress
        {
            get => emailAddress;
            set
            {
                if (emailAddress == value) return;
                emailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
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
        public String AddressText => $"{AddressStreet} {AddressStreetNumber}, {AddressCity}, {AddressCountry}";

        private double averageReviewRating;
        public double AverageReviewRating
        {
            get => averageReviewRating;
            set
            {
                if (averageReviewRating == value) return;
                averageReviewRating = value;
                OnPropertyChanged(nameof(AverageReviewRating));
                OnPropertyChanged(nameof(AverageReviewRatingText));
            }
        }

        // Koristi se za prikaz prosječne ocjene recenzija u korisničkom interfejsu
        // (na po na početnom ekranu na tab-u za posjetioce unutar DataGrid-a) .
        public String AverageReviewRatingText
        {
            get
            {
                if (AverageReviewRating == 0) return "Nema recenzija";
                return AverageReviewRating.ToString("0.00");
            }
        }

        private string membershipCardNumber = string.Empty;
        public string MembershipCardNumber
        {
            get => membershipCardNumber;
            set
            {
                if (membershipCardNumber == value) return;
                membershipCardNumber = value;
                OnPropertyChanged(nameof(MembershipCardNumber));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private int membershipYear = DateTime.Now.Year; // Podrazumijevana vrijednost za prikaz u korisničkom interfejsu.
        public int MembershipYear
        {
            get => membershipYear;
            set
            {
                if (membershipYear == value) return;
                membershipYear = value;
                OnPropertyChanged(nameof(MembershipYear));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private MemberStatus status = MemberStatus.R;
        public MemberStatus Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public String StatusText
        {
            get
            {
                switch (Status)
                {
                    case MemberStatus.R:
                        return "Redovan";
                    case MemberStatus.V:
                        return "VIP";
                    default:
                        return "Nepoznat";
                }
            }
        }

        private List<Book> wishlistedBooks = new List<Book>();
        public List<Book> WishlistedBooks
        {
            get => wishlistedBooks;
            set
            {
                if (wishlistedBooks == value) return;
                wishlistedBooks = value;
                OnPropertyChanged(nameof(WishlistedBooks));
            }
        }

        private List<Book> purchasedBooks = new List<Book>();
        public List<Book> PurchasedBooks
        {
            get => purchasedBooks;
            set
            {
                if (purchasedBooks == value) return;
                purchasedBooks = value;
                OnPropertyChanged(nameof(PurchasedBooks));
            }
        }

        // Koristi se za omogućavanje dugmeta za čuvanje promjena (promjene je moguće sačuvati
        // samo ako su svi obavezni podaci ispravno uneseni).
        public bool AllFieldsValid
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(FirstName) &&
                    !string.IsNullOrWhiteSpace(LastName) &&
                    DateOfBirth <= DateTime.Now && // Datum rođenja ne može biti u budućnosti.
                    !string.IsNullOrWhiteSpace(PhoneNumber) &&
                    !string.IsNullOrWhiteSpace(EmailAddress) &&
                    Address != null &&
                    !string.IsNullOrWhiteSpace(AddressStreet) &&
                    !string.IsNullOrWhiteSpace(AddressStreetNumber) &&
                    !string.IsNullOrWhiteSpace(AddressCity) &&
                    !string.IsNullOrWhiteSpace(AddressCountry) &&
                    !string.IsNullOrWhiteSpace(MembershipCardNumber) &&
                    MembershipYear > 1900 && MembershipYear <= DateTime.Now.Year; // Pretpostavljamo da godina članstva ne može biti prije 1900. i ne može biti u budućnosti.
            }
        }
    }
}
