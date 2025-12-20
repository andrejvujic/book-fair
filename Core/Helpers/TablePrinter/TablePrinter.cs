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


    public TablePrinter(string[] tableHeadRow, List<T> tableData) {
        TableColumnCount = tableHeadRow.Length;
        TableRowCount = tableData.Count;

        Table = new string[TableRowCount][];
        TableHeadRow = tableHeadRow;

        foreach (var element in tableData.Select((Value, Index) => new { Value, Index }))
        {
            string[] tableRow = element.Value.ToCSV();

            // Broj elementa svake vrste mora biti jednak broju kolona tabele
            // tj. broju kolona u zaglavlju tabele.
            if (tableRow.Length != tableHeadRow.Length)
                throw new Exception($"ERROR: Incompatible lengths. Expected number of columns is {tableHeadRow.Length}, got {tableRow.Length}.");

            Table[element.Index] = tableRow;
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
        PrintTableRowSperatator();

        for (int tableRowIndex = 0; tableRowIndex < TableRowCount; ++tableRowIndex)
        {
            PrintTableRow(Table[tableRowIndex]);
            if (tableRowIndex < TableRowCount - 1)
                PrintTableRowSperatator();
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
                Console.Write("│");
        }
        Console.Write("\n");
    }

    private void PrintTableRowSperatator()
    {
        for (int index = 0; index < TableColumnCount; ++index)
        {
            string element = new string('─', MaxWidthOfColumns[index]);
            Console.Write(element);
            if (index < TableColumnCount - 1)
                Console.Write("┼");
        }
        Console.Write("\n");
    }
}

