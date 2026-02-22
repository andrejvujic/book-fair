using System.Windows;
using Core.Models;
using WPFClient.DTO;
namespace WPFClient.Views.Author
{
    public partial class UpdateAuthor : Window
    {
        public AuthorDTO Author { get; set; }

        public UpdateAuthor(AuthorDTO author)
        {
            InitializeComponent();
            Author = author;
            DataContext = this;
        }
    }
}
