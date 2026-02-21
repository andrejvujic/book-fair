using Core.Models;
using System.Collections.Generic;

namespace WPFClient.DTO
{
    public class PublisherDTO : DTO<Publisher>, IValidatable
    {
        public PublisherDTO() : base(new Publisher()) { }

        public PublisherDTO(Publisher publisher) : base(publisher) { }

        public override PublisherDTO Copy()
        {
            return new PublisherDTO
            {
                Id = Id,
                Name = Name,
                BossId = BossId,
                Boss = Boss,
                Authors = new List<Author>(Authors),
                Books = new List<Book>(Books),
            };
        }

        protected override void FromModel(Publisher publisher)
        {
            Id = publisher.Id;
            Name = publisher.Name;

            BossId = publisher.BossId;
            Boss = publisher.Boss;

            Authors = publisher.Authors ?? new List<Author>();
            Books = publisher.Books ?? new List<Book>();
        }

        public override Publisher ToModel()
        {
            return new Publisher
            {
                Id = Id,
                Name = Name,
                BossId = BossId,
                Boss = Boss ?? new Author(),
                Authors = Authors,
                Books = Books
            };
        }

        public int Id { get; set; }

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }


        private int bossId;
        public int BossId
        {
            get => bossId;
            set
            {
                if (bossId == value) return;
                bossId = value;
                OnPropertyChanged(nameof(BossId));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        private Author? boss;
        public Author? Boss {
            get => boss;
            set
            {
                if (boss == value) return;
                boss = value;
                OnPropertyChanged(nameof(Boss));
                OnPropertyChanged(nameof(FullNameOfBoss));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public string FullNameOfBoss {
            get
            {
                if (Boss == null) return "Nema";
                return $"{Boss.FirstName} {Boss.LastName}";
            }
        }

        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Book> Books { get; set; } = new List<Book>();

        public bool AllFieldsValid
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(Name);
            }
        }
    }
}
