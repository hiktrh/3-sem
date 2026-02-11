using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.ObjectModel;

// 1. Класс Товар для 3-го варианта
public class Product : IComparable<Product>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }

    public Product(int id, string name, decimal price, int quantity, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
    }

    public int CompareTo(Product other)
    {
        if (other == null) return 1;
        return Price.CompareTo(other.Price);
    }

    public override string ToString()
    {
        return $"ID: {Id}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity}, Категория: {Category}";
    }
}

// 2. Коллекция с управлением (использует IOrderedDictionary)
public class ProductCollection : IOrderedDictionary
{
    private ArrayList products = new ArrayList();

    public object this[int index]
    {
        get => products[index];
        set => products[index] = value;
    }

    public object this[object key]
    {
        get
        {
            if (key is int id)
            {
                foreach (Product product in products)
                {
                    if (product.Id == id)
                        return product;
                }
            }
            return null;
        }
        set
        {
            // Не реализовано для упрощения
        }
    }

    public ICollection Keys
    {
        get
        {
            ArrayList keys = new ArrayList();
            foreach (Product product in products)
            {
                keys.Add(product.Id);
            }
            return keys;
        }
    }

    public ICollection Values => products;

    public int Count => products.Count;
    public bool IsReadOnly => false;
    public bool IsFixedSize => false;

    public IDictionaryEnumerator GetEnumerator() => new ProductEnumerator(products);

    IEnumerator IEnumerable.GetEnumerator() => products.GetEnumerator();

    public void Add(object key, object value)
    {
        if (value is Product product)
        {
            products.Add(product);
        }
    }

    public void Clear() => products.Clear();

    public bool Contains(object key)
    {
        if (key is int id)
        {
            foreach (Product product in products)
            {
                if (product.Id == id)
                    return true;
            }
        }
        return false;
    }

    public void Remove(object key)
    {
        if (key is int id)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (((Product)products[i]).Id == id)
                {
                    products.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void RemoveAt(int index) => products.RemoveAt(index);

    public void Insert(int index, object key, object value)
    {
        if (value is Product product && index >= 0 && index <= products.Count)
        {
            products.Insert(index, product);
        }
    }

    public void CopyTo(Array array, int index) => products.CopyTo(array, index);

    public object SyncRoot => products.SyncRoot;
    public bool IsSynchronized => products.IsSynchronized;

    // Методы для управления коллекцией
    public void AddProduct(Product product) => products.Add(product);

    public void RemoveProduct(int id) => Remove(id);

    public Product FindProductById(int id)
    {
        foreach (Product product in products)
        {
            if (product.Id == id)
                return product;
        }
        return null;
    }

    public List<Product> FindProductsByCategory(string category)
    {
        List<Product> result = new List<Product>();
        foreach (Product product in products)
        {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                result.Add(product);
        }
        return result;
    }

    public void DisplayAll()
    {
        Console.WriteLine("=== ВСЕ ТОВАРЫ ===");
        foreach (Product product in products)
        {
            Console.WriteLine(product);
        }
    }

    // Вложенный класс-перечислитель
    private class ProductEnumerator : IDictionaryEnumerator
    {
        private ArrayList products;
        private int position = -1;

        public ProductEnumerator(ArrayList products)
        {
            this.products = products;
        }

        public bool MoveNext()
        {
            position++;
            return position < products.Count;
        }

        public void Reset() => position = -1;

        public object Current
        {
            get
            {
                try
                {
                    return Entry;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                Product product = (Product)products[position];
                return new DictionaryEntry(product.Id, product);
            }
        }

        public object Key => ((Product)products[position]).Id;
        public object Value => products[position];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ЛАБОРАТОРНАЯ РАБОТА №9 - КОЛЛЕКЦИИ ===");
        Console.WriteLine("=== Вариант 3: Товар, IOrderedDictionary, ConcurrentBag ===");
        Console.WriteLine();

        // Часть 1: Работа с классом Product и коллекцией ProductCollection
        Console.WriteLine("=== ЧАСТЬ 1: РАБОТА С КЛАССОМ ТОВАР ===");

        ProductCollection productCollection = new ProductCollection();

        // Добавление товаров
        productCollection.AddProduct(new Product(1, "Ноутбук", 1500.99m, 10, "Электроника"));
        productCollection.AddProduct(new Product(2, "Смартфон", 899.50m, 25, "Электроника"));
        productCollection.AddProduct(new Product(3, "Книга 'C# для начинающих'", 45.99m, 50, "Книги"));
        productCollection.AddProduct(new Product(4, "Наушники", 129.99m, 30, "Электроника"));
        productCollection.AddProduct(new Product(5, "Кофеварка", 89.99m, 15, "Бытовая техника"));

        // Вывод всех товаров
        productCollection.DisplayAll();

        // Поиск товара по ID
        Console.WriteLine("\n=== ПОИСК ТОВАРА ПО ID ===");
        Product foundProduct = productCollection.FindProductById(2);
        Console.WriteLine(foundProduct != null ? $"Найден товар: {foundProduct}" : "Товар не найден");

        // Поиск товаров по категории
        Console.WriteLine("\n=== ТОВАРЫ КАТЕГОРИИ 'ЭЛЕКТРОНИКА' ===");
        var electronics = productCollection.FindProductsByCategory("Электроника");
        foreach (var product in electronics)
        {
            Console.WriteLine(product);
        }

        // Удаление товара
        Console.WriteLine("\n=== УДАЛЕНИЕ ТОВАРА С ID=4 ===");
        productCollection.RemoveProduct(4);
        productCollection.DisplayAll();

        // Часть 2: Универсальная коллекция ConcurrentBag<T>
        Console.WriteLine("\n\n=== ЧАСТЬ 2: УНИВЕРСАЛЬНАЯ КОЛЛЕКЦИЯ ConcurrentBag<T> ===");

        // 2a. Создание и заполнение ConcurrentBag<int>
        ConcurrentBag<int> numbersBag = new ConcurrentBag<int>();
        Console.WriteLine("\n2a. Заполнение ConcurrentBag<int>:");
        for (int i = 1; i <= 10; i++)
        {
            numbersBag.Add(i * 10);
        }

        Console.Write("Содержимое: ");
        foreach (var num in numbersBag)
        {
            Console.Write(num + " ");
        }

        // 2b. Удаление n последовательных элементов
        Console.WriteLine("\n\n2b. Удаление 3 элементов:");
        int removeCount = 3;
        for (int i = 0; i < removeCount; i++)
        {
            if (numbersBag.TryTake(out int removed))
            {
                Console.WriteLine($"Удален элемент: {removed}");
            }
        }

        Console.Write("Оставшееся содержимое: ");
        foreach (var num in numbersBag)
        {
            Console.Write(num + " ");
        }

        // 2c. Добавление других элементов
        Console.WriteLine("\n\n2c. Добавление новых элементов:");
        numbersBag.Add(110);
        numbersBag.Add(120);
        numbersBag.Add(130);

        // Используем ToArray() для упорядоченного вывода
        var sorted = numbersBag.ToArray();
        Array.Sort(sorted);
        Console.Write("Содержимое после добавления: ");
        foreach (var num in sorted)
        {
            Console.Write(num + " ");
        }

        // 2d. Создание второй коллекции (LinkedList<T>)
        Console.WriteLine("\n\n2d. Создание второй коллекции LinkedList<int>:");
        LinkedList<int> linkedList = new LinkedList<int>();

        // Заполняем из ConcurrentBag (порядок может быть произвольным)
        foreach (var num in numbersBag)
        {
            linkedList.AddLast(num);
        }

        // Сортируем для красивого вывода
        var sortedList = new List<int>(linkedList);
        sortedList.Sort();
        linkedList.Clear();
        foreach (var num in sortedList)
        {
            linkedList.AddLast(num);
        }

        // 2e. Вывод второй коллекции
        Console.WriteLine("\n2e. Содержимое LinkedList<int>:");
        Console.Write("Элементы: ");
        foreach (var num in linkedList)
        {
            Console.Write(num + " ");
        }

        // 2f. Поиск заданного значения
        Console.WriteLine("\n\n2f. Поиск значения 80:");
        bool found = linkedList.Contains(80);
        Console.WriteLine($"Значение 80 {(found ? "найдено" : "не найдено")}");

        // Поиск значения 120
        Console.WriteLine("Поиск значения 120:");
        found = linkedList.Contains(120);
        Console.WriteLine($"Значение 120 {(found ? "найдено" : "не найдено")}");

        // Часть 3: Наблюдаемая коллекция ObservableCollection<T>
        Console.WriteLine("\n\n=== ЧАСТЬ 3: НАБЛЮДАЕМАЯ КОЛЛЕКЦИЯ ObservableCollection<T> ===");

        ObservableCollection<Product> observableProducts = new ObservableCollection<Product>();

        // Регистрируем обработчик события
        observableProducts.CollectionChanged += OnProductsCollectionChanged;

        Console.WriteLine("\nДобавление элементов в ObservableCollection:");
        observableProducts.Add(new Product(6, "Планшет", 499.99m, 8, "Электроника"));
        observableProducts.Add(new Product(7, "Монитор", 299.99m, 12, "Электроника"));

        Console.WriteLine("\nУдаление элемента из ObservableCollection:");
        if (observableProducts.Count > 0)
        {
            observableProducts.RemoveAt(0);
        }

        Console.WriteLine("\nВставка элемента в ObservableCollection:");
        observableProducts.Insert(0, new Product(8, "Клавиатура", 59.99m, 20, "Аксессуары"));

        Console.WriteLine("\nИзменение элемента в ObservableCollection:");
        if (observableProducts.Count > 0)
        {
            observableProducts[0] = new Product(9, "Мышь", 29.99m, 25, "Аксессуары");
        }

        // Отписываемся от события
        observableProducts.CollectionChanged -= OnProductsCollectionChanged;

        // Дополнительно: демонстрация других коллекций из задания
        Console.WriteLine("\n\n=== ДОПОЛНИТЕЛЬНАЯ ДЕМОНСТРАЦИЯ ===");

        // Создание и работа с Stack<T>
        Console.WriteLine("\nРабота с Stack<Product>:");
        Stack<Product> productStack = new Stack<Product>();
        productStack.Push(new Product(10, "Принтер", 199.99m, 5, "Офисная техника"));
        productStack.Push(new Product(11, "Сканер", 149.99m, 7, "Офисная техника"));

        Console.WriteLine("Верхний элемент стека: " + productStack.Peek());
        Console.WriteLine("Извлечение из стека: " + productStack.Pop());
        Console.WriteLine("Осталось в стеке: " + productStack.Count);

        // Создание и работа с Queue<T>
        Console.WriteLine("\nРабота с Queue<Product>:");
        Queue<Product> productQueue = new Queue<Product>();
        productQueue.Enqueue(new Product(12, "Роутер", 79.99m, 18, "Сетевое оборудование"));
        productQueue.Enqueue(new Product(13, "МФУ", 249.99m, 4, "Офисная техника"));

        Console.WriteLine("Первый в очереди: " + productQueue.Peek());
        Console.WriteLine("Обработка из очереди: " + productQueue.Dequeue());
        Console.WriteLine("Осталось в очереди: " + productQueue.Count);

        // Демонстрация Hashtable
        DemonstrateHashtable();

        Console.WriteLine("\n\n=== ЛАБОРАТОРНАЯ РАБОТА ЗАВЕРШЕНА ===");
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    // Обработчик события изменения коллекции
    private static void OnProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                Console.WriteLine($"  Добавлено {e.NewItems.Count} элемент(ов)");
                foreach (Product product in e.NewItems)
                {
                    Console.WriteLine($"    - {product.Name}");
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                Console.WriteLine($"  Удалено {e.OldItems.Count} элемент(ов)");
                foreach (Product product in e.OldItems)
                {
                    Console.WriteLine($"    - {product.Name}");
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                Console.WriteLine($"  Заменен элемент на позиции {e.NewStartingIndex}");
                break;

            case NotifyCollectionChangedAction.Move:
                Console.WriteLine($"  Перемещен элемент с {e.OldStartingIndex} на {e.NewStartingIndex}");
                break;

            case NotifyCollectionChangedAction.Reset:
                Console.WriteLine("  Коллекция полностью очищена");
                break;
        }
    }

    // Дополнительный метод для демонстрации работы с Hashtable
    private static void DemonstrateHashtable()
    {
        Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ Hashtable ===");

        Hashtable productTable = new Hashtable();

        // Добавляем товары с использованием ID в качестве ключа
        productTable.Add(101, new Product(101, "Телевизор", 799.99m, 6, "Электроника"));
        productTable.Add(102, new Product(102, "Холодильник", 1299.99m, 3, "Бытовая техника"));
        productTable.Add(103, new Product(103, "Стиральная машина", 599.99m, 4, "Бытовая техника"));

        // Поиск по ключу
        Console.WriteLine("\nПоиск товара с ID 102:");
        if (productTable.ContainsKey(102))
        {
            Console.WriteLine(productTable[102]);
        }

        // Перебор всех элементов
        Console.WriteLine("\nВсе товары в Hashtable:");
        foreach (DictionaryEntry entry in productTable)
        {
            Console.WriteLine($"Ключ: {entry.Key}, Значение: {entry.Value}");
        }
    }
}