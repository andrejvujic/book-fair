using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class UpdateVisitor : Window
    {
        public VisitorDTO Visitor { get; set; }
        private VisitorController visitorController;

        public UpdateVisitor(VisitorDTO visitor, VisitorController visitorController)
        {
            InitializeComponent();
            DataContext = this;
            Visitor = visitor.Copy();
            this.visitorController = visitorController;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            visitorController.Update(Visitor.ToVisitor());
            Close();
        }
    }
}
