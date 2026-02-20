using Core.Models;
using System;
using System.Collections.Generic;

namespace WPFClient.DTO
{
    public class BookDTO : DTO<Book>, IValidatable
    {

        public BookDTO() : base(new Book()) { }

        public BookDTO(Book book) : base(book) { }

        public override BookDTO Copy()
        {
            return new BookDTO
            {
                Id = Id,
                Isbn = Isbn,
                Title = Title,
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
            Title = book.Title;
            Genre = book.Genre;
            ReleaseYear = book.ReleaseYear > 0 ? book.ReleaseYear.ToString() : string.Empty;
            Price = book.Price > 0 ? book.Price.ToString() : string.Empty;
            NumberOfPages = book.NumberOfPages > 0 ? book.NumberOfPages.ToString() : string.Empty;

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
                Title = Title,
                Genre = Genre,
                ReleaseYear = int.Parse(ReleaseYear),
                Price = double.Parse(Price),
                NumberOfPages = int.Parse(NumberOfPages),

                Authors = Authors ?? new List<Author>(),

                PublisherId = Publisher == null ? 0 : Publisher.Id,
                Publisher = Publisher ?? new Publisher(),

                Buyers = Buyers ?? new List<Visitor>(),
                Wishlisters = Wishlisters ?? new List<Visitor>()
            };
        }

        public int Id { get; set; }

        private string isbn = string.Empty;
        public string Isbn
        {
            get => isbn;
            set
            {
                if (isbn == value) return;
                isbn = value;
                OnPropertyChanged(nameof(Isbn));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private string title = string.Empty;
        public string Title
        {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private BookGenre genre;
        public BookGenre Genre
        {
            get => genre;
            set
            {
                if (genre == value) return;
                genre = value;
                OnPropertyChanged(nameof(Genre));
                OnPropertyChanged(nameof(GenreText));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public string GenreText
        {
            get
            {
                switch (Genre)
                {
                    case BookGenre.FANTASY:
                        return "Fantazija";
                    case BookGenre.SCIENCE_FICTION:
                        return "Naučna fantastika";
                    case BookGenre.MYSTERY:
                        return "Misterija";
                    case BookGenre.THRILLER:
                        return "Triler";
                    case BookGenre.ROMANCE:
                        return "Romansa";
                    case BookGenre.HORROR:
                        return "Horor";
                    case BookGenre.COMEDY:
                        return "Komedija";
                    case BookGenre.NOVEL:
                        return "Roman";
                    case BookGenre.POETRY:
                        return "Poezija";
                    case BookGenre.DRAMA:
                        return "Drama";
                    default:
                        return "Nepoznat";
                }
            }
        }

        private string releaseYear = string.Empty;
        public string ReleaseYear
        {
            get => releaseYear;
            set
            {
                if (releaseYear == value) return;
                releaseYear = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(IsReleaseYearValid));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private bool IsReleaseYearValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ReleaseYear)) return false;
                if (!int.TryParse(ReleaseYear, out int value)) return false;
                return value > 0 && value <= DateTime.Now.Year;
            }
        }

        private string price = string.Empty;
        public string Price
        {
            get => price;
            set
            {
                if (price == value) return;
                price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(IsPriceValid));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private bool IsPriceValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Price)) return false;
                if (!double.TryParse(Price, out double value)) return false;
                return value > 0;
            }
        }

        private string numberOfPages = string.Empty;
        public string NumberOfPages
        {
            get => numberOfPages;
            set
            {
                if (numberOfPages == value) return;
                numberOfPages = value;
                OnPropertyChanged(nameof(NumberOfPages));
                OnPropertyChanged(nameof(IsNumberOfPagesValid));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private bool IsNumberOfPagesValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NumberOfPages)) return false;
                if (!int.TryParse(NumberOfPages, out int value)) return false;
                return value > 0;
            }
        }

        private List<Author> authors = new List<Author>();
        public List<Author> Authors
        {
            get => authors;
            set
            {
                if (authors == value) return;
                authors = value;
                OnPropertyChanged(nameof(Authors));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public int PublisherId { get; set; }

        private Publisher? publisher;
        public Publisher? Publisher
        {
            get => publisher;
            set
            {
                if (publisher == value) return;
                publisher = value;
                PublisherId = publisher?.Id ?? 0;
                OnPropertyChanged(nameof(Publisher));
                OnPropertyChanged(nameof(PublisherId));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private List<Visitor> buyers = new List<Visitor>();
        public List<Visitor> Buyers
        {
            get => buyers;
            set
            {
                if (buyers == value) return;
                buyers = value;
                OnPropertyChanged(nameof(Buyers));
            }
        }

        private List<Visitor> wishlisters = new List<Visitor>();
        public List<Visitor> Wishlisters
        {
            get => wishlisters;
            set
            {
                if (wishlisters == value) return;
                wishlisters = value;
                OnPropertyChanged(nameof(Wishlisters));
            }
        }

        public bool AllFieldsValid =>
            !string.IsNullOrWhiteSpace(Isbn) &&
            !string.IsNullOrWhiteSpace(Title) &&
            IsReleaseYearValid &&
            IsPriceValid &&
            IsNumberOfPagesValid;
    }
}
