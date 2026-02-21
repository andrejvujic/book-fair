using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Book;
using WPFClient.Views.Publisher;
using WPFClient.Views.Visitor;
using WPFClient.Views.Author;

namespace WPFClient
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly DispatcherTimer? _timer;
        private VisitorController VisitorController { get; }
        private BookController BookController { get; }

        private string rawSearchQuery = string.Empty;
        public string RawSearchQuery
        {
            get => rawSearchQuery;
            set
            {
                if (string.Equals(rawSearchQuery, value)) return;
                rawSearchQuery = value;
                OnPropertyChanged(nameof(RawSearchQuery));
            }
        }

        public string VisitorsTabSearchQuery { get => SelectedTabIndex == VISITORS_TAB_INDEX ? RawSearchQuery : string.Empty; }
        public string BooksTabSearchQuery { get => SelectedTabIndex == BOOKS_TAB_INDEX ? RawSearchQuery : string.Empty; }
        public string AuthorsTabSearchQuery { get => SelectedTabIndex == AUTHORS_TAB_INDEX ? RawSearchQuery : string.Empty; }

        private int SelectedTabIndex { get; set; }
        private string CurrentTabIdentifier
        {
            get
            {
                int SelectedIndex = EntityTabs.SelectedIndex > -1 ? EntityTabs.SelectedIndex : 0;
                switch (SelectedIndex)
                {
                    case VISITORS_TAB_INDEX:
                        return "Posjetioci";
                    case AUTHORS_TAB_INDEX:
                        return "Autori";
                    case BOOKS_TAB_INDEX:
                        return "Knjige";
                    default:
                        return string.Empty;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            VisitorController = new VisitorController(App.Context.VisitorDAO, App.Context.AddressDAO);
            BookController = new BookController(App.Context.BookDAO);

            RefreshCurrentTabIndicator(); // Postavlja tekst indikatora trenutnog taba za statusnu traku.


            // Tajmer koji se pokreće svake sekunde i osvježava
            // prikaz trenutnog datuma i vremena na statusnoj traci.
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => RefreshCurrentDateAndTime();
            _timer.Start();
            RefreshCurrentDateAndTime(); // Inicijalno postavljanje trenutnog datuma i vremena prilikom pokretanja aplikacije.
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void NewMenuItem_Click(object sender, RoutedEventArgs e) => PerformAddEntity();
        private void PerformAddEntity()
        {
            // Odabrana je opcija za dodavanje entiteta, potrebno je otvoriti odgovarajući
            // dijalog za traženi entitet na osnovu trenutno odabranog taba.

            switch (EntityTabs.SelectedIndex)
            {
                case VISITORS_TAB_INDEX:
                    AddVisitor addVisitorDialog = new AddVisitor(VisitorController);
                    addVisitorDialog.Owner = this; // Postavlja ovaj prozor kao vlasnika dijaloga - omogućava modalno ponašanje i centriranje dijaloga u odnosu na ovaj prozor.
                    addVisitorDialog.ShowDialog();
                    break;
                case BOOKS_TAB_INDEX:
                    AddBook addBookDialog = new AddBook(BookController);
                    addBookDialog.Owner = this;
                    addBookDialog.ShowDialog();
                    break;
                default:
                    MessageBox.Show(
                        "Dodavanje novih entiteta nije implementirano za ovaj tab.",
                        "Funkcionalnost nije implementirana",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    break;
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e) => PerformEditEntity();
        private void PerformEditEntity()
        {
            switch (EntityTabs.SelectedIndex)
            {
                case VISITORS_TAB_INDEX:
                    VisitorDTO? targetVisitor = VisitorTab.SelectedVisitor;
                    if (targetVisitor == null)
                    {
                        MessageBox.Show(
                            "Nije odabran posjetilac za izmjenu. Potrebno je odabrati pojsetioca iz liste.",
                            "Nije odabran posjetilac",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }

                    UpdateVisitor updateVisitorDialog = new UpdateVisitor(targetVisitor, VisitorController);
                    updateVisitorDialog.Owner = this; // Postavlja ovaj prozor kao vlasnika dijaloga - omogućava modalno ponašanje i centriranje dijaloga u odnosu na ovaj prozor.
                    updateVisitorDialog.ShowDialog();
                    break;
                case BOOKS_TAB_INDEX:
                    BookDTO? targetBook = BookTab.SelectedBook;
                    if (targetBook == null)
                    {
                        MessageBox.Show(
                            "Nije odabrana knjiga za izmjenu. Potrebno je odabrati knjigu iz liste.",
                            "Nije odabrana knjiga",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }
                    UpdateBook updateBookDialog = new UpdateBook(targetBook, BookController);
                    updateBookDialog.Owner = this;
                    updateBookDialog.ShowDialog();
                    break;
                default:
                    // TODO: Ovde treba koristiti AuthorDTO i treba kupiti oznacenog
                    // autora iz liste autora...
                    Core.Models.Author targetAuthor = App.Context.AuthorDAO.GetById(0) ?? new Core.Models.Author();
                    UpdateAuthor updateAuthorDialog = new UpdateAuthor(targetAuthor);
                    updateAuthorDialog.Owner = this;
                    updateAuthorDialog.ShowDialog();
                    break;
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e) => PerformDeleteEntity();
        private void PerformDeleteEntity()
        {
            switch (EntityTabs.SelectedIndex)
            {
                case VISITORS_TAB_INDEX:
                    VisitorDTO? targetVisitor = VisitorTab.SelectedVisitor;
                    if (targetVisitor == null)
                    {
                        MessageBox.Show(
                            "Nije odabran posjetilac za brisanje. Potrebno je odabrati pojsetioca iz liste.",
                            "Nije odabran posjetilac",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }

                    MessageBoxResult result = MessageBox.Show(
                        "Da li ste sigurni da želite da obrišete posjetioca?",
                        "Brisanje posjetioca",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            targetVisitor.IsDeleted = true;
                            VisitorController.Update(targetVisitor.ToModel());
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }


                    break;
                case BOOKS_TAB_INDEX:
                    MessageBox.Show(
                        "Brisanje entiteta nije implementirano za ovaj tab.",
                        "Funkcionalnost nije implementirana",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    break;
                default:
                    MessageBox.Show(
                        "Brisanje entiteta nije implementirano za ovaj tab.",
                        "Funkcionalnost nije implementirana",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    break;
            }
        }
        
        // Poziva se svaki put kada se promijeni odabrani tab.
        private void EntityTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RawSearchQuery = string.Empty;

            switch (SelectedTabIndex)
            {
                case VISITORS_TAB_INDEX:
                    OnPropertyChanged(nameof(VisitorsTabSearchQuery));
                    break;
                case AUTHORS_TAB_INDEX:
                    OnPropertyChanged(nameof(AuthorsTabSearchQuery));
                    break;
                case BOOKS_TAB_INDEX:
                    OnPropertyChanged(nameof(BooksTabSearchQuery));
                    break;
            }

            SelectedTabIndex = EntityTabs.SelectedIndex > -1 ? EntityTabs.SelectedIndex : 0;
            OnPropertyChanged(nameof(SelectedTabIndex));

            RefreshCurrentTabIndicator();
        }

        // Opcije menija za otvaranje tabova:
        private void VisitorsMenuItem_Click(object sender, RoutedEventArgs e) => EntityTabs.SelectedIndex = VISITORS_TAB_INDEX;

        private void PublishersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PublisherWindow window = new PublisherWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void UpdateSearchQuery()
        {
            SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            switch (SelectedTabIndex)
            {
                case VISITORS_TAB_INDEX:
                    OnPropertyChanged(nameof(VisitorsTabSearchQuery));
                    break;
                case AUTHORS_TAB_INDEX:
                    OnPropertyChanged(nameof(AuthorsTabSearchQuery));
                    break;
                case BOOKS_TAB_INDEX:
                    OnPropertyChanged(nameof(BooksTabSearchQuery));
                    break;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e) =>
            UpdateSearchQuery();

        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            RawSearchQuery = string.Empty;
            SearchTextBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            UpdateSearchQuery();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox target = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(target.Text)) return;
            UpdateSearchQuery();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateSearchQuery();
                return;
            }
        }

        const int VISITORS_TAB_INDEX = 0;
        const int AUTHORS_TAB_INDEX = 1;
        const int BOOKS_TAB_INDEX = 2;

        private void RefreshCurrentTabIndicator() =>
            CurrentTabIndicator.Text = $"Sajam Knjiga — {CurrentTabIdentifier}";

        private void RefreshCurrentDateAndTime() =>
            CurrentDateAndTime.Text = DateTime.Now.ToString("HH:mm — d. M. yyyy.");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
