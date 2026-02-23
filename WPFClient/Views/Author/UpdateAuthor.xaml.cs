using System.Windows;
using Core.Models;
using WPFClient.Controllers;
using WPFClient.DTO;
namespace WPFClient.Views.Author
{
    public partial class UpdateAuthor : Window
    {
        public AuthorDTO Author { get;set; }
        public AuthorController AuthorController { get;set; }   

        public UpdateAuthor(AuthorDTO author, AuthorController authorController)
        {
            InitializeComponent();

            Author = author.Copy();
            AuthorController = authorController;

            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorController.Update(Author.ToModel());
            App.Context.AddressDAO.Update(Author.Address);
            Close();
		}
	}
}
