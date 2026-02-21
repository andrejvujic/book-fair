using Core.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFClient.DTO;

namespace WPFClient.Views.Author.WrittenBooks
{
    public partial class AuthorWrittenBooksTab : UserControl, IObserver
    {
        public ObservableCollection<BookDTO> WrittenBooks { get; set; } = new ObservableCollection<BookDTO>();

        public static readonly DependencyProperty AuthorProperty =
                   DependencyProperty.Register(
                       nameof(Author),
                       typeof(Core.Models.Author),
                       typeof(AuthorWrittenBooksTab),
                       new PropertyMetadata(null, OnDependencyPropertyChanged)
                   );

        // TODO: Koristiti AuthorDTO
        public Core.Models.Author? Author
        {
            get => (Core.Models.Author?)GetValue(AuthorProperty);
            set => SetValue(AuthorProperty, value);
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AuthorWrittenBooksTab control = (AuthorWrittenBooksTab)d;
            control.Update();
        }

        public void Update()
        {
            if (Author == null) return;
            Render(Author.Books);
        }

        public void Render(List<Core.Models.Book> books)
        {
            WrittenBooks.Clear();
            foreach (var b in books)
                WrittenBooks.Add(new BookDTO(b));
        }

        public AuthorWrittenBooksTab()
        {
            InitializeComponent();

            // TODO: Zamijeniti sa AuthorController.Subscribe(this)
            App.Context.AuthorDAO.Subject.Subscribe(this);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Author == null) return;
            AuthorWrittenBooksAdd dialog = new AuthorWrittenBooksAdd(Author);
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
        }
    }
}
