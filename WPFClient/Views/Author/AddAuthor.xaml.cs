using Core.DAO;
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

namespace WPFClient.Views.Author
{
    public partial class AddAuthor : Window
    {
        public AuthorDTO Author { get; set; }
        private AuthorController authorController;
        private readonly AddressDAO addressDAO = new AddressDAO();

        public AddAuthor(AuthorController authorController)
        {
            InitializeComponent();
            DataContext = this;
            Author = new AuthorDTO();
            this.authorController = authorController;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Author.AddressId = addressDAO.Add(Author.Address);
            authorController.Add(Author.ToModel());
            Close();
		}
	}
}
