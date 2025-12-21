using Core.Models;

namespace Core.DAO;

public class AddressDAO : DAO<Address>
{
	private static string _filename = "Addresses.txt";

    public override string Identifier { get { return "Adress"; } }

	public AddressDAO() : base(_filename)
	{
	}

    public override Address? Update(Address entity)
    {
        Address? oldEntity = _entities.Find(e => e.Id == entity.Id);
        if (oldEntity == null) return null;

        oldEntity.Id = entity.Id;
        oldEntity.Street = entity.Street;
        oldEntity.StreetNumber = entity.StreetNumber;
        oldEntity.Country = entity.Country; 
        oldEntity.City = entity.City;

        _storage.Save(_entities);
        return oldEntity;   
    }

    protected override Dictionary<string, IOrderedEnumerable<Address>> SortCriteriaMapping
    {
        get
        {
            return new Dictionary<string, IOrderedEnumerable<Address>>
            {
                { "Id", _entities.OrderBy(x => x.Id)},
                { "Street", _entities.OrderBy(x => x.Street)},
                { "StreetNumber", _entities.OrderBy(x => x.StreetNumber)},
                { "City", _entities.OrderBy(x => x.City)},
                { "Country", _entities.OrderBy(x => x.Country)},
            };

        }
    }

}


