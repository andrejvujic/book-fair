using Core.Models;
using Core.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class VisitorWishlistedBooksTab : UserControl, IObserver
    {
        public static readonly DependencyProperty VisitorProperty =
            DependencyProperty.Register(
                nameof(Visitor),
                typeof(VisitorDTO),
                typeof(VisitorWishlistedBooksTab),
                new PropertyMetadata(null, OnDependencyPropertyChanged)
            );


        public VisitorDTO? Visitor
        {
            get => (VisitorDTO?)GetValue(VisitorProperty);
            set => SetValue(VisitorProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorWishlistedBooksTab control = (VisitorWishlistedBooksTab)d;
            control.Update();
        }

        public ObservableCollection<Core.Models.Book> WishlistedBooks { get; set; } = new ObservableCollection<Core.Models.Book>();
        private VisitorController VisitorController { get; set; }

        public VisitorWishlistedBooksTab()
        {
            InitializeComponent();
            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);
            VisitorController.Subscribe(this);
        }

        public void Update()
        {
            if (Visitor == null) return;

            WishlistedBooks.Clear();
            foreach (Core.Models.Book book in Visitor.WishlistedBooks)
                WishlistedBooks.Add(book);
        }

        private void MakePurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            Core.Models.Book? targetBook = WishlistedBooksDataGrid.SelectedItem as Core.Models.Book;
            if (targetBook == null)
            {
                MessageBox.Show(
                      "Nije odabrana knjiga za kupovinu. Potrebno je odabrati knjigu iz liste, a zatim odabrati opciju 'Kupovina'.",
                      "Nije odabrana knjiga za kupovinu",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning
                  );
                return;
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Visitor == null) return;

            Core.Models.Book? targetBook = WishlistedBooksDataGrid.SelectedItem as Core.Models.Book;
            if (targetBook == null)
            {
                MessageBox.Show(
                      "Nije odabrana knjiga za brisanje. Potrebno je odabrati knjigu iz liste, a zatim odabrati opciju 'Obriši'.",
                      "Nije odabrana knjiga za brisanje",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning
                  );
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite da obrišete knjigu iz liste želja?",
                "Brisanje knjige",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            switch (result)
                {
                    case MessageBoxResult.Yes:
                        Visitor.WishlistedBooks.Remove(targetBook);
                        VisitorController.Update(Visitor.ToModel());
                    break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
        }
    }
}
