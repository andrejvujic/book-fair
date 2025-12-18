// See https://aka.ms/new-console-template for more information
using Core.Models;
using Core.Helpers.Relations;
using Core.DAO;

ManyToManyRelation<Visitor, Book> visitorPurchasedBooks =
    new ManyToManyRelation<Visitor, Book>("VisitorPurchasedBooks.txt");

ManyToManyRelation<Visitor, Book> visitorWhislistedBooks =
    new ManyToManyRelation<Visitor, Book>("VisitorWishlistedBooks.txt");

PublisherDAO publisherDAO = new PublisherDAO();
VisitorDAO visitorDAO = new VisitorDAO(visitorPurchasedBooks, visitorWhislistedBooks);
PurchaseDAO purchaseDAO = new PurchaseDAO();

visitorDAO.ForEach(visitor => visitorPurchasedBooks.Connect(visitor, visitor.PurchasedBooks, visitor.PurchasedBooks.Add));
visitorDAO.ForEach(visitor => visitorWhislistedBooks.Connect(visitor, visitor.WishlistedBooks, visitor.WishlistedBooks.Add));