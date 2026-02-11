using System;
using System.Collections.Generic;
using System.Linq;

// Делегаты для событий
public delegate void IncreaseEventHandler(double amount);
public delegate void PenaltyEventHandler(double amount);

// Класс Директор с событиями
public class Director
{
    public event IncreaseEventHandler OnIncrease;
    public event PenaltyEventHandler OnPenalty;

    public string Name { get; set; }

    public Director(string name)
    {
        Name = name;
    }

    // Метод для вызова события повышения
    public void RaiseIncrease(double amount)
    {
        Console.WriteLine($"\nДиректор {Name} объявляет повышение на {amount}%");
        OnIncrease?.Invoke(amount);
    }

    // Метод для вызова события штрафа
    public void RaisePenalty(double amount)
    {
        Console.WriteLine($"\nДиректор {Name} объявляет штраф в размере {amount}%");
        OnPenalty?.Invoke(amount);
    }
}

// Базовый класс для всех, кто может получать зарплату
public abstract class SalaryRecipient
{
    public string Name { get; set; }
    public double Salary { get; set; }

    public SalaryRecipient(string name, double salary)
    {
        Name = name;
        Salary = salary;
    }

    public virtual void DisplayStatus()
    {
        Console.WriteLine($"{GetType().Name} {Name}: Зарплата = {Salary:C}");
    }
}

// Класс Токарь
public class Turner : SalaryRecipient
{
    public int Experience { get; set; }

    public Turner(string name, double salary, int experience)
        : base(name, salary)
    {
        Experience = experience;
    }

    // Обработчик повышения
    public void HandleIncrease(double amount)
    {
        double increase = Salary * (amount / 100);
        Salary += increase;
        Console.WriteLine($"Токарь {Name}: Получил повышение! +{increase:C} (новый оклад: {Salary:C})");
    }

    // Обработчик штрафа
    public void HandlePenalty(double amount)
    {
        double penalty = Salary * (amount / 100);
        Salary = Math.Max(0, Salary - penalty); // Зарплата не может быть отрицательной
        Console.WriteLine($"Токарь {Name}: Получил штраф! -{penalty:C} (новый оклад: {Salary:C})");
    }

    public override void DisplayStatus()
    {
        Console.WriteLine($"{GetType().Name} {Name}: Зарплата = {Salary:C}, Опыт = {Experience} лет");
    }
}

// Класс Студент-заочник
public class CorrespondenceStudent : SalaryRecipient
{
    public string University { get; set; }
    public int Course { get; set; }

    public CorrespondenceStudent(string name, double salary, string university, int course)
        : base(name, salary)
    {
        University = university;
        Course = course;
    }

    // Обработчик повышения (студенты получают меньшее повышение)
    public void HandleIncrease(double amount)
    {
        double studentIncrease = Salary * (amount / 100 * 0.5); // Студенты получают 50% от обычного повышения
        Salary += studentIncrease;
        Console.WriteLine($"Студент {Name}: Получил повышение! +{studentIncrease:C} (новый оклад: {Salary:C})");
    }

    // Обработчик штрафа (студенты получают больший штраф)
    public void HandlePenalty(double amount)
    {
        double studentPenalty = Salary * (amount / 100 * 1.5); // Студенты получают 150% от обычного штрафа
        Salary = Math.Max(0, Salary - studentPenalty);
        Console.WriteLine($"Студент {Name}: Получил штраф! -{studentPenalty:C} (новый оклад: {Salary:C})");
    }

    public override void DisplayStatus()
    {
        Console.WriteLine($"{GetType().Name} {Name}: Зарплата = {Salary:C}, Университет = {University}, Курс = {Course}");
    }
}

// Часть 2: Обработка строк с использованием стандартных делегатов
public static class StringProcessor
{
    // Методы обработки строк
    public static string RemovePunctuation(string input)
    {
        var result = new string(input.Where(c => !char.IsPunctuation(c)).ToArray());
        Console.WriteLine($"Удалены знаки препинания: '{input}' -> '{result}'");
        return result;
    }

    public static string ToUpperCase(string input)
    {
        var result = input.ToUpper();
        Console.WriteLine($"Преобразовано в верхний регистр: '{input}' -> '{result}'");
        return result;
    }

    public static string RemoveExtraSpaces(string input)
    {
        var result = string.Join(" ", input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        Console.WriteLine($"Удалены лишние пробелы: '{input}' -> '{result}'");
        return result;
    }

    public static string AddStars(string input)
    {
        var result = $"*** {input} ***";
        Console.WriteLine($"Добавлены звезды: '{input}' -> '{result}'");
        return result;
    }

    public static string ReverseWords(string input)
    {
        var words = input.Split(' ');
        Array.Reverse(words);
        var result = string.Join(" ", words);
        Console.WriteLine($"Слова в обратном порядке: '{input}' -> '{result}'");
        return result;
    }

    // Метод для последовательной обработки строки
    public static string ProcessString(string input, List<Func<string, string>> processors)
    {
        string result = input;
        foreach (var processor in processors)
        {
            result = processor(result);
        }
        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ЛАБОРАТОРНАЯ РАБОТА №8 - ДЕЛЕГАТЫ, СОБЫТИЯ И ЛЯМБДА-ВЫРАЖЕНИЯ ===");
        Console.WriteLine("=== Вариант 3: Директор с событиями Повысить и Штраф ===\n");

        // Создаем директора
        Director director = new Director("Иван Петрович");

        // Создаем объекты токарей и студентов
        Turner turner1 = new Turner("Алексей", 50000, 5);
        Turner turner2 = new Turner("Сергей", 45000, 3);
        Turner turner3 = new Turner("Дмитрий", 60000, 10);

        CorrespondenceStudent student1 = new CorrespondenceStudent("Мария", 25000, "БГТУ", 3);
        CorrespondenceStudent student2 = new CorrespondenceStudent("Анна", 20000, "БНТУ", 2);
        CorrespondenceStudent student3 = new CorrespondenceStudent("Игорь", 22000, "БГУ", 4);

        // Подписываем объекты на события произвольным образом
        Console.WriteLine("=== ПОДПИСКА НА СОБЫТИЯ ===");

        // Токарь 1 подписан на оба события
        director.OnIncrease += turner1.HandleIncrease;
        director.OnPenalty += turner1.HandlePenalty;
        Console.WriteLine($"Токарь {turner1.Name} подписан на оба события");

        // Токарь 2 подписан только на повышение
        director.OnIncrease += turner2.HandleIncrease;
        Console.WriteLine($"Токарь {turner2.Name} подписан только на повышение");

        // Токарь 3 подписан только на штраф
        director.OnPenalty += turner3.HandlePenalty;
        Console.WriteLine($"Токарь {turner3.Name} подписан только на штраф");

        // Студент 1 подписан на оба события
        director.OnIncrease += student1.HandleIncrease;
        director.OnPenalty += student1.HandlePenalty;
        Console.WriteLine($"Студент {student1.Name} подписан на оба события");

        // Студент 2 подписан только на повышение
        director.OnIncrease += student2.HandleIncrease;
        Console.WriteLine($"Студент {student2.Name} подписан только на повышение");

        // Студент 3 не подписан ни на что
        Console.WriteLine($"Студент {student3.Name} не подписан ни на какие события");

        // Отображаем начальное состояние
        Console.WriteLine("\n=== НАЧАЛЬНОЕ СОСТОЯНИЕ ===");
        turner1.DisplayStatus();
        turner2.DisplayStatus();
        turner3.DisplayStatus();
        student1.DisplayStatus();
        student2.DisplayStatus();
        student3.DisplayStatus();

        // Вызываем события
        Console.WriteLine("\n=== ВЫЗОВ СОБЫТИЙ ===");

        // Повышение на 10%
        director.RaiseIncrease(10);

        // Штраф 5%
        director.RaisePenalty(5);

        // Еще одно повышение на 15%
        director.RaiseIncrease(15);

        // Отображаем конечное состояние
        Console.WriteLine("\n=== КОНЕЧНОЕ СОСТОЯНИЕ ===");
        turner1.DisplayStatus();
        turner2.DisplayStatus();
        turner3.DisplayStatus();
        student1.DisplayStatus();
        student2.DisplayStatus();
        student3.DisplayStatus();

        // Часть 2: Обработка строк с использованием стандартных делегатов
        Console.WriteLine("\n\n=== ЧАСТЬ 2: ОБРАБОТКА СТРОК С ИСПОЛЬЗОВАНИЕМ СТАНДАРТНЫХ ДЕЛЕГАТОВ ===");

        // Использование Action для вывода
        Action<string> printAction = (str) => Console.WriteLine($"Результат: '{str}'");

        // Использование Predicate для проверки
        Predicate<string> isLongEnough = (str) => str.Length > 5;

        // Использование Func для обработки
        List<Func<string, string>> processors = new List<Func<string, string>>
        {
            StringProcessor.RemovePunctuation,
            StringProcessor.ToUpperCase,
            StringProcessor.RemoveExtraSpaces,
            StringProcessor.AddStars,
            StringProcessor.ReverseWords
        };

        // Тестовые строки
        string[] testStrings = {
            "Привет,  как    дела?",
            "C# -   лучший    язык программирования!",
            "Делегаты,  события...   и лямбда-выражения"
        };

        foreach (string testString in testStrings)
        {
            Console.WriteLine($"\n--- Обработка строки: '{testString}' ---");

            // Проверка длины строки с помощью Predicate
            if (isLongEnough(testString))
            {
                Console.WriteLine("Строка достаточно длинная для обработки");

                // Обработка строки с помощью цепочки Func
                string result = StringProcessor.ProcessString(testString, processors);

                // Вывод результата с помощью Action
                printAction(result);
            }
            else
            {
                Console.WriteLine("Строка слишком короткая для обработки");
            }
        }

        // Дополнительный пример с лямбда-выражениями
        Console.WriteLine("\n--- Дополнительные примеры с лямбда-выражениями ---");

        // Лямбда с несколькими параметрами
        Func<double, double, double> calculateBonus = (salary, percentage) => salary * (percentage / 100);

        double bonus = calculateBonus(50000, 10);
        Console.WriteLine($"Бонус 10% от 50000: {bonus:C}");

        // Лямбда с действием
        Action<string, double> announceBonus = (name, amount) =>
            Console.WriteLine($"{name} получает бонус: {amount:C}");

        announceBonus("Алексей", bonus);

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}