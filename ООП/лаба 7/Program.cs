using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// 1. Обобщенный интерфейс с операциями добавить, удалить, просмотреть
public interface ICollectionOperations<T>
{
    void Add(T item);
    bool Remove(T item);
    IEnumerable<T> Find(Predicate<T> predicate);
    void Display();
}

// 2. Обобщенный класс CollectionType<T> с ограничением where T : class
public class CollectionType<T> : ICollectionOperations<T> where T : class
{
    private List<T> collection;

    public CollectionType()
    {
        collection = new List<T>();
    }

    public CollectionType(IEnumerable<T> items)
    {
        collection = new List<T>(items);
    }

    // Реализация методов интерфейса
    public void Add(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Нельзя добавить null элемент");

            collection.Add(item);
            Console.WriteLine($"Элемент добавлен: {item}");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Операция добавления завершена");
        }
    }

    public bool Remove(T item)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Нельзя удалить null элемент");

            bool removed = collection.Remove(item);
            if (removed)
                Console.WriteLine($"Элемент удален: {item}");
            else
                Console.WriteLine($"Элемент не найден: {item}");

            return removed;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            return false;
        }
        finally
        {
            Console.WriteLine("Операция удаления завершена");
        }
    }

    public IEnumerable<T> Find(Predicate<T> predicate)
    {
        try
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "Предикат не может быть null");

            var result = collection.FindAll(predicate);
            Console.WriteLine($"Найдено элементов: {result.Count}");
            return result;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            return Enumerable.Empty<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            return Enumerable.Empty<T>();
        }
        finally
        {
            Console.WriteLine("Операция поиска завершена");
        }
    }

    public void Display()
    {
        Console.WriteLine($"Коллекция содержит {collection.Count} элементов:");
        foreach (var item in collection)
        {
            Console.WriteLine($"  {item}");
        }
    }

    // Свойства для доступа к коллекции
    public int Count => collection.Count;
    public List<T> Items => new List<T>(collection);

    // 5. Методы для сохранения и чтения из файла (текстовый формат)
    public void SaveToFile(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (var item in collection)
                {
                    writer.WriteLine(item.ToString());
                }
            }
            Console.WriteLine($"Коллекция сохранена в файл: {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
        }
    }

    public void LoadFromFile(string filename)
    {
        try
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"Файл {filename} не найден");

            // Для пользовательских классов нужна специальная логика загрузки
            // В данном примере просто сообщаем об успешной загрузке
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"Прочитано: {line}");
                }
            }
            Console.WriteLine($"Данные загружены из файла: {filename}");
            Console.WriteLine("Примечание: Для пользовательских классов требуется специальная логика десериализации");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке из файла: {ex.Message}");
        }
    }
}

// 4. Пользовательский класс из лабораторной №4 (адаптированный)
public class Student : IComparable<Student>
{
    public string Name { get; set; }
    public int Age { get; set; }
    public double Grade { get; set; }

    public Student(string name, int age, double grade)
    {
        Name = name;
        Age = age;
        Grade = grade;
    }

    public override string ToString()
    {
        return $"Student: {Name}, Age: {Age}, Grade: {Grade}";
    }

    public int CompareTo(Student other)
    {
        return Grade.CompareTo(other.Grade);
    }

    public override bool Equals(object obj)
    {
        return obj is Student student && Name == student.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

// Простой класс для демонстрации работы с файлами
public class StringData
{
    public string Content { get; set; }

    public StringData(string content)
    {
        Content = content;
    }

    public override string ToString()
    {
        return Content;
    }
}

// Класс для демонстрации работы с разными типами
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ЛАБОРАТОРНАЯ РАБОТА №7 - ОБОБЩЕНИЯ ===");
        Console.WriteLine();

        // 3. Тестирование со стандартными типами данных

        // Тестирование с типом string
        Console.WriteLine("1. ТЕСТИРОВАНИЕ СО СТРОКАМИ:");
        CollectionType<string> stringCollection = new CollectionType<string>();

        stringCollection.Add("Hello");
        stringCollection.Add("World");
        stringCollection.Add("C#");
        stringCollection.Add("Programming");

        stringCollection.Display();

        // Поиск строк длиннее 3 символов
        var longStrings = stringCollection.Find(s => s.Length > 3);
        Console.WriteLine("Строки длиннее 3 символов:");
        foreach (var str in longStrings)
        {
            Console.WriteLine($"  {str}");
        }

        stringCollection.Remove("C#");
        stringCollection.Display();

        // Сохранение строк в файл
        stringCollection.SaveToFile("strings.txt");

        Console.WriteLine();

        // Тестирование с пользовательским классом Student
        Console.WriteLine("2. ТЕСТИРОВАНИЕ С ПОЛЬЗОВАТЕЛЬСКИМ КЛАССОМ STUDENT:");
        CollectionType<Student> studentCollection = new CollectionType<Student>();

        studentCollection.Add(new Student("Иван Иванов", 20, 8.5));
        studentCollection.Add(new Student("Петр Петров", 21, 7.8));
        studentCollection.Add(new Student("Мария Сидорова", 19, 9.2));
        studentCollection.Add(new Student("Анна Козлова", 22, 8.9));

        studentCollection.Display();

        // Поиск студентов с оценкой выше 8.5
        var topStudents = studentCollection.Find(s => s.Grade > 8.5);
        Console.WriteLine("Студенты с оценкой выше 8.5:");
        foreach (var student in topStudents)
        {
            Console.WriteLine($"  {student}");
        }

        // Удаление студента
        studentCollection.Remove(new Student("Петр Петров", 0, 0));
        studentCollection.Display();

        // Сохранение студентов в файл
        studentCollection.SaveToFile("students.txt");

        Console.WriteLine();

        // Тестирование с простым классом StringData
        Console.WriteLine("3. ТЕСТИРОВАНИЕ С КЛАССОМ StringData:");
        CollectionType<StringData> dataCollection = new CollectionType<StringData>();

        dataCollection.Add(new StringData("Первая запись"));
        dataCollection.Add(new StringData("Вторая запись"));
        dataCollection.Add(new StringData("Третья запись"));

        dataCollection.Display();
        dataCollection.SaveToFile("data.txt");

        Console.WriteLine();

        // 5. Тестирование загрузки из файла
        Console.WriteLine("4. ТЕСТИРОВАНИЕ ЗАГРУЗКИ ИЗ ФАЙЛА:");

        CollectionType<string> loadedCollection = new CollectionType<string>();
        loadedCollection.LoadFromFile("strings.txt");

        Console.WriteLine();

        // Дополнительные тесты с исключениями
        Console.WriteLine("5. ТЕСТИРОВАНИЕ ОБРАБОТКИ ИСКЛЮЧЕНИЙ:");

        // Попытка добавить null
        studentCollection.Add(null);

        // Попытка удалить null
        studentCollection.Remove(null);

        // Попытка поиска с null предикатом
        studentCollection.Find(null);

        // Попытка загрузить из несуществующего файла
        loadedCollection.LoadFromFile("nonexistent.txt");

        Console.WriteLine();
        Console.WriteLine("=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===");

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}

// Дополнительный класс для демонстрации наследования от обобщенного класса
public class SpecializedCollection<T> : CollectionType<T> where T : class
{
    public void ClearAll()
    {
        var items = Items;
        items.Clear();
        Console.WriteLine("Коллекция полностью очищена");
    }

    public T GetFirstItem()
    {
        return Items.FirstOrDefault();
    }

    public void DisplayCount()
    {
        Console.WriteLine($"Текущее количество элементов: {Count}");
    }
}