using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
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

                ReleaseYearText = ReleaseYearText,
                PriceText = PriceText,
                NumberOfPagesText = NumberOfPagesText,

                Authors = new ObservableCollection<Author>(Authors),

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

            ReleaseYearText = book.ReleaseYear > 0 ? book.ReleaseYear.ToString() : string.Empty;
            PriceText = book.Price > 0 ? book.Price.ToString(CultureInfo.InvariantCulture) : string.Empty;
            NumberOfPagesText = book.NumberOfPages > 0 ? book.NumberOfPages.ToString() : string.Empty;

            Authors = new ObservableCollection<Author>(book.Authors ?? new List<Author>());

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

                ReleaseYear = ReleaseYear,
                Price = Price,
                NumberOfPages = NumberOfPages,

                Authors = Authors == null ? new List<Author>() : new List<Author>(Authors),

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

        private string releaseYearText = string.Empty;
        public string ReleaseYearText
        {
            get => releaseYearText;
            set
            {
                if (releaseYearText == value) return;
                releaseYearText = value;
                OnPropertyChanged(nameof(ReleaseYearText));
                OnPropertyChanged(nameof(ReleaseYear));
                OnPropertyChanged(nameof(IsReleaseYearValid));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public int ReleaseYear =>
            int.TryParse(ReleaseYearText, out int value) ? value : 0;

        private bool IsReleaseYearValid =>
            int.TryParse(ReleaseYearText, out int value)
            && value > 0
            && value <= DateTime.Now.Year;

        private string priceText = string.Empty;
        public string PriceText
        {
            get => priceText;
            set
            {
                if (priceText == value) return;
                priceText = value;
                OnPropertyChanged(nameof(PriceText));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(IsPriceValid));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public double Price =>
            double.TryParse(PriceText, out double value) ? value : 0;

        private bool IsPriceValid =>
            double.TryParse(PriceText, out double value) && value > 0;

        private string numberOfPagesText = string.Empty;
        public string NumberOfPagesText
        {
            get => numberOfPagesText;
            set
            {
                if (numberOfPagesText == value) return;
                numberOfPagesText = value;
                OnPropertyChanged(nameof(NumberOfPagesText));
                OnPropertyChanged(nameof(NumberOfPages));
                OnPropertyChanged(nameof(IsNumberOfPagesValid));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public int NumberOfPages =>
            int.TryParse(NumberOfPagesText, out int value) ? value : 0;

        private bool IsNumberOfPagesValid =>
            int.TryParse(NumberOfPagesText, out int value) && value > 0;

        private ObservableCollection<Author> authors = new();
        public ObservableCollection<Author> Authors
        {
            get => authors;
            set
            {
                if (authors == value) return;

                if (authors != null)
                    authors.CollectionChanged -= Authors_CollectionChanged;

                authors = value ?? new ObservableCollection<Author>();
                authors.CollectionChanged += Authors_CollectionChanged;

                OnPropertyChanged(nameof(Authors));
                OnPropertyChanged(nameof(FirstAuthor));
                OnPropertyChanged(nameof(FirstAuthorFullName));
                OnPropertyChanged(nameof(HasGotAuthors));
                OnPropertyChanged(nameof(HasntGotAuthors));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private void Authors_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FirstAuthor));
            OnPropertyChanged(nameof(FirstAuthorFullName));
            OnPropertyChanged(nameof(HasGotAuthors));
            OnPropertyChanged(nameof(HasntGotAuthors));
            OnPropertyChanged(nameof(AllFieldsValid));
        }

        public List<string> FirstAndLastNamesOfAllAuthors
        {
            get
            {
                List<string> names = new List<string>();
                foreach (Author author in Authors.Where(a => !a.IsDeleted))
                    names.Add($"{author.FirstName} {author.LastName}");
                return names;
            }
        }

        public Author? FirstAuthor => Authors?.FirstOrDefault(a => !a.IsDeleted);

        public string FirstAuthorFullName
        {
            get
            {
                var a = Authors?.FirstOrDefault(a => !a.IsDeleted);
                return a != null ? $"{a.FirstName} {a.LastName}" : string.Empty;
            }
        }

        public bool HasGotAuthors { get => FirstAuthor != null; }
        public bool HasntGotAuthors { get => FirstAuthor == null; }

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
            HasGotPublisher &&
            HasGotAuthors;

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