using Core.Models;

namespace Core.DAO;

public class PurchaseDAO : DAO<Purchase>
{
    private static string _fileName = "Purchases.txt";
    public override string Identifier { get { return "Purchase"; } }
    public override string[] EntityProperties
    {
        get
        {
            return new string[] {
                "ID", "Visitor ID", "Book ID", "Date", "Rating", "Comment"
            };
        }
    }

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

    protected override Dictionary<string, IOrderedEnumerable<Purchase>> SortCriteriaMapping
    {
        get
        {
            return new Dictionary<string, IOrderedEnumerable<Purchase>>
            {
                { "Id", _entities.OrderBy(x => x.Id) },
                { "Date", _entities.OrderBy(x => x.Date) },
                { "Rating", _entities.OrderBy(x => x.Rating) },
                { "VisitorId", _entities.OrderBy(x => x.VisitorId) },
                { "BookId", _entities.OrderBy(x => x.BookId) }
            };
        }
    }
}
