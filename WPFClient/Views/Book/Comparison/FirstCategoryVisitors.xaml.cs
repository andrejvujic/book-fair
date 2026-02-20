using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Book.Comparison
{
    public partial class FirstCategoryVisitors : UserControl
    {
        public ObservableCollection<VisitorDTO> Visitors { get; set; } = new ObservableCollection<VisitorDTO>();
        private VisitorController VisitorController { get; }
        public string RawSearchQuery { get; set; } = string.Empty;

        public static readonly DependencyProperty FirstBookIdProperty =
            DependencyProperty.Register(
                nameof(FirstBookId),
                typeof(int?),
                typeof(FirstCategoryVisitors),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public int? FirstBookId
        {
            get => (int?)GetValue(FirstBookIdProperty);
            set => SetValue(FirstBookIdProperty, value);
        }


        public static readonly DependencyProperty SecondBookIdProperty =
            DependencyProperty.Register(
                nameof(SecondBookId),
                typeof(int?),
                typeof(FirstCategoryVisitors),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public int? SecondBookId
        {
            get => (int?)GetValue(SecondBookIdProperty);
            set => SetValue(SecondBookIdProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FirstCategoryVisitors control = (FirstCategoryVisitors)d;
            control.Update();
        }

        public void Update()
        {
            if (FirstBookId == null || SecondBookId == null) return;
            if (VisitorController == null) return;

            Render(
                VisitorController
                .PerformSerach(
                    RawSearchQuery,
                    // Druga kategorija predstavlja posjetioce koji su kupili prvu knjigu,
                    // ali nisu kupili drugu knjigu.
                    (e) => e.WishlistedBooks.Any((b) => b.Id == FirstBookId) && e.WishlistedBooks.Any((b) => b.Id == SecondBookId)
                )
                .PerformSorting()
                .GetWithPagination()
            );
        }

        public FirstCategoryVisitors()
        {
            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);
            InitializeComponent();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FirstBookId == null || SecondBookId == null) return;
            Update();
        }

        private void Render(List<Core.Models.Visitor> visitors)
        {
            Visitors.Clear();
            foreach (Core.Models.Visitor v in visitors)
                Visitors.Add(new VisitorDTO(v));
        }
    }
}
