using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WPFClient.Localization;

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

                Publisher = Publisher,

                Buyers = new List<Visitor>(Buyers),
                Wishlisters = new List<Visitor>(Wishlisters),

                IsDeleted = IsDeleted,
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

            Publisher = book.PublisherId > -1 ? book.Publisher : null;

            Buyers = book.Buyers ?? new List<Visitor>();
            Wishlisters = book.Wishlisters ?? new List<Visitor>();

            IsDeleted = book.IsDeleted;
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
                Wishlisters = Wishlisters ?? new List<Visitor>(),

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
                string key = Genre switch
                {
                    BookGenre.FANTASY => "Genre_Fantasy",
                    BookGenre.SCIENCE_FICTION => "Genre_ScienceFiction",
                    BookGenre.MYSTERY => "Genre_Mystery",
                    BookGenre.THRILLER => "Genre_Thriller",
                    BookGenre.ROMANCE => "Genre_Romance",
                    BookGenre.HORROR => "Genre_Horror",
                    BookGenre.COMEDY => "Genre_Comedy",
                    BookGenre.NOVEL => "Genre_Novel",
                    BookGenre.POETRY => "Genre_Poetry",
                    BookGenre.DRAMA => "Genre_Drama",
                    _ => "Genre_Unknown"
                };

                return TranslationSource.Instance[key];
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
                OnPropertyChanged(nameof(FirstAndLastNamesOfAllAuthors));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public List<string> FirstAndLastNamesOfAllAuthors
        {
            get
            {
                List<string> names = new List<string>();
                foreach (Author author in Authors)
                    names.Add($"{author.FirstName} {author.LastName}");
                return names;
            }
        }

        public int PublisherId { get => Publisher == null ? -1 : Publisher.Id; }

        private Publisher? publisher;
        public Publisher? Publisher
        {
            get => publisher;
            set
            {
                if (publisher == value) return;
                publisher = value;
                OnPropertyChanged(nameof(Publisher));
                OnPropertyChanged(nameof(PublisherId));
                OnPropertyChanged(nameof(HasGotPublisher));
                OnPropertyChanged(nameof(HasntGotPublisher));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public bool HasGotPublisher { get => Publisher != null; }
        public bool HasntGotPublisher { get => Publisher == null; }


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
            IsIsbnValid &&
            !string.IsNullOrWhiteSpace(Title) &&
            IsReleaseYearValid &&
            IsPriceValid &&
            IsNumberOfPagesValid &&
            HasGotPublisher;
        private bool IsIsbnValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Isbn)) return false;
                return Isbn.Length == 13 && Isbn.All(char.IsDigit);
            }
        }
    }
}
