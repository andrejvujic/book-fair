using Core.DAO;

namespace ConsoleClient.Views.Utilities;
public static class SafeInput
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
}

