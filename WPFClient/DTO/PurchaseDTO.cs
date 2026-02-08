using System;
using Core.Models;
using System.ComponentModel;

namespace WPFClient.DTO
{
    public class PurchaseDTO : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private int visitorId;
        public int VisitorId
        {
            get => visitorId;
            set
            {
                if (visitorId != value)
                {
                    visitorId = value;
                    OnPropertyChanged(nameof(VisitorId));
                }
            }
        }

        private Visitor visitor;
        public Visitor Visitor
        {
            get => visitor;
            set
            {
                if (visitor != value)
                {
                    visitor = value;
                    OnPropertyChanged(nameof(Visitor));
                }
            }
        }

        private int bookId;
        public int BookId
        {
            get => bookId;
            set
            {
                if (bookId != value)
                {
                    bookId = value;
                    OnPropertyChanged(nameof(BookId));
                }
            }
        }

        private Book book;
        public Book Book
        {
            get => book;
            set
            {
                if (book != value)
                {
                    book = value;
                    OnPropertyChanged(nameof(Book));
                }
            }
        }

        private DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        private int rating;
        public int Rating
        {
            get => rating;
            set
            {
                if (rating != value)
                {
                    rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        private string comment = "";
        public string Comment
        {
            get => comment;
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public PurchaseDTO() { }

        public PurchaseDTO(Purchase purchase)
        {
            Id = purchase.Id;
            VisitorId = purchase.VisitorId;
            Visitor = purchase.Visitor;
            BookId = purchase.BookId;
            Book = purchase.Book;
            Date = purchase.Date;
            Rating = purchase.Rating;
            Comment = purchase.Comment;
        }

        public Purchase ToPurchase()
        {
            if (Visitor == null) throw new Exception("Visitor can't be null");
            if (Book == null) throw new Exception("Book can't be null");

            return new Purchase
            {
                Visitor = Visitor,
                VisitorId = VisitorId,
                Book = Book,
                BookId = BookId,
                Date =Date,
                Rating=Rating,
                Comment=Comment,
                Id = Id
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
