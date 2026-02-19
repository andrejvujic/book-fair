using Core.DAO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Visitor
{
    public partial class AddVisitor : Window
    {
        public VisitorDTO Visitor { get; set; }
        private VisitorController visitorController;

        public AddVisitor(VisitorController visitorController)
        {
            InitializeComponent();
            DataContext = this;
            Visitor = new VisitorDTO();
            this.visitorController = visitorController;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            visitorController.Add(Visitor.ToModel());
            Close();
        }

        // Potrebno zbog povezivanja sa poljem za unos statusa člana u korisničkom interfejsu.
    }
}