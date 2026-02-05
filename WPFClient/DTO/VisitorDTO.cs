using Core.Models;
using System;
using System.ComponentModel;

namespace WPFClient.DTO
{
    public class VisitorDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string firstName;
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
                }
            }
        }

        private string lastName;
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
                }
            }
        }

        private DateOnly dateOfBirth;
        public DateOnly DateOfBirth
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
                }
            }
        }

        public string phoneNumber;
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
                }
            }
        }

        private string emailAddress;
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
                }
            }
        }

        public int AddressId { get; set; }

        private Address address;
        public Address Address
        {
            get
            {
                return address;
            }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                    OnPropertyChanged(nameof(AddressText));
                }
            }
        }
        public String AddressText
        {
            get
            {
                if (Address == null) return "Nepoznata";
                return $"{Address.Street} {Address.StreetNumber}, {Address.City}, {Address.Country}";
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
                }
            }
        }

        private int membershipYear;
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
                }
            }
        }

        private MemberStatus status;
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
            Address = visitor.Address;
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
