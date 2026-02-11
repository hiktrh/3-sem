using System;

class zad5
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Введите целые числа через пробел:");
        string[] input = Console.ReadLine().Split(' ');
        int[] numbers = Array.ConvertAll(input, int.Parse);

        Console.Write("Введите строку: ");
        string text = Console.ReadLine();

        (int max, int min, int sum, char firstChar) Analyze(int[] arr, string str)
        {
            int max = int.MinValue;
            int min = int.MaxValue;
            int sum = 0;

            foreach (int num in arr)
            {
                if (num > max) max = num;
                if (num < min) min = num;
                sum += num;
            }

            char firstChar = string.IsNullOrEmpty(str) ? '?' : str[0];
            return (max, min, sum, firstChar);
        }

        var result = Analyze(numbers, text);

        Console.WriteLine($"\nРезультаты анализа:");
        Console.WriteLine($"Максимум: {result.max}");
        Console.WriteLine($"Минимум: {result.min}");
        Console.WriteLine($"Сумма: {result.sum}");
        Console.WriteLine($"Первая буква строки: {result.firstChar}");

        Console.WriteLine("\nВсе задания выполнены.");
        Console.ReadKey();
    }
}
