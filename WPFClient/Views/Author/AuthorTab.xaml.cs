using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core.DAO;
using Core.Helpers.Sorting;
using Core.Models;
using Core.Observer;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Visitor;

namespace WPFClient.Views.Author
{
	public partial class AuthorTab : UserControl, IObserver
	{
		public ObservableCollection<AuthorDTO> Authors { get; set; } = new ObservableCollection<AuthorDTO>();
		public AuthorController AuthorController { get;}
		public AuthorDTO? SelectedAuthor => AuthorsDataGrid.SelectedItem as AuthorDTO;

		public static readonly DependencyProperty RawSearchQueryProperty =
		   DependencyProperty.Register(
			   nameof(RawSearchQuery),
			   typeof(string),
			   typeof(AuthorTab),
			   new PropertyMetadata(null, OnSearchQueryPropertyChanged)
		   );

		public string? RawSearchQuery
		{
			get => (string?)GetValue(RawSearchQueryProperty);
			set => SetValue(RawSearchQueryProperty, value);
		}

		private static void OnSearchQueryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AuthorTab control = (AuthorTab)d;
			control.Update();
		}
		public AuthorTab()
		{
			InitializeComponent();
			AuthorController = new AuthorController(App.Context.AuthorDAO, App.Context.AddressDAO);
			AuthorController.Subscribe(this);
			Update();
		}

		public void Update()
		{
			Render(
				AuthorController
				.PerformSerach(RawSearchQuery ?? string.Empty)
				.PerformSorting()
				.GetWithPagination()
			);
		}

		private void Render(List<Core.Models.Author> authors)
		{
			Authors.Clear();
			foreach (Core.Models.Author a in authors)
				Authors.Add(new AuthorDTO(a));
		}

		private void AuthorsDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
		{
			string criteria = e.Column.SortMemberPath ?? string.Empty;
			Direction direction = e.Column.SortDirection == ListSortDirection.Ascending ? Direction.Descending : Direction.Ascending;

			if (!AuthorController.SetSortingParameters(criteria, direction)) return;

			Render(
				AuthorController
				.PerformSorting()
				.GetWithPagination()
			);
		}
		private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (!AuthorController.GoToPreviousPage()) return;
			Render(AuthorController.GetWithPagination());
		}
		private void NextPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (!AuthorController.GoToNextPage()) return;
			Render(AuthorController.GetWithPagination());
		}
	}
}