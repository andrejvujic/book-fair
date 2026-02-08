using Core.Models;
using System;
using System.ComponentModel;
using System.IO;

namespace WPFClient.DTO
{
    public class VisitorDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string firstName = "";
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string lastName = "";
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private DateTime dateOfBirth = DateTime.Now; // Podrazumijevana vrijednost za prikaz u korisničkom interfejsu.
        public DateTime DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }
            set
            {
                if (dateOfBirth != value)
                {
                    dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        public string phoneNumber = "";
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string emailAddress = "";
        public string EmailAddress
        {
            get
            {
                return emailAddress;
            }
            set
            {
                if (emailAddress != value)
                {
                    emailAddress = value;
                    OnPropertyChanged(nameof(EmailAddress));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        public int AddressId { get; set; }

        private string addressCity = "";
        public string AddressCity
        {
            get
            {
                return addressCity;
            }
            set
            {
                if (addressCity != value)
                {
                    addressCity = value;
                    OnPropertyChanged(nameof(AddressCity));
                    OnPropertyChanged(nameof(AddressText));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string addressCountry = "";
        public string AddressCountry
        {
            get
            {
                return addressCountry;
            }
            set
            {
                if (addressCountry != value)
                {
                    addressCountry = value;
                    OnPropertyChanged(nameof(AddressCountry));
                    OnPropertyChanged(nameof(AddressText));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string addressStreet = "";
        public string AddressStreet
        {
            get
            {
                return addressStreet;
            }
            set
            {
                if (addressStreet != value)
                {
                    addressStreet = value;
                    OnPropertyChanged(nameof(AddressStreet));
                    OnPropertyChanged(nameof(AddressText));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string addressStreetNumber = "";
        public string AddressStreetNumber
        {
            get
            {
                return addressStreetNumber;
            }
            set
            {
                if (addressStreetNumber != value)
                {
                    addressStreetNumber = value;
                    OnPropertyChanged(nameof(AddressStreetNumber));
                    OnPropertyChanged(nameof(AddressText));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        public Address Address
        {
            get
            {
                return new Address(AddressStreet, AddressStreetNumber, AddressCity, AddressCountry);
            }
        }

        public String AddressText
        {
            get
            {
                return $"{AddressStreet} {AddressStreetNumber}, {AddressCity}, {AddressCountry}";
            }
        }

        private double averageReviewRating;
        public double AverageReviewRating
        {
            get
            {
                return averageReviewRating;
            }
            set
            {
                if (averageReviewRating != value)
                {
                    averageReviewRating = value;
                    OnPropertyChanged(nameof(AverageReviewRating));
                    OnPropertyChanged(nameof(AverageReviewRatingText));
                }
            }
        }

        public String AverageReviewRatingText
        {
            get
            {
                if (AverageReviewRating == 0) return "Nema recenzija";
                return AverageReviewRating.ToString("0.00");
            }
        }


        private string membershipCardNumber = "";
        public string MembershipCardNumber
        {
            get
            {
                return membershipCardNumber;
            }
            set
            {
                if (membershipCardNumber != value)
                {
                    membershipCardNumber = value;
                    OnPropertyChanged(nameof(MembershipCardNumber));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private int membershipYear = DateTime.Now.Year; // Podrazumijevana vrijednost za prikaz u korisničkom interfejsu.
        public int MembershipYear
        {
            get
            {
                return membershipYear;
            }
            set
            {
                if (membershipYear != value)
                {
                    membershipYear = value;
                    OnPropertyChanged(nameof(MembershipYear));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private MemberStatus status = MemberStatus.R;
        public MemberStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(StatusText));
                }
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

        public bool AllFieldsValid {
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

        public VisitorDTO()
        {
        }

        public VisitorDTO(Visitor visitor)
        {
            Id = visitor.Id;
            FirstName = visitor.FirstName;
            LastName = visitor.LastName;
            DateOfBirth = visitor.DateOfBirth;
            PhoneNumber = visitor.PhoneNumber;
            EmailAddress = visitor.EmailAddress;
            AddressId = visitor.AddressId;
            AddressCity = visitor.Address.City;
            AddressCountry = visitor.Address.Country;
            AddressStreet = visitor.Address.Street;
            AddressStreetNumber = visitor.Address.StreetNumber;
            MembershipCardNumber = visitor.MembershipCardNumber;
            MembershipYear = visitor.MembershipYear;
            Status = visitor.Status;
        }

        public Visitor ToVisitor()
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
                Status = Status
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
