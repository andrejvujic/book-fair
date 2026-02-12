using System;
using Core.Models;
using System.ComponentModel;

namespace WPFClient.DTO
{
    public class PurchaseDTO : DTO<Purchase>
    {
        public PurchaseDTO() : base(new Purchase()) { }

        public PurchaseDTO(Purchase purchase) : base(purchase) { }

        protected override void FromModel(Purchase purchase)
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

        public override Purchase ToModel()
        {
            return new Purchase
            {
                Visitor = Visitor ?? new Visitor(),
                VisitorId = VisitorId,
                Book = Book ?? new Book(),
                BookId = BookId,
                Date = Date,
                Rating = Rating,
                Comment = Comment,
                Id = Id
            };
        }

        public override DTO<Purchase> Copy()
        {
            return new PurchaseDTO
            {
                Id = Id,
                VisitorId = VisitorId,
                Visitor = Visitor,
                BookId = BookId,
                Book = Book,
                Date = Date,
                Rating = Rating,
                Comment = Comment
            };
        }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private int visitorId;
        public int VisitorId
        {
            get => visitorId;
            set
            {
                if (visitorId == value) return;
                visitorId = value;
                OnPropertyChanged(nameof(VisitorId));
            }
        }

        private Visitor visitor = new Visitor();
        public Visitor Visitor
        {
            get => visitor;
            set
            {
                if (visitor == value) return;
                visitor = value;
                OnPropertyChanged(nameof(Visitor));
            }
        }

        private int bookId;
        public int BookId
        {
            get => bookId;
            set
            {
                if (bookId == value) return;
                bookId = value;
                OnPropertyChanged(nameof(BookId));
            }
        }

        private Book book = new Book();
        public Book Book
        {
            get => book;
            set
            {
                if (book == value) return;
                book = value;
                OnPropertyChanged(nameof(Book));
            }
        }

        private DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly Date
        {
            get => date;
            set
            {
                if (date == value) return;
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private int rating;
        public int Rating
        {
            get => rating;
            set
            {
                if (rating == value) return;
                rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        private string comment = string.Empty;
        public string Comment
        {
            get => comment;
            set
            {
                if (comment == value) return;
                comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
    }
}
