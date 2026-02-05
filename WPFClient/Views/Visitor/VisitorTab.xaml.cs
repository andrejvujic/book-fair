using Core.DAO;
using Core.Observer;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorTab : UserControl, IObserver
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private VisitorDAO VisitorDAO { get; set; }

        public VisitorTab()
        {
            InitializeComponent();
            DataContext = this;

            VisitorDAO = App.Context.VisitorDAO;
            VisitorDAO.Subject.Subscribe(this);
            Update();
        }

        public void Update()
        {
            Visitors.Clear();
            foreach (var v in VisitorDAO.GetAll())
            {
                Visitors.Add(new VisitorDTO
                {
                    Id = v.Id,
                    FirstName = v.FirstName,
                    LastName = v.LastName,
                    DateOfBirth = v.DateOfBirth,
                    PhoneNumber = v.PhoneNumber,
                    EmailAddress = v.EmailAddress,
                    AddressId = v.AddressId,
                    Address = v.Address,
                    MembershipCardNumber = v.MembershipCardNumber,
                    MembershipYear = v.MembershipYear,
                    Status = v.Status,
                    AverageReviewRating = v.AverageReviewRating,
                });
            }
        }
    }
}
