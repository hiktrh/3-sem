using System;
using System.Text;

class zad2
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string str1 = "Привет";
        string str2 = "Привет";
        string str3 = "привет";

        Console.WriteLine($"str1 == str2: {str1 == str2}"); 
        Console.WriteLine($"str1 == str3: {str1 == str3}"); 
        Console.WriteLine($"str1.Equals(str3, StringComparison.OrdinalIgnoreCase): {str1.Equals(str3, StringComparison.OrdinalIgnoreCase)}"); 

        string s1 = "Добро";
        string s2 = "пожаловать";
        string s3 = "в C#";

        // Сцепление
        string joined = string.Concat(s1, " ", s2, " ", s3);
        Console.WriteLine("Сцепление: " + joined);

        string copy = string.Copy(s2);
        Console.WriteLine("Копия строки: " + copy);

        // Подстрока
        string sub = joined.Substring(7, 10);
        Console.WriteLine("Подстрока: " + sub);

        // Разделение
        string[] words = joined.Split(' ');
        Console.WriteLine("Разделение на слова:");
        foreach (var word in words)
            Console.WriteLine(word);

        // Вставка подстроки
        string inserted = joined.Insert(6, "[вставка]");
        Console.WriteLine("После вставки: " + inserted);

        // Удаление подстроки
        string removed = inserted.Remove(6, 9); 
        Console.WriteLine("После удаления: " + removed);

        // Интерполяция
        string name = "Павел";
        int age = 30;
        string interpolated = $"Имя: {name}, Возраст: {age}";
        Console.WriteLine("Интерполяция: " + interpolated);

        string emptyStr = "";
        string nullStr = null;

        Console.WriteLine($"emptyStr is null or empty: {string.IsNullOrEmpty(emptyStr)}");
        Console.WriteLine($"nullStr is null or empty: {string.IsNullOrEmpty(nullStr)}");

        // Что ещё можно сделать
        Console.WriteLine("emptyStr.Length: " + emptyStr.Length); // 0
        // Console.WriteLine("nullStr.Length: " + nullStr.Length); //  вызовет исключение

        if (!string.IsNullOrEmpty(nullStr))
        {
            Console.WriteLine("Длина строки: " + nullStr.Length);
        }
        else
        {
            Console.WriteLine("Строка null или пустая");
        }

        // StringBuilder
        StringBuilder sb = new StringBuilder("Пример строки");
        sb.Remove(0, 7); // удаляем "Пример "
        sb.Insert(0, ">>"); // вставка в начало
        sb.Append(" <<"); // добавление в конец

        Console.WriteLine("StringBuilder результат: " + sb.ToString());

        Console.WriteLine("Все задания выполнены.");
        Console.ReadKey();
    }
}
