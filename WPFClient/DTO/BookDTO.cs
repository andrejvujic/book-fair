using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WPFClient.DTO
{
    public class BookDTO : DTO<Book>, IValidatable
    {
        public int Id { get; set; }

        private string isbn = string.Empty;
        public string Isbn
        {
            get => isbn;
            set
            {
                if (isbn != value)
                {
                    isbn = value;
                    OnPropertyChanged(nameof(Isbn));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string bookTitle = string.Empty;
        public string BookTitle
        {
            get => bookTitle;
            set
            {
                if (bookTitle != value)
                {
                    bookTitle = value;
                    OnPropertyChanged(nameof(BookTitle));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private string genre = string.Empty;
        public string Genre
        {
            get => genre;
            set
            {
                if (genre != value)
                {
                    genre = value;
                    OnPropertyChanged(nameof(Genre));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private int releaseYear;
        public int ReleaseYear
        {
            get => releaseYear;
            set
            {
                if (releaseYear != value)
                {
                    releaseYear = value;
                    OnPropertyChanged(nameof(ReleaseYear));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private int price;
        public int Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private int? numberOfPages;
        public int NumberOfPages
        {
            get => numberOfPages ?? 0;
            set
            {
                if (numberOfPages != value)
                {
                    numberOfPages = value;
                    OnPropertyChanged(nameof(NumberOfPages));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private List<Author> authors = new List<Author>();
        public List<Author> Authors
        {
            get => authors;
            set
            {
                if (authors != value)
                {
                    authors = value;
                    OnPropertyChanged(nameof(Authors));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        public int PublisherId { get; set; }

        private Publisher? publisher;
        public Publisher? Publisher
        {
            get => publisher;
            set
            {
                if (publisher != value)
                {
                    publisher = value;
                    PublisherId = publisher?.Id ?? 0;
                    OnPropertyChanged(nameof(Publisher));
                    OnPropertyChanged(nameof(PublisherId));
                    OnPropertyChanged(nameof(AllFieldsValid));
                }
            }
        }

        private List<Visitor> buyers = new List<Visitor>();
        public List<Visitor> Buyers
        {
            get => buyers;
            set
            {
                if (buyers != value)
                {
                    buyers = value;
                    OnPropertyChanged(nameof(Buyers));
                }
            }
        }

        private List<Visitor> wishlisters = new List<Visitor>();
        public List<Visitor> Wishlisters
        {
            get => wishlisters;
            set
            {
                if (wishlisters != value)
                {
                    wishlisters = value;
                    OnPropertyChanged(nameof(Wishlisters));
                }
            }
        }

        public bool AllFieldsValid
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(Isbn) &&
                    !string.IsNullOrWhiteSpace(BookTitle) &&
                    !string.IsNullOrWhiteSpace(Genre) &&

                    ReleaseYear <= DateTime.Now.Year &&
                    Price >= 0 &&
                    NumberOfPages > 0;
            }
        }

        public BookDTO() : base(new Book()) { }

        public BookDTO(Book book) : base(book) {}

        public override BookDTO Copy()
        {
            return new BookDTO
            {
                Id = Id,
                Isbn = Isbn,
                BookTitle = BookTitle,
                Genre = Genre,
                ReleaseYear = ReleaseYear,
                Price = Price,
                NumberOfPages = NumberOfPages,

                Authors = new List<Author>(Authors),

                PublisherId = PublisherId,
                Publisher = Publisher,

                Buyers = new List<Visitor>(Buyers),
                Wishlisters = new List<Visitor>(Wishlisters)
            };
        }

        protected override void FromModel(Book book)
        {
            Id = book.Id;
            Isbn = book.Isbn;
            BookTitle = book.BookTitle;
            Genre = book.Genre;
            ReleaseYear = book.ReleaseYear;
            Price = book.Price;
            NumberOfPages = book.NumberOfPages;

            Authors = book.Authors ?? new List<Author>();

            PublisherId = book.PublisherId;
            Publisher = book.Publisher;

            Buyers = book.Buyers ?? new List<Visitor>();
            Wishlisters = book.Wishlisters ?? new List<Visitor>();
        }

        public override Book ToModel()
        {
            return new Book
            {
                Id = Id,
                Isbn = Isbn,
                BookTitle = BookTitle,
                Genre = Genre,
                ReleaseYear = ReleaseYear,
                Price = Price,
                NumberOfPages = NumberOfPages,

                Authors = Authors ?? new List<Author>(),

                PublisherId = Publisher == null ? 0 : Publisher.Id,
                Publisher = Publisher ?? new Publisher(),

                Buyers = Buyers ?? new List<Visitor>(),
                Wishlisters = Wishlisters ?? new List<Visitor>()
            };
        }
    }
}
