using System;

class zad1
{
    static void Main()
    {

        bool isActive = true;
        byte b = 255;
        sbyte sb = -128;
        short s = -32768;
        ushort us = 65535;
        int i = 2147483647;
        uint ui = 4294967295;
        long l = -9223372036854775808;
        ulong ul = 18446744073709551615;
        float f = 3.14f;
        double d = 2.718281828;
        decimal dec = 1000.50m;
        char ch = 'A';
        string str = "Привет";
        object obj = 42;

        Console.WriteLine("Введите целое число:");
        int userInt = int.Parse(Console.ReadLine());
        Console.WriteLine("Вы ввели: " + userInt);

        Console.WriteLine($"bool: {isActive}, byte: {b}, sbyte: {sb}, short: {s}, ushort: {us}");
        Console.WriteLine($"int: {i}, uint: {ui}, long: {l}, ulong: {ul}");
        Console.WriteLine($"float: {f}, double: {d}, decimal: {dec}");
        Console.WriteLine($"char: {ch}, string: {str}, object: {obj}");

        
        // Неявное 
        int x = 10;
        long y = x; 
        float z = x; 
        double dd = f; 
        uint uu = b; 
        decimal dec2 = x; 

        // Явное 
        double d1 = 9.99;
        int i1 = (int)d1; 
        float f1 = (float)d1; 
        byte b1 = (byte)i1; 
        char c1 = (char)b1; 
        short s1 = (short)d1; 

        string numStr = "123";
        int converted = Convert.ToInt32(numStr);
        Console.WriteLine("Convert.ToInt32(\"123\") = " + converted);

     
        int value = 42;
        object boxed = value; // упаковка
        int unboxed = (int)boxed; // распаковка
        Console.WriteLine($"Упаковано: {boxed}, Распаковано: {unboxed}");

        var message = "Это строка";
        Console.WriteLine(message);

       
        int? nullableInt = null;
        Console.WriteLine("nullableInt.HasValue = " + nullableInt.HasValue);
        nullableInt = 5;
        Console.WriteLine("nullableInt.Value = " + nullableInt.Value);

        // Ошибка при попытке изменить тип var
        var number = 10;
        Console.WriteLine("var number = 10; // тип int, нельзя присвоить строку позже");

        Console.WriteLine("Все задания выполнены.");
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();

    }
}
