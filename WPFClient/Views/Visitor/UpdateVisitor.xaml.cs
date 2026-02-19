using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class UpdateVisitor : Window
    {
        public VisitorDTO Visitor { get; set; }
        public VisitorController VisitorController { get; set; }

        public UpdateVisitor(VisitorDTO visitor, VisitorController visitorController)
        {
            InitializeComponent();

            Visitor = visitor.Copy();
            VisitorController = visitorController;

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            VisitorController.Update(Visitor.ToModel());
            // TODO: Ovde treba da se koristi AddressController kada se napravi...
            App.Context.AddressDAO.Update(Visitor.Address);
            Close();
        }
    }
}
