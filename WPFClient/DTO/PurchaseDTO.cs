using System;
using Core.Models;
using System.ComponentModel;

namespace WPFClient.DTO
{
    public class PurchaseDTO : DTO<Purchase>, IValidatable
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
            RatingText = purchase.Rating > 0 ? purchase.Rating.ToString() : string.Empty;
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
                Date = Date ?? DateTime.Now,
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
                RatingText = RatingText,
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

        private DateTime? date;
        public DateTime? Date
        {
            get => date;
            set
            {
                if (date == value) return;
                date = value;
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private string ratingText = string.Empty;
        public string RatingText
        {
            get => ratingText;
            set
            {
                if (ratingText == value) return;
                ratingText = value;
                OnPropertyChanged(nameof(RatingText));
                OnPropertyChanged(nameof(Rating));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public int Rating { get => int.TryParse(ratingText, out int rating) ? rating : 0; }

        private string comment = string.Empty;
        public string Comment
        {
            get => comment;
            set
            {
                if (comment == value) return;
                comment = value;
                OnPropertyChanged(nameof(Comment));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public bool AllFieldsValid
        {
            get
            {
                return !string.IsNullOrEmpty(RatingText) &&
                        int.TryParse(RatingText, out var r) && r >= 1 && r <= 5 &&
                        Date != null && Date <= DateTime.Now;
            }
        }
    }
}
