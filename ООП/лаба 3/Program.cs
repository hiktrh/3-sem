using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Lab3_SetClass.Set;

namespace Lab3_SetClass
{
    // Вложенный класс Production
    public class Production
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }


        public Production(int id, string name)
        {
            Id = id;
            OrganizationName = name;
        }

        public override string ToString()
        {
            return $"Production: ID={Id}, Organization={OrganizationName}";
        }
    }

    // Вложенный класс Developer
    public class Developer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }

        public Developer(int id, string name, string department)
        {
            Id = id;
            FullName = name;
            Department = department;
        }

        public override string ToString()
        {
            return $"Developer: ID={Id}, Name={FullName}, Department={Department}";
        }
    }

    // Основной класс Set
    public class Set
    {
        private List<int> elements;

        // Вложенные объекты
        public Production ProductionInfo { get; set; }
        public Developer DeveloperInfo { get; set; }

        public Set()
        {
            elements = new List<int>();
            // Инициализация вложенных объектов
            ProductionInfo = new Production(1, "BSTU Corporation");
            DeveloperInfo = new Developer(101, "Ivan Ivanov", "Software Development");
        }

        public Set(params int[] items)
        {
            elements = new List<int>();
            foreach (int item in items)
            {
                if (!elements.Contains(item))
                    elements.Add(item);
            }
            // Инициализация вложенных объектов
            ProductionInfo = new Production(1, "BSTU Corporation");
            DeveloperInfo = new Developer(101, "Ivan Ivanov", "Software Development");
        }

        // Добавление элемента
        public void Add(int item)
        {
            if (!elements.Contains(item))
                elements.Add(item);
        }

        // Удаление элемента
        public void Remove(int item)
        {
            elements.Remove(item);
        }

        // Мощность множества
        public int Count => elements.Count;

        // Элементы множества
        public int[] Elements => elements.ToArray();

        // Перегрузка операции + (добавить элемент) - set + item
        public static Set operator +(Set set, int item)
        {
            Set result = new Set();
            result.elements.AddRange(set.elements);
            if (!result.elements.Contains(item))
                result.elements.Add(item);
            return result;
        }

        

        // Перегрузка операции + (объединение множеств) - set1 + set2
        public static Set operator +(Set set1, Set set2)
        {
            Set result = new Set();
            foreach (int item in set1.elements)
                result.Add(item);
            foreach (int item in set2.elements)
                result.Add(item);
            return result;
        }

        // Перегрузка операции * (пересечение множеств)
        public static Set operator *(Set set1, Set set2)
        {
            Set result = new Set();
            foreach (int item in set1.elements)
            {
                if (set2.elements.Contains(item))
                    result.Add(item);
            }
            return result;
        }

        public class MyInt
        {
            public int Value { get; set; }

            public MyInt(int value)
            {
                Value = value;
            }

            // Перегрузка оператора + для сложения двух MyInt
            public static MyInt operator +(MyInt a, MyInt b)
            {
                return new MyInt(a.Value + b.Value);
            }

            public override string ToString()
            {
                return $"MyInt: {Value}";
            }
        }

        public class Complex
        {
            public double Real { get; set; }
            public double Imag { get; set; }

            public Complex(double real, double imag)
            {
                Real = real;
                Imag = imag;
            }

            // Перегрузка оператора + для сложения двух комплексных чисел
            public static Complex operator +(Complex a, Complex b)
            {
                return new Complex(a.Real + b.Real, a.Imag + b.Imag);
            }

            public override string ToString()
            {
                return $"Complex: {Real} + {Imag}j";
            }
        }
    }

    // Явное приведение к int (мощность множества)
    public static explicit operator int(Set set)
        {
            return set.elements.Count;
        }

        // Перегрузка операции false (проверка размера массива)
        public static bool operator false(Set set)
        {   
            return set.elements.Count < 1 || set.elements.Count > 10;
        }

        // Перегрузка операции true (для парности с false)
        public static bool operator true(Set set)
        {
            return set.elements.Count >= 1 && set.elements.Count <= 10;
        }

        public override string ToString()
        {
            return $"Set: [{string.Join(", ", elements)}]";
        }
    }

    // Статический класс StatisticOperation
    public static class StatisticOperation
    {
        // Сумма всех элементов
        public static int Sum(Set set)
        {
            return set.Elements.Sum();
        }

        // Разница между максимальным и минимальным
        public static int Difference(Set set)
        {
            if (set.Count == 0) return 0;
            return set.Elements.Max() - set.Elements.Min();
        }

        // Подсчет количества элементов
        public static int CountElements(Set set)
        {
            return set.Count;
        }

        // Метод расширения для string: Добавление запятой после каждого слова
        public static string AddCommaAfterEachWord(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(", ", words) + (str.EndsWith(" ") ? " " : "");
        }

        // Метод расширения для Set: Удаление повторяющихся элементов
        public static Set RemoveDuplicates(this Set set)
        {
            // В классе Set дубликаты уже не допускаются, но можно использовать для очистки
            Set result = new Set();
            foreach (int item in set.Elements)
                result.Add(item);
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ КЛАССА SET ===");
            Console.WriteLine();

            // Создание множеств
            Set set1 = new Set(1, 2, 3, 4, 5);
            Set set2 = new Set(4, 5, 6, 7, 8);

            Console.WriteLine($"Множество 1: {set1}");
            Console.WriteLine($"Множество 2: {set2}");

            // Тестирование перегруженных операций
            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ПЕРЕГРУЖЕННЫХ ОПЕРАЦИЙ ===");

            // Операция + (добавление элемента)
            Set set3 = set1 + 10;
            Console.WriteLine($"set1 + 10 = {set3}");

        MyInt a = new MyInt(5);
        MyInt b = new MyInt(4);
        MyInt c = a + b;
        Console.WriteLine(c);

        Complex z1 = new Complex(1, 5.5);
        Complex z2 = new Complex(3, 2.7);
        Complex sum = z1 + z2;
        Console.WriteLine(sum);

        // Операция + (объединение множеств)
        Set unionSet = set1 + set2;
            Console.WriteLine($"set1 + set2 (объединение) = {unionSet}");

            // Операция * (пересечение множеств)
            Set intersectionSet = set1 * set2;
            Console.WriteLine($"set1 * set2 (пересечение) = {intersectionSet}");

            // Явное приведение к int (мощность)
            int power = (int)set1;
            Console.WriteLine($"Мощность set1: {power}");

            // Операции true/false
            Set smallSet = new Set(1, 2, 3);
            Set largeSet = new Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Console.WriteLine("\nПроверка диапазона размера:");
            Console.WriteLine($"smallSet (3 элемента): {smallSet.Count} - {(smallSet ? "в диапазоне" : "вне диапазона")}");
            Console.WriteLine($"largeSet (12 элементов): {largeSet.Count} - {(largeSet ? "в диапазоне" : "вне диапазона")}");

            // Тестирование статических методов
            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ СТАТИЧЕСКИХ МЕТОДОВ ===");
            Console.WriteLine($"Сумма элементов set1: {StatisticOperation.Sum(set1)}");
            Console.WriteLine($"Разница max-min set1: {StatisticOperation.Difference(set1)}");
            Console.WriteLine($"Количество элементов set1: {StatisticOperation.CountElements(set1)}");

            // Тестирование методов расширения
            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ МЕТОДОВ РАСШИРЕНИЯ ===");

            string testString = "Hello world this is test";
            string resultString = testString.AddCommaAfterEachWord();
            Console.WriteLine($"Исходная строка: {testString}");
            Console.WriteLine($"С запятыми: {resultString}");

            Set testSet = new Set(1, 2, 2, 3, 3, 4); // Дубликаты будут удалены автоматически
            Set cleanedSet = testSet.RemoveDuplicates();
            Console.WriteLine($"Исходное множество: {testSet}");
            Console.WriteLine($"После 'очистки': {cleanedSet}");
            Console.WriteLine($"Пустое ли множество: {testSet.IsEmpty()}");

            // Вывод информации о вложенных объектах
            Console.WriteLine("\n=== ИНФОРМАЦИЯ О ВЛОЖЕННЫХ ОБЪЕКТАХ ===");
            Console.WriteLine(set1.ProductionInfo);
            Console.WriteLine(set1.DeveloperInfo);

            // Дополнительные тесты
            Console.WriteLine("\n=== ДОПОЛНИТЕЛЬНЫЕ ТЕСТЫ ===");

            // Создание пустого множества
            Set emptySet = new Set();
            Console.WriteLine($"Пустое множество: {emptySet}");
            Console.WriteLine($"Пустое ли: {emptySet.IsEmpty()}");

            // Последовательные операции
            Set complexSet = (set1 + 100) * (set2 + 100);
            Console.WriteLine($"Сложная операция: (set1 + 100) * (set2 + 100) = {complexSet}");

            Console.WriteLine("\nТестирование завершено!");
            Console.ReadKey();
        }
    }
}