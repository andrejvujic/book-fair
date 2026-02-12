using Core.DAO;
using Core.Models;
using Core.Observer;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorTab : UserControl, IObserver
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private VisitorDAO VisitorDAO { get; set; }
        public VisitorDTO? SelectedVisitor => VisitorsDataGrid.SelectedItem as VisitorDTO;

        public VisitorTab()
        {
            InitializeComponent();

            VisitorDAO = App.Context.VisitorDAO;
            VisitorDAO.Subject.Subscribe(this);
            Update();

            DataContext = this;
        }

        public void Update()
        {
            Visitors.Clear();
            // Prikazuje samo posjetioce koji nisu obrisani.
            // Brisanje je logicko sto znaci da se (logicki) obrisani
            // posjetioci ignorisu, nema fizickog brisanja zbog komplikacija
            // oko vezivanja sa drugim entitetima.
            foreach (var v in VisitorDAO.GetWhere((e) => !e.IsDeleted))
                Visitors.Add(new VisitorDTO(v));
            
        }
    }
}
