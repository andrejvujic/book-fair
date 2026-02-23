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
                CodeText = CodeText,
                IsDeleted = IsDeleted,
            };
        }

        protected override void FromModel(Publisher publisher)
        {
            Id = publisher.Id;
            Name = publisher.Name;

            CodeText = publisher.Code > -1 ? publisher.Code.ToString() : string.Empty;

            BossId = publisher.BossId;
            Boss = publisher.Boss;

            Authors = publisher.Authors ?? new List<Author>();
            Books = publisher.Books ?? new List<Book>();

            IsDeleted = publisher.IsDeleted;
        }

        public override Publisher ToModel()
        {
            return new Publisher
            {
                Id = Id,
                Name = Name,
                Code = Code,
                BossId = BossId,
                Boss = Boss ?? new Author(),
                Authors = Authors,
                Books = Books,
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

        private string codeText = string.Empty;
        public string CodeText
        {
            get => codeText;
            set
            {
                if (codeText == value) return;
                codeText = value;
                OnPropertyChanged(nameof(CodeText));
                OnPropertyChanged(nameof(Code));
                OnPropertyChanged(nameof(AllFieldsValid));
            }
        }

        public int Code
        { 
            get { 
                if (int.TryParse(codeText, out int code)) return code;
                return -1;
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
                return !string.IsNullOrWhiteSpace(Name) && Code != -1;
            }
        }
    }
}
