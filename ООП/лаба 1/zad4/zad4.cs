using System;

class zad4
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        (int, string, char, string, ulong) myTuple = (42, "Ульяна", 'A', "Минск", 12345678901234567890);

        Console.WriteLine("Полный кортеж:");
        Console.WriteLine(myTuple);

        Console.WriteLine($"Выборочные элементы: {myTuple.Item1}, {myTuple.Item3}, {myTuple.Item4}");

        
        var (id, name, symbol, city, bigNumber) = myTuple;
        Console.WriteLine($"Распаковано: id={id}, name={name}, symbol={symbol}, city={city}, bigNumber={bigNumber}");

        var (_, _, ch, _, number) = myTuple;
        Console.WriteLine($"Распаковано выборочно: символ={ch}, число={number}");

        int id2;
        string name2;
        char symbol2;
        string city2;
        ulong bigNumber2;
        (id2, name2, symbol2, city2, bigNumber2) = myTuple;
        Console.WriteLine($"Распаковано в переменные: {id2}, {name2}, {symbol2}, {city2}, {bigNumber2}");

        var tuple1 = (1, "test", 'X', "data", 999UL);
        var tuple2 = (1, "test", 'X', "data", 999UL);
        var tuple3 = (2, "other", 'Y', "info", 1000UL);

        Console.WriteLine($"tuple1 == tuple2: {tuple1 == tuple2}"); 
        Console.WriteLine($"tuple1 == tuple3: {tuple1 == tuple3}");

        Console.WriteLine("\nВсе задания выполнены.");
        Console.ReadKey();
    }
}
