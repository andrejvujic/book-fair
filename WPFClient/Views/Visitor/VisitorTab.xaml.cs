using Core.DAO;
using Core.Helpers;
using Core.Helpers.Search;
using Core.Observer;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorTab : UserControl, IObserver
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private VisitorDAO VisitorDAO { get; set; }
        public VisitorDTO? SelectedVisitor => VisitorsDataGrid.SelectedItem as VisitorDTO;

        public static readonly DependencyProperty RawSearchQueryProperty =
            DependencyProperty.Register(
                nameof(RawSearchQuery),
                typeof(string),
                typeof(VisitorTab),
                new PropertyMetadata(null, OnSearchQueryPropertyChanged)
            );


        public string? RawSearchQuery
        {
            get => (string?)GetValue(RawSearchQueryProperty);
            set => SetValue(RawSearchQueryProperty, value);
        }

        private static void OnSearchQueryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorTab control = (VisitorTab)d;
            control.Update();
        }

        public VisitorTab()
        {
            InitializeComponent();

            VisitorDAO = App.Context.VisitorDAO;
            VisitorDAO.Subject.Subscribe(this);
            Update();
        }

        public void Update()
        {
            SearchQuery query = new SearchQuery(RawSearchQuery ?? string.Empty, 3);

            Visitors.Clear();
            // Prikazuje samo posjetioce koji nisu obrisani.
            // Brisanje je logicko sto znaci da se (logicki) obrisani
            // posjetioci ignorisu, nema fizickog brisanja zbog komplikacija
            // oko vezivanja sa drugim entitetima.
            foreach (var v in VisitorDAO.GetWhere((e) => !e.IsDeleted).Search(query))
                Visitors.Add(new VisitorDTO(v));
            
        }
    }
}
