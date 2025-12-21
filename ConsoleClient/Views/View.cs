using Core.DAO;
using Core.Helpers;
using Core.Helpers.TablePrinter;
using Core.Serialization;

namespace ConsoleClient.Views;

public abstract class View<T> where T : class, ISerializable, IIdentifiable, new()
{
    protected readonly DAO<T> _dao;

    protected abstract string[] EntityProperties { get; }

    public View(DAO<T> dao)
    {
        _dao = dao; 
    }

    public virtual void Run()
    {
        while (true)
        {
            ShowOptions();

            int selectedOption = -1;

            Console.Write("> ");
            if (!int.TryParse(Console.ReadLine(), out selectedOption))
            {
                Console.Clear();
                Console.WriteLine("ERROR: Invalid input! Please select one of the available options.\n");
                continue;
            }

            Console.Clear();
            if (selectedOption == BACK)
                break;

            HandleAction(selectedOption);
        }
    }

    protected abstract void ShowOptions();
    protected abstract void HandleAction(int selectedOption);

    protected abstract T InputEntity();
    protected virtual void AddEntityAction() {
        Console.WriteLine($"[{_dao.Identifier}] Action 'ADD':\n");

        T entity = InputEntity();
        _dao.Add(entity);

        Console.Clear();
        Console.WriteLine("SUCCESS: Action 'ADD' successful.\n");
    }

    protected virtual void RemoveEntityAction() {
        Console.WriteLine($"[{_dao.Identifier}] Action 'REMOVE':\n");

        Console.Write("Enter ID: ");
        int id = -1;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Clear();
            Console.WriteLine($"ERROR: {_dao.Identifier} ID invalid.\n");
            return;
        }

        Console.Clear();

        if (_dao.Remove(id) == null)
        {
            Console.WriteLine($"ERROR: {_dao.Identifier} with ID '{id}' doesn't exist.\n");
            return;
        }

        Console.WriteLine("SUCCESS: Action 'REMOVE' successful.\n");
    }

    protected virtual void UpdateEntityAction() {
        Console.WriteLine($"[{_dao.Identifier}] Action 'UPDATE':\n");

        Console.Write("Enter ID: ");

        int id = -1;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Clear();
            Console.WriteLine($"ERROR: {_dao.Identifier} ID invalid.\n");
            return;
        }

        Console.Write("\n");

        T entity = InputEntity();
        entity.Id = id;

        Console.Clear();

        if (_dao.Update(entity) == null)
        {
            Console.WriteLine($"ERROR: {_dao.Identifier} with ID '{id}' doesn't exist.\n");
            return;
        }

        Console.WriteLine("SUCCESS: Action 'UPDATE' successful.\n");
    }

    protected virtual void ShowAllAction()
    {
        Console.WriteLine($"[{_dao.Identifier}] All entities:\n");

        Console.WriteLine($"Number of entities: {_dao.Count()}\n");

        new TablePrinter<T>(
            EntityProperties,
            _dao.GetAll()
        ).Print();
    }

    protected void ShowPaginatedAndSortedAction()
    {
        Console.WriteLine($"[{_dao.Identifier}] Showing with pagination and sorting:\n");

        Console.Write("Enter page size: ");
        int pageSize = -1;
        if (!int.TryParse(Console.ReadLine(), out pageSize))
        {
            Console.WriteLine($"ERROR: Invalid page size.\n");
            return;
        }

        if (pageSize < 1)
        {
            Console.WriteLine($"ERROR: Page size must be at least 1.\n");
            return;
        }

        int totalPageCount = (int)Math.Ceiling(_dao.Count() / (double)pageSize);

        Console.Write("Enter sorting criteria: ");
        string sortingCriteria = Console.ReadLine();

        Console.Write("Sorting direction:\n0 -> Ascending\n1 -> Descending\n");
        Console.Write("Enter sorting direction: ");

        int sortingDirection = -1;
        if (!int.TryParse(Console.ReadLine(), out sortingDirection))
        {
            Console.WriteLine($"ERROR: Invalid sorting direction.\n");
            return;
        }

        if (sortingDirection != 0 && sortingDirection != 1)
            Console.WriteLine("\nWARNING: Invalid sorting direction. Sorting direction will be set to ASCENDING.");

        Console.Write("\n");

        if (totalPageCount == 0)
        {
            Console.WriteLine("WARNING: Please add at least 1 entity before trying to show all entities.\n");
            return;
        }

        int pageNumber = 1;
        while (true)
        {
            List<T> currentPageEntities = _dao.Get(
                pageNumber,
                pageSize,
                sortingCriteria,
                sortingDirection == 1 ? SortingDirection.Descending : SortingDirection.Ascending
            );

            Console.WriteLine($"Page: {pageNumber} / {totalPageCount}");
            Console.WriteLine($"Showing: {currentPageEntities.Count()} / {_dao.Count()} entities\n");

            new TablePrinter<T>(
                EntityProperties,
                currentPageEntities
            ).Print();

            Console.Write("\n");

            Console.WriteLine("1 -> Previous page");
            Console.WriteLine("2 -> Next page");
            Console.WriteLine("0 -> Back");

            int selectedOption = -1;

            Console.Write("> ");
            if (!int.TryParse(Console.ReadLine(), out selectedOption))
            {
                Console.Clear();
                Console.WriteLine("ERROR: Invalid input! Please select one of the available options.\n");
                continue;
            }

            Console.Clear();
            if (selectedOption == BACK)
                break;

            switch (selectedOption)
            {
                case PREVIOUS_PAGE:
                    pageNumber = pageNumber - 1 < 1 ? totalPageCount : pageNumber - 1;
                    break;
                case NEXT_PAGE:
                    pageNumber = pageNumber + 1 > totalPageCount ? 1 : pageNumber + 1;
                    break;
            }

        }
    }

    protected const int BACK = 0;
    private const int PREVIOUS_PAGE = 1;
    private const int NEXT_PAGE = 2;
}
