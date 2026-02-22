using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.DAO;
using WPFClient.DTO;
using Core.Observer;

namespace WPFClient.Views.Author
{
	public partial class AuthorTab : UserControl, IObserver, INotifyPropertyChanged
	{
		public ObservableCollection<AuthorDTO> Authors { get; set; } = new ObservableCollection<AuthorDTO>();

		private AuthorDAO AuthorDAO { get; set; }

		private string rawSearchQuery = string.Empty;
		public string RawSearchQuery
		{
			get => rawSearchQuery;
			set
			{
				if (rawSearchQuery == value) return;
				rawSearchQuery = value;
				OnPropertyChanged(nameof(RawSearchQuery));
				Update();
			}
		}

		public AuthorDTO? SelectedAuthor => AuthorsDataGrid.SelectedItem as AuthorDTO;

		public AuthorTab()
		{
			InitializeComponent();

			AuthorDAO = App.Context.AuthorDAO;
			AuthorDAO.Subject.Subscribe(this);

			DataContext = this;
			Update();
		}

		// IObserver implementation - called when DAO notifies observers
		public void Update()
		{
			var all = AuthorDAO.GetAll();

			var filtered = string.IsNullOrWhiteSpace(RawSearchQuery)
				? all
				: all.Where(a =>
					(a.FirstName ?? string.Empty).Contains(RawSearchQuery, StringComparison.InvariantCultureIgnoreCase) ||
					(a.LastName ?? string.Empty).Contains(RawSearchQuery, StringComparison.InvariantCultureIgnoreCase) ||
					(a.Email ?? string.Empty).Contains(RawSearchQuery, StringComparison.InvariantCultureIgnoreCase)
				  ).ToList();

			Authors.Clear();
			foreach (var a in filtered)
				Authors.Add(new AuthorDTO(a));
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var model = new Core.Models.Author();
			var dlg = new UpdateAuthor(model);
			dlg.Owner = Window.GetWindow(this);
			dlg.ShowDialog();

			// If user filled required fields (minimal check), add to DAO.
			if (!string.IsNullOrWhiteSpace(model.FirstName) || !string.IsNullOrWhiteSpace(model.LastName))
			{
				AuthorDAO.Add(model);
				// AuthorDAO will notify observers -> Update() will refresh the grid.
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			var sel = SelectedAuthor;
			if (sel == null)
			{
				MessageBox.Show("Nije odabran autor za izmjenu.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			var model = sel.ToModel();
			var dlg = new UpdateAuthor(model);
			dlg.Owner = Window.GetWindow(this);
			dlg.ShowDialog();

			// After dialog close, persist changes
			AuthorDAO.Update(model);
			// AuthorDAO notifies observers -> Update()
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			var sel = SelectedAuthor;
			if (sel == null)
			{
				MessageBox.Show("Nije odabran autor za brisanje.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			var result = MessageBox.Show($"Da li ste sigurni da želite obrisati autora '{sel.FirstName} {sel.LastName}'?", "Brisanje", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				AuthorDAO.Remove(sel.Id);
				// AuthorDAO notifies observers -> Update()
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}