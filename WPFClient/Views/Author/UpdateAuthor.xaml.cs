using System.Windows;
using Core.Models;
namespace WPFClient.Views.Author
{
    public partial class UpdateAuthor : Window
    {
        public Core.Models.Author Author { get; set; }

        public UpdateAuthor(Core.Models.Author author)
        {
            InitializeComponent();

            Author = author;

            DataContext = this;
        }
    }
}
