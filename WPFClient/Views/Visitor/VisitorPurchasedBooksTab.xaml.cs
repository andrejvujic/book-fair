using Core.DAO;
using Core.Models;
using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorPurchasedBooksTab : UserControl, IObserver, INotifyPropertyChanged
    {

        public static readonly DependencyProperty VisitorProperty =
            DependencyProperty.Register(
                nameof(Visitor),
                typeof(VisitorDTO),
                typeof(VisitorPurchasedBooksTab),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );

        public VisitorDTO? Visitor
        {
            get => (VisitorDTO?)GetValue(VisitorProperty);
            set => SetValue(VisitorProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorPurchasedBooksTab control = (VisitorPurchasedBooksTab)d;
            control.Update();
        }


        public ObservableCollection<PurchaseDTO> Purchases { get; set; } = new ObservableCollection<PurchaseDTO>();
        private PurchaseController PurchaseController { get; set; }
        private VisitorController VisitorController { get; set; }

        private double totalSpent;
        public double TotalSpent
        {
            get => totalSpent;
            set
            {
                totalSpent = value;
                OnPropertyChanged(nameof(TotalSpent));
            }
        }

        private double averageRating;
        public double AverageRating
        {
            get => averageRating;
            set
            {
                averageRating = value;
                OnPropertyChanged(nameof(AverageRating));
            }
        }

        public VisitorPurchasedBooksTab()
        {
            InitializeComponent();
            PurchaseController = new PurchaseController(App.Context.PurchaseDAO);
            PurchaseController.Subscribe(this);
            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);
            VisitorController.Subscribe(this);
        }  

        public void Update()
        {
            if (Visitor == null) return;

            Purchases.Clear();
            List<Purchase> purchases = PurchaseController.GetByVisitorId(Visitor.Id);
            foreach(Purchase purchase in purchases)
                Purchases.Add(
                    new PurchaseDTO(purchase)
                );

            TotalSpent = PurchaseController.GetTotalSpentByVisitorId(Visitor.Id);
            AverageRating = PurchaseController.GetAverageRatingByVisitorId(Visitor.Id);
        }

        private void DeletePurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Visitor == null) return;

            PurchaseDTO? targetPurchase = PurchasedBooksDataGrid.SelectedItem as PurchaseDTO;
            if (targetPurchase == null)
            {
                MessageBox.Show(
                      "Nije odabrana kupovina za brisanje. Potrebno je odabrati kupovinu iz liste.",
                      "Nije odabrana kupovina",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning
                  );
                return;
            }
            
            PurchaseController.Delete(targetPurchase.Id);
            Update();

            Visitor.WishlistedBooks.Add(targetPurchase.Book);
            // Potrebno je knjigu koja je obrisana iz liste
            // kupljenih knjiga prebaciti u listu želja.
            VisitorController.Update(Visitor.ToModel());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
