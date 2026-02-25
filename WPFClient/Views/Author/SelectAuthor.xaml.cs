using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Core.Observer;
using System.Collections.ObjectModel;
using System.Windows;
using WPFClient.Controllers;
using WPFClient.DTO;

namespace WPFClient.Views.Author
{
    public partial class SelectAuthor : Window, IObserver
    {
        public AuthorDTO? SelectedAuthor { get; set; }
        public ObservableCollection<AuthorDTO> Authors { get; set; } = new ObservableCollection<AuthorDTO>();
        private AuthorController AuthorController { get; }
		public SelectAuthor()
        {
            InitializeComponent();

            AuthorController = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO, App.Context.BookDAO);
            AuthorController.Subscribe(this);
            DataContext = this;
            Update();
		}

        public void Update()
        {
            Authors.Clear();
            foreach (var a in AuthorController.GetWhere((e) => !e.IsDeleted))
                Authors.Add(new AuthorDTO(a));
		}

		private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorDTO? author = AuthorsDataGrid.SelectedItem as AuthorDTO;

            if (author == null)
            {
				MessageBox.Show(
				   "Nije odabran autor. Potrebno je odabrati autora iz liste.",
				   "Nije odabran autor",
				   MessageBoxButton.OK,
				   MessageBoxImage.Warning
			   );
				return;
			}

            SelectedAuthor = author;
            DialogResult = true;
		}
    }
}
