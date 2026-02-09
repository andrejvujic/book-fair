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

        public static readonly DependencyProperty VisitorControllerProperty =
                    DependencyProperty.Register(
                        nameof(VisitorController),
                        typeof(VisitorController),
                        typeof(VisitorWishlistedBooksTab),
                        new PropertyMetadata(null, OnDependencyPropertyChanged)
                    );


        public VisitorController? VisitorController
        {
            get => (VisitorController?)GetValue(VisitorControllerProperty);
            set => SetValue(VisitorControllerProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisitorWishlistedBooksTab control = (VisitorWishlistedBooksTab)d;
            control.Update();
            if (control.VisitorController == null) return;
            control.VisitorController.Subscribe(control);
        }

        public ObservableCollection<Book> WishlistedBooks { get; set; } = new ObservableCollection<Book>();

        public VisitorWishlistedBooksTab()
        {
            InitializeComponent();
        }

        public void Update()
        {
            Trace.WriteLine("VisitorWishlistedBooksTab Update called");
            if (Visitor == null) return;

            WishlistedBooks.Clear();
            foreach (Book book in Visitor.WishlistedBooks)
                WishlistedBooks.Add(book);

        }
    }
}
