using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFClient.DTO;

namespace WPFClient.Views.Book.Comparison
{
    public partial class VisitorCategoryOverview : UserControl
    {
        public string RawSearchQuery { get; set; } = string.Empty;
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();

        public static readonly DependencyProperty VisitorsProperty =
            DependencyProperty.Register(
                nameof(Visitors_),
                typeof(List<Core.Models.Visitor>),
                typeof(VisitorCategoryOverview),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public List<Core.Models.Visitor>? Visitors_
        {
            get => (List<Core.Models.Visitor>?)GetValue(VisitorsProperty);
            set => SetValue(VisitorsProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorCategoryOverview control = (VisitorCategoryOverview)d;
            control.Update();
        }

        private void Update()
        {
            if (Visitors_ == null) return;
            Visitors.Clear();
            foreach (var v in Visitors_)
                Visitors.Add(new VisitorDTO(v));
        }

        public VisitorCategoryOverview()
        {
            InitializeComponent();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
