using Core.Models;

namespace Core.DAO;

public class PurchaseDAO : DAO<Purchase>
{
    private static string _fileName = "Purchases.txt";
    public PurchaseDAO() : base(_fileName) { }

    public override Purchase? Update(Purchase entity)
    {
        Purchase? oldEntity = GetById(entity.Id);
        if (oldEntity == null) return null;

        oldEntity.Visitor = entity.Visitor;
        oldEntity.VisitorId = entity.VisitorId;
        oldEntity.Book = entity.Book;
        oldEntity.BookId = entity.BookId;
        oldEntity.Comment = entity.Comment;
        oldEntity.Rating = entity.Rating;
        oldEntity.Date = entity.Date;

        _storage.Save(_entities);
        return oldEntity;
    }
}
