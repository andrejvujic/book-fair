using Core.Models;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Purchase
{
    public partial class MakePurchase : Window
    {
        public PurchaseDTO Purchase { get; set; }
        private PurchaseController PurchaseController { get; } = new PurchaseController(App.Context.PurchaseDAO);

        public MakePurchase(Core.Models.Visitor visitor, Core.Models.Book book)
        {
            InitializeComponent();
            Purchase = new PurchaseDTO();
            Purchase.Visitor = visitor;
            Purchase.VisitorId = visitor.Id;
            Purchase.Book = book;
            Purchase.BookId = book.Id;

            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PurchaseController.Add(Purchase.ToModel());
            DialogResult = true;
            Close();
        }
    }
}
