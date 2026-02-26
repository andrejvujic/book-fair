using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Core.DAO;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Localization;
using WPFClient.Views.About;
using WPFClient.Views.Author;
using WPFClient.Views.Book;
using WPFClient.Views.Publisher;
using WPFClient.Views.Visitor;

namespace WPFClient
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly DispatcherTimer? _timer;
        private VisitorController VisitorController { get; }
        private BookController BookController { get; }
        private AuthorController AuthorController { get; }
        private PublisherController PublisherController { get; }
        private PurchaseController PurchaseController { get; }

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
        public string CurrentTabIdentifier
        {
            get
            {
                int SelectedIndex = EntityTabs.SelectedIndex > -1 ? EntityTabs.SelectedIndex : 0;
                switch (SelectedIndex)
                {
                    case VISITORS_TAB_INDEX:
                        return TranslationSource.Instance["Visitors"];
                    case AUTHORS_TAB_INDEX:
                        return TranslationSource.Instance["Authors"];
                    case BOOKS_TAB_INDEX:
                        return TranslationSource.Instance["Books"];
                    default:
                        return string.Empty;
                }
            }
        }

        public string CurrentDateTimeText { get => DateTime.Now.ToString("f", TranslationSource.Instance.CurrentCulture); }
        public MainWindow()
        {
            SetWindowSize();
            InitializeComponent();
            DataContext = this;

            VisitorController = new VisitorController(
                App.Context.VisitorDAO,
                App.Context.AddressDAO,
                App.Context.BookDAO,
                App.Context.PurchaseDAO
            );
            BookController = new BookController(
                App.Context.BookDAO,
                App.Context.VisitorDAO,
                App.Context.AuthorDAO,
                App.Context.PurchaseDAO
            );

            AuthorController = new AuthorController(
                App.Context.AuthorDAO,
                App.Context.AddressDAO,
                App.Context.BookDAO
            );

            PublisherController = new PublisherController(App.Context.PublisherDAO);

            PurchaseController = new PurchaseController(App.Context.PurchaseDAO);


            // Tajmer koji se pokreće svake sekunde i osvježava
            // prikaz trenutnog datuma i vremena na statusnoj traci.
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => OnPropertyChanged(nameof(CurrentDateTimeText));
            _timer.Start();
        }

        private void SetWindowSize()
        {
            const double factor = 0.75;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            Width = screenWidth * factor;
            Height = screenHeight * factor;
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
                case AUTHORS_TAB_INDEX:
                    AddAuthor addAuthorDialog = new AddAuthor(AuthorController);
                    addAuthorDialog.Owner = this;
                    addAuthorDialog.ShowDialog();
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
						    TranslationSource.Instance["EditBookText"],
							TranslationSource.Instance["EditBookCaption"],
							MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }
                    UpdateBook updateBookDialog = new UpdateBook(targetBook, BookController);
                    updateBookDialog.Owner = this;
                    updateBookDialog.ShowDialog();
                    break;
                case AUTHORS_TAB_INDEX:
					AuthorDTO? targetAuthor = AuthorTab.SelectedAuthor;
					if (targetAuthor == null)
					{
						MessageBox.Show(
					        TranslationSource.Instance["EditAuthorText"],
                            TranslationSource.Instance["EditAuthorCaption"],
							MessageBoxButton.OK,
							MessageBoxImage.Warning
						);
						return;
					}
					UpdateAuthor updateAuthorDialog = new UpdateAuthor(targetAuthor, AuthorController);
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
                            "Nije odabran posjetilac za brisanje. Potrebno je odabrati posjetioca iz liste.",
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
                            VisitorController.Delete(targetVisitor);
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;

                case BOOKS_TAB_INDEX:
                  BookDTO? targetBook = BookTab.SelectedBook;
                    if (targetBook == null)
                    {
                        MessageBox.Show(
                            "Nije odabrana knjiga za brisanje. Potrebno je odabrati knjigu iz liste.",
                            "Nije odabrana knjiga",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
                    }

                    MessageBoxResult bookResult = MessageBox.Show(
                        "Da li ste sigurni da želite da obrišete knjigu?",
                        "Brisanje knjige",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    switch (bookResult)
                    {
                        case MessageBoxResult.Yes:
							BookController.Delete(targetBook);
							break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;

				case AUTHORS_TAB_INDEX:
					AuthorDTO? targetAuthor = AuthorTab.SelectedAuthor;
					if (targetAuthor == null)
					{
						MessageBox.Show(
                            TranslationSource.Instance["NoSelectedAuthorText"],
							TranslationSource.Instance["NoSelectedAuthorCaption"],
							MessageBoxButton.OK,
							MessageBoxImage.Warning
						);
						return;
					}

					bool hasActiveBook = BookController.GetWhere(b => !b.IsDeleted && b.Authors.Any(a => a.Id == targetAuthor.Id)).Count > 0;
					if (hasActiveBook)
					{
						MessageBox.Show(
							TranslationSource.Instance["DeleteAuthorNotPossibleBookText"],
							TranslationSource.Instance["DeleteAuthorNotPossibleBookCaption"],
							MessageBoxButton.OK,
							MessageBoxImage.Warning
						);
						return;
					}

                    bool isBoss = PublisherController.GetWhere(p => !p.IsDeleted && p.Boss != null && p.Boss.Id == targetAuthor.Id).Count > 0;
                    if (isBoss) {
                        MessageBox.Show(
							TranslationSource.Instance["DeleteAuthorNotPossiblePublisherText"],
							TranslationSource.Instance["DeleteAuthorNotPossibleBookCaption"],
							MessageBoxButton.OK,
                            MessageBoxImage.Warning
                        );
                        return;
					}

					MessageBoxResult authorResult = MessageBox.Show(
						TranslationSource.Instance["DeleteAuthorQ"],
						TranslationSource.Instance["DeleteAuthorQCaption"],
						MessageBoxButton.YesNo,
						MessageBoxImage.Question
					);

					switch (authorResult)
					{
						case MessageBoxResult.Yes:
							AuthorController.Delete(targetAuthor);
							break;
						case MessageBoxResult.No:
							break;
						default:
							break;
					}
					break;
			}
        }
        
        // Poziva se svaki put kada se promijeni odabrani tab.
        private void EntityTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EntityTabs.SelectedIndex == SelectedTabIndex) return;

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
            OnPropertyChanged(nameof(CurrentTabIdentifier));
        }

        // Opcije menija za otvaranje tabova:
        private void VisitorsMenuItem_Click(object sender, RoutedEventArgs e) => EntityTabs.SelectedIndex = VISITORS_TAB_INDEX;
		private void AuthorsMenuItem_Click(object sender, RoutedEventArgs e) => EntityTabs.SelectedIndex = AUTHORS_TAB_INDEX;
		private void BooksMenuItem_Click(object sender, RoutedEventArgs e) => EntityTabs.SelectedIndex = BOOKS_TAB_INDEX;
        
        private void PublishersMenuItem_Click(object sender, RoutedEventArgs e) => OpenPublishersWindow();
        private void OpenPublishersWindow()
        {
            PublisherWindow window = new PublisherWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e) => OpenHelpWindow();
        private void OpenHelpWindow()
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void SerbianMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App)?.ChangeLanguage(LanguageConfiguration.Serbian);
            OnPropertyChanged(nameof(CurrentDateTimeText));
            OnPropertyChanged(nameof(CurrentTabIdentifier));
        }

        private void EnglishMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App)?.ChangeLanguage(LanguageConfiguration.English);
            OnPropertyChanged(nameof(CurrentDateTimeText));
            OnPropertyChanged(nameof(CurrentTabIdentifier));
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool isCtrlKeyDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.N))
            {
                PerformAddEntity();
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.E))
            {
                PerformEditEntity();
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.D))
            {
                PerformDeleteEntity();
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.D1))
            {
                EntityTabs.SelectedIndex = VISITORS_TAB_INDEX;
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.D2))
            {
                EntityTabs.SelectedIndex = AUTHORS_TAB_INDEX;
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.D3))
            {
                EntityTabs.SelectedIndex = BOOKS_TAB_INDEX;
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.D4))
            {
                OpenPublishersWindow();
                return;
            }

            if (isCtrlKeyDown && Keyboard.IsKeyDown(Key.H))
            {
                OpenHelpWindow();
                return;
            }
        }

        const int VISITORS_TAB_INDEX = 0;
        const int AUTHORS_TAB_INDEX = 1;
        const int BOOKS_TAB_INDEX = 2;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
