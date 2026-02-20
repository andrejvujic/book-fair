using Core.DAO;

namespace ConsoleClient.Views.Utilities;
public static class ConsoleUtilities
{
    public static int ReadIntRequired() {
        int value = -1;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("ERROR: Please enter a valid number.\n");
                Console.Write("Try again: ");
                continue;
            }

            return value;
        }
    }

    public static int ReadIntOptional(int defaultValue)
    {
        int value = -1;
        if (!int.TryParse(Console.ReadLine(), out value))
            return defaultValue;

        return value;
    }


    public static List<int> ReadIntListRequired(int minLength = 1, int? maxLength = null, char separator = ' ')
    {
        List<int> values = new List<int>();

        while (true)
        {
            string input = Console.ReadLine() ?? string.Empty;
            string[] rawValues = input.Split(separator);
            if (ParseIntList(rawValues, out values, minLength, maxLength, separator)) break;
        }

        return values;
    }

    private static bool ParseIntList(string[] rawValues, out List<int> values, int minLength = 1, int? maxLength = null, char separator = ' ')
    {
        values = new List<int>();

        if (maxLength != null && maxLength < rawValues.Length)
            return false;

        if (rawValues.Length < minLength) return false;

        for (int index = 0; index < rawValues.Length; index++)
        {
            int value;

            if (!int.TryParse(rawValues[index], out value))
            {
                values.Clear();
                Console.WriteLine("ERROR: Please enter a valid list of numbers.\n");
                Console.Write("Try again: ");
                return false;
            }

            values.Add(value);
        }

        return true;
    }

    public static string ReadStringRequired()
    {
        while (true)
        {
            string value = Console.ReadLine() ?? string.Empty;
            if (value.Length > 0) return value;
            Console.WriteLine("ERROR: Please enter a valid string.\n");
            Console.Write("Try again: ");
        }
    }

    public static string ReadStringOptional(string defaultValue)
    {
        string value = Console.ReadLine() ?? string.Empty;
        if (value.Length > 0) return value;
        return defaultValue;
    }

	public static double ReadDoubleRequired()
	{
		double value = -1;
		while (true)
		{
			if (!double.TryParse(Console.ReadLine(), out value))
			{
				Console.WriteLine("ERROR: Please enter a valid number.\n");
				Console.Write("Try again: ");
				continue;
			}

			return value;
		}
	}

	public static double ReadDoubleOptional(double defaultValue)
	{
		double value = -1;
		if (!double.TryParse(Console.ReadLine(), out value))
			return defaultValue;

		return value;
	}
}

