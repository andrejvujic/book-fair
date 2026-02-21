using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Publisher
{
    public partial class AddPublisher : Window
    {
        public PublisherDTO Publisher { get; set; } = new PublisherDTO();
        private PublisherController PublisherController { get; set; } = new PublisherController(App.Context.PublisherDAO);

        public AddPublisher()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PublisherController.Add(Publisher.ToModel());
            Close();
        }
    }
}
