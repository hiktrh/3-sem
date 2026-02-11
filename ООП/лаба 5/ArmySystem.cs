using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
// Убрал Newtonsoft.Json так как он не установлен

namespace Lab5_ArmyHierarchy
{
    // 3. КЛАСС-КОНТЕЙНЕР ДЛЯ АРМИИ (требование №3)
    public class ArmyContainer
    {
        private List<ArmyUnit> units;

        public ArmyContainer()
        {
            units = new List<ArmyUnit>();
        }

        // Метод get для получения единицы по индексу
        public ArmyUnit Get(int index)
        {
            if (index >= 0 && index < units.Count)
                return units[index];
            throw new IndexOutOfRangeException($"Индекс {index} вне диапазона [0, {units.Count - 1}]");
        }

        // Метод set для установки единицы по индексу
        public void Set(int index, ArmyUnit unit)
        {
            if (index >= 0 && index < units.Count)
                units[index] = unit ?? throw new ArgumentNullException(nameof(unit));
            else
                throw new IndexOutOfRangeException($"Индекс {index} вне диапазона [0, {units.Count - 1}]");
        }

        // Добавление единицы в армию
        public void AddUnit(ArmyUnit unit)
        {
            units.Add(unit ?? throw new ArgumentNullException(nameof(unit)));
        }

        // Удаление единицы из армии
        public bool RemoveUnit(ArmyUnit unit)
        {
            return units.Remove(unit);
        }

        // Удаление единицы по индексу
        public bool RemoveUnitAt(int index)
        {
            if (index >= 0 && index < units.Count)
            {
                units.RemoveAt(index);
                return true;
            }
            return false;
        }

        // Вывод всех единиц на консоль
        public void DisplayAllUnits()
        {
            if (units.Count == 0)
            {
                Console.WriteLine("Армия пуста");
                return;
            }

            Console.WriteLine("=== ВСЕ БОЕВЫЕ ЕДИНИЦЫ АРМИИ ===");
            for (int i = 0; i < units.Count; i++)
            {
                Console.Write($"[{i}] ");
                units[i].DisplayInfo();
            }
            Console.WriteLine($"Всего единиц: {units.Count}");
            Console.WriteLine("================================\n");
        }

        // Получение количества единиц
        public int Count => units.Count;

        // Получение списка всех единиц
        public List<ArmyUnit> GetAllUnits() => new List<ArmyUnit>(units);
    }

    // 4. КЛАСС-КОНТРОЛЛЕР (требование №4)
    public class ArmyController
    {
        private ArmyContainer container;

        public ArmyController()
        {
            container = new ArmyContainer();
        }

        public ArmyController(ArmyContainer armyContainer)
        {
            container = armyContainer ?? throw new ArgumentNullException(nameof(armyContainer));
        }

        // ЗАПРОС 1: Найти боевую единицу заданного года рождения/создания
        public List<ArmyUnit> FindUnitsByYear(int year)
        {
            return container.GetAllUnits()
                          .Where(unit => unit.Year == year)
                          .ToList();
        }

        // ЗАПРОС 2: Вывести имена трансформеров заданной мощности
        public List<string> GetTransformersByPower(int minPower)
        {
            return container.GetAllUnits()
                          .Where(unit => unit is Transformer)
                          .Cast<Transformer>()
                          .Where(transformer => transformer.Specs.Power >= minPower)
                          .Select(transformer => transformer.Name)
                          .ToList();
        }

        // ЗАПРОС 3: Получить количество боевых единиц в армии
        public int GetTotalUnitsCount()
        {
            return container.Count;
        }

        // ДОПОЛНИТЕЛЬНО: Загрузка из текстового файла (без JSON)
        public void LoadFromTextFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Файл не найден: {filePath}");
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var data = line.Split(';');
                    if (data.Length >= 4)
                    {
                        ArmyUnit unit;
                        var type = data[0].Trim();
                        var name = data[1].Trim();
                        var year = int.Parse(data[2].Trim());

                        if (type == "Human" && data.Length >= 8)
                        {
                            var rank = data[3].Trim();
                            var specialization = data[4].Trim();
                            var power = int.Parse(data[5].Trim());
                            var defense = int.Parse(data[6].Trim());
                            var weapon = data[7].Trim();

                            unit = new HumanUnit(name, year, rank, specialization, power, defense, weapon);
                            container.AddUnit(unit);
                        }
                        else if (type == "Transformer" && data.Length >= 7)
                        {
                            var altMode = data[3].Trim();
                            var power = int.Parse(data[4].Trim());
                            var defense = int.Parse(data[5].Trim());
                            var weapon = data[6].Trim();

                            unit = new Transformer(name, altMode, year, power, defense, weapon);
                            container.AddUnit(unit);
                        }
                    }
                }
                Console.WriteLine($"Данные загружены из файла: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки из текстового файла: {ex.Message}");
            }
        }

        // Демонстрация работы
        public void DemonstrateWork()
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАБОТЫ АРМИИ ===\n");

            // Показываем все единицы
            container.DisplayAllUnits();

            // Поиск единиц 2020 года
            Console.WriteLine("=== ЕДИНИЦЫ 2020 ГОДА ===");
            var units2020 = FindUnitsByYear(2020);
            if (units2020.Any())
            {
                foreach (var unit in units2020)
                {
                    unit.DisplayInfo();
                }
            }
            else
            {
                Console.WriteLine("Единицы 2020 года не найдены");
            }

            // Трансформеры с мощностью >= 80
            Console.WriteLine("\n=== ТРАНСФОРМЕРЫ С МОЩНОСТЬЮ >= 80 ===");
            var powerfulTransformers = GetTransformersByPower(80);
            if (powerfulTransformers.Any())
            {
                foreach (var name in powerfulTransformers)
                {
                    Console.WriteLine(name);
                }
            }
            else
            {
                Console.WriteLine("Трансформеры с мощностью >= 80 не найдены");
            }

            // Общее количество единиц
            Console.WriteLine($"\nОБЩЕЕ КОЛИЧЕСТВО БОЕВЫХ ЕДИНИЦ: {GetTotalUnitsCount()}");
        }
    }

    // ГЛАВНАЯ ПРОГРАММА
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Создаем армию
            var army = CreateTestArmy();
            var controller = new ArmyController(army);

            // Демонстрация работы
            controller.DemonstrateWork();

            // Тестирование partial класса
            TestPartialClass();

            // Тестирование загрузки из файла
            TestFileLoading();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static ArmyContainer CreateTestArmy()
        {
            var army = new ArmyContainer();

            // Добавляем людей
            army.AddUnit(new HumanUnit("Иван Петров", 1995, "Сержант", "Снайпер", 75, 60, "Снайперская винтовка"));
            army.AddUnit(new HumanUnit("Алексей Сидоров", 1990, "Лейтенант", "Командир", 80, 70, "Пистолет-пулемет"));
            army.AddUnit(new HumanUnit("Мария Козлова", 2000, "Рядовой", "Медик", 65, 55, "Пистолет"));
            army.AddUnit(new HumanUnit("Дмитрий Волков", 2020, "Капитан", "Спецназ", 90, 80, "Штурмовая винтовка"));

            // Добавляем трансформеров
            army.AddUnit(new Transformer("Оптимус Прайм", "Грузовик", 2020, 95, 85, "Энергетический меч"));
            army.AddUnit(new Transformer("Бамблби", "Спорткар", 2020, 75, 65, "Лазерная пушка"));
            army.AddUnit(new Transformer("Мегатрон", "Танк", 2018, 100, 90, "Плазменная пушка"));
            army.AddUnit(new Transformer("Сталькор", "Истребитель", 2019, 85, 75, "Ракетная установка"));

            return army;
        }

        static void TestPartialClass()
        {
            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ PARTIAL КЛАССА ===");

            var transformer = new Transformer("Тестовый", "Машина", 2023, 50, 40, "Тестовое оружие");

            // Методы из первой части
            transformer.DisplayInfo();
            transformer.Transform();

            // Методы из второй части
            transformer.DisplayPowerInfo();
            transformer.UpgradeWeapon("Улучшенное оружие");
            Console.WriteLine($"Достаточно мощен: {transformer.IsPowerfulEnough(30)}");
        }

        static void TestFileLoading()
        {
            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ЗАГРУЗКИ ИЗ ФАЙЛА ===");

            // Создаем тестовый файл
            var lines = new string[]
            {
                "Human;Олег Новиков;1998;Майор;Инженер;70;65;Инженерный инструмент",
                "Transformer;Валькирия;2021;Самолет;88;78;Авиационная пушка",
                "Human;Светлана Орлова;2001;Лейтенант;Связист;68;62;Радиостанция"
            };

            File.WriteAllLines("army_data.txt", lines);
            Console.WriteLine("Создан тестовый файл: army_data.txt");

            // Загружаем из файла
            var fileController = new ArmyController();
            fileController.LoadFromTextFile("army_data.txt");
            fileController.DemonstrateWork();
        }
    }
}