using Core.Serialization;
namespace Core.Helpers.TablePrinter;

public class TablePrinter<T>
    where T : class, ISerializable
{
    // Sadrzaj tabele u obliku matrice.
    private string[][] Table { get; }
    // Sadrzi nazive kolona tabele (nazivi polja modela koji se prikazuje).
    private string[] TableHeadRow { get; }

    private int TableRowCount { get; }
    private int TableColumnCount { get; }

    // Sadrzi sirinu maksimalnog elementa svake kolone. Neophodno
    // je pamtiti makismalnu sirinu svake tabele zbog poravnanja tabele.
    private int[] MaxWidthOfColumns { get; }


    public TablePrinter(string[] tableColumnNames, List<T> tableData) {
        TableColumnCount = tableColumnNames.Length;
        TableRowCount = tableData.Count;

        Table = new string[TableRowCount][];
        TableHeadRow = tableColumnNames;

        foreach (var element in tableData.Select((Value, Index) => new { Value, Index }))
        {
            string[] currentTableRow = element.Value.ToCSV();

            // Broj elementa svake vrste mora biti jednak broju kolona tabele
            // tj. broju kolona u zaglavlju tabele.
            if (currentTableRow.Length != TableHeadRow.Length)
                throw new Exception($"ERROR: Incompatible table row sizes. Expected number of columns is {TableHeadRow.Length}, got {currentTableRow.Length}.");

            Table[element.Index] = currentTableRow;
        }

        MaxWidthOfColumns = new int[TableColumnCount];
        for (int tableColumnIndex = 0; tableColumnIndex < TableColumnCount; ++tableColumnIndex)
            MaxWidthOfColumns[tableColumnIndex] = TableHeadRow[tableColumnIndex].Length;

        for (int tableRowIndex = 0; tableRowIndex < tableData.Count; ++tableRowIndex)
            for (int tableColumnIndex = 0; tableColumnIndex < TableColumnCount; ++tableColumnIndex)
            {
                int currentElementLength = Table[tableRowIndex][tableColumnIndex].Length;

                if (currentElementLength > MaxWidthOfColumns[tableColumnIndex])
                    MaxWidthOfColumns[tableColumnIndex] = currentElementLength;
            }
    }

    public void Print()
    {
        PrintTableRow(TableHeadRow);
        PrintTableRowSeparator();

        for (int tableRowIndex = 0; tableRowIndex < TableRowCount; ++tableRowIndex)
        {
            PrintTableRow(Table[tableRowIndex]);

            if (tableRowIndex < TableRowCount - 1)
                PrintTableRowSeparator();
            else
                Console.Write("\n");
        }
    }

    private void PrintTableRow(string[] row) {
        for (int index = 0; index < TableColumnCount; ++index)
        {
            Console.Write(
                row[index].PadLeft(
                    MaxWidthOfColumns[index]
                )
            );

            if (index < TableColumnCount - 1)
                Console.Write(VERTICAL_SEPARATOR);
        }

        Console.Write("\n");
    }

    private void PrintTableRowSeparator()
    {
        for (int index = 0; index < TableColumnCount; ++index)
        {
            string element = new string(HORIZONTAL_SEPARATOR, MaxWidthOfColumns[index]);
            Console.Write(element);

            if (index < TableColumnCount - 1)
                Console.Write(CROSS_SEPARATOR);
        }
        Console.Write("\n");
    }
    
    const char VERTICAL_SEPARATOR = '│';
    const char HORIZONTAL_SEPARATOR = '─';
    const char CROSS_SEPARATOR = '┼';
}

