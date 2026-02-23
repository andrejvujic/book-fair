using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Publisher
{
    public partial class EditPublisher : Window
    {
        public PublisherDTO Publisher { get; set; }
        private PublisherController PublisherController { get; set; } = new PublisherController(App.Context.PublisherDAO);

        public EditPublisher(PublisherDTO publisher)
        {
            InitializeComponent();
            Publisher = publisher.Copy();
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PublisherController.Update(Publisher.ToModel());
            Close();
        }
    }
}
