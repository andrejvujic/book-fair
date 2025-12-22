using ConsoleClient.Views.Utilities;
using Core.DAO;
using Core.Models;
using Microsoft.VisualBasic;

namespace ConsoleClient.Views;

public class AddressView : View<Address>
{
	public AddressView(AddressDAO addressDAO) : base(addressDAO) { }
    protected override void ShowOptions()
    {
		Console.WriteLine("[Address] Available options:\n");

		Console.WriteLine("1 -> Show all");
		Console.WriteLine("2 -> Add");
		Console.WriteLine("3 -> Update");
		Console.WriteLine("4 -> Remove");
		Console.WriteLine("5 -> Show (with pagination and sorting)");
		Console.WriteLine("0 -> Back\n");
	}

	protected override void HandleAction(int selectedOption)
	{
		switch (selectedOption)
		{
			case SHOW_ALL:
				ShowAllAction();
				break;
			case ADD:
				AddEntityAction();
				break;
			case UPDATE:
				UpdateEntityAction();
				break;
			case REMOVE:
				RemoveEntityAction();
				break;
			case SHOW_WITH_PAGINATION_AND_SORTING:
				ShowPaginatedAndSortedAction();
				break;
			default:
				Console.WriteLine($"ERROR: Unknown option '{selectedOption}'. Please select one of the available options.\n");
				break;
		}
	}

	protected override Address InputEntity()
	{
		Console.WriteLine("Enter street: ");
		string street = ConsoleUtilities.ReadStringRequired();

		Console.WriteLine("Enter street number: ");
		string streetNumber = ConsoleUtilities.ReadStringRequired();

		Console.WriteLine("Enter city: ");
		string city = ConsoleUtilities.ReadStringRequired();

		Console.WriteLine("Enter country: ");
		string country = ConsoleUtilities.ReadStringRequired();

		Address address = new Address(street, streetNumber, city, country);
	
		return address;
	}

	private const int SHOW_ALL = 1;
	private const int ADD = 2;
	private const int UPDATE = 3;
	private const int REMOVE = 4;
	private const int SHOW_WITH_PAGINATION_AND_SORTING = 5;
}