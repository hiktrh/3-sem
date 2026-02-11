using System;

class zad3
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int[,] matrix = {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };

        Console.WriteLine("Двумерный массив (матрица):");
        for (int i = 0; i < matrix.GetLength(0); i++) // строки
        {
            for (int j = 0; j < matrix.GetLength(1); j++) // столбцы
            {
                Console.Write($"{matrix[i, j],4}");
            }
            Console.WriteLine();
        }

        string[] words = { "яблоко", "банан", "вишня", "груша" };
        Console.WriteLine("\nОдномерный массив строк:");
        foreach (string word in words)
            Console.WriteLine(word);

        Console.WriteLine($"Длина массива: {words.Length}");

        Console.Write("\nВведите позицию для замены (0–3): ");
        int pos = int.Parse(Console.ReadLine());

        Console.Write("Введите новое значение: ");
        string newValue = Console.ReadLine();

        if (pos >= 0 && pos < words.Length)
        {
            words[pos] = newValue;
            Console.WriteLine("Обновлённый массив:");
            foreach (string word in words)
                Console.WriteLine(word);
        }
        else
        {
            Console.WriteLine("Недопустимая позиция.");
        }

        double[][] jaggedArray = new double[3][];
        jaggedArray[0] = new double[2];
        jaggedArray[1] = new double[3];
        jaggedArray[2] = new double[4];

        Console.WriteLine("\nВведите значения для ступенчатого массива:");
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            Console.WriteLine($"Строка {i + 1}:");
            for (int j = 0; j < jaggedArray[i].Length; j++)
            {
                Console.Write($"[{i}][{j}] = ");
                jaggedArray[i][j] = double.Parse(Console.ReadLine());
            }
        }

        Console.WriteLine("\nСтупенчатый массив:");
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            foreach (double val in jaggedArray[i])
                Console.Write($"{val,6:F2}");
            Console.WriteLine();
        }

        var autoArray = new[] { 1, 2, 3, 4 };
        var autoString = "Пример строки";

        Console.WriteLine("\nНеявно типизированный массив:");
        foreach (var item in autoArray)
            Console.Write(item + " ");
        Console.WriteLine($"\nНеявно типизированная строка: {autoString}");

        Console.WriteLine("\nВсе задания выполнены.");
        Console.ReadKey();
    }
}
