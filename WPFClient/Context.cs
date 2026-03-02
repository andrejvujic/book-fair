using ConsoleClient;
using Core.DAO;
using Core.Helpers.Relations;
using Core.Models;
using System.Collections.Generic;

namespace WPFClient
{
    public sealed class Context
    {
        private static Context? instance;

        public static Context Instance
        {
            get
            {
                if (instance == null) instance = new Context();
                return instance;
            }
        }

        public ManyToManyRelation<Visitor, Book> VisitorWishlistedBooks { get; }
        public ManyToManyRelation<Author, Book> AuthorWrittenBooks { get; }

        public RelationManager RelationManager { get; }

        public AddressDAO AddressDAO { get; }
        public AuthorDAO AuthorDAO { get; }
        public BookDAO BookDAO { get; }
        public PublisherDAO PublisherDAO { get; }
        public VisitorDAO VisitorDAO { get; }
        public PurchaseDAO PurchaseDAO { get; }

        private Context(
        )
        {
            VisitorWishlistedBooks = new ManyToManyRelation<Visitor, Book>("VisitorWishlistedBooks.txt");
            AuthorWrittenBooks = new ManyToManyRelation<Author, Book>("AuthorWrittenBooks.txt");

            AddressDAO = new AddressDAO();
            AuthorDAO = new AuthorDAO(AuthorWrittenBooks);
            BookDAO = new BookDAO(AuthorWrittenBooks, VisitorWishlistedBooks);
            VisitorDAO = new VisitorDAO(VisitorWishlistedBooks);
            PurchaseDAO = new PurchaseDAO();
            PublisherDAO = new PublisherDAO();

            RelationManager = new RelationManager(AuthorDAO, BookDAO, PublisherDAO, VisitorDAO, AddressDAO, PurchaseDAO);
        }

        public void RestoreRelations()
        {
            VisitorDAO.ForEach(visitor =>
            {
                visitor.WishlistedBooks = new List<Book>();
                VisitorWishlistedBooks.Connect(visitor, BookDAO.GetAll(), visitor.WishlistedBooks.Add);
            });

            AuthorDAO.ForEach(author =>
            {
                author.Books = new List<Book>();
                AuthorWrittenBooks.Connect(author, BookDAO.GetAll(), author.Books.Add);
            });

            BookDAO.ForEach(book =>
            {
                book.Authors = new List<Author>();
                AuthorWrittenBooks.Connect(book, AuthorDAO.GetAll(), book.Authors.Add);
            });

            BookDAO.ForEach(book =>
            {
                book.Wishlisters = new List<Visitor>();
                VisitorWishlistedBooks.Connect(book, VisitorDAO.GetAll(), book.Wishlisters.Add);
            });

            RelationManager.ConnectAll();
            AddressDAO.NotifyObservers();
            AuthorDAO.NotifyObservers();
            BookDAO.NotifyObservers();
            PublisherDAO.NotifyObservers();
            VisitorDAO.NotifyObservers();
            PublisherDAO.NotifyObservers();
        }
    }
}
