using Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPFClient.Controllers;
using WPFClient.DTO;
using WPFClient.Views.Book;
using WPFClient.Views.Visitor;

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer? _timer;
        private VisitorController VisitorController { get; set; }
        private BookController BookController { get; set; }

        private String CurrentTabIdentifier
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

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show(
                        "Dodavanje novih entiteta nije implementirano za ovaj tab.",
                        "Funkcionalnost nije implementirana",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    break;
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
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

                    Trace.WriteLine($"Deleting visitor: {targetVisitor.Id}");

                    targetVisitor.IsDeleted = true;
                    VisitorController.Update(targetVisitor.ToModel());
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

        private void EntityTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Poziva se svaki put kada se promijeni odabrani tab.
            RefreshCurrentTabIndicator();
        }

        // Opcije menija za otvaranje tabova:
        private void VisitorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EntityTabs.SelectedIndex = VISITORS_TAB_INDEX;
        }

        const int VISITORS_TAB_INDEX = 0;
        const int AUTHORS_TAB_INDEX = 1;
        const int BOOKS_TAB_INDEX = 2;

        private void RefreshCurrentTabIndicator()
        {
            CurrentTabIndicator.Text = $"Sajam Knjiga — {CurrentTabIdentifier}";
        }

        private void RefreshCurrentDateAndTime()
        {
            CurrentDateAndTime.Text = DateTime.Now.ToString("HH:mm — d. M. yyyy.");
        }
    }

}
