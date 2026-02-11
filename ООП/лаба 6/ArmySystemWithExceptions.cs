using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace Lab6_ArmyHierarchy
{
    // Класс-контейнер с обработкой исключений
    public class ArmyContainer
    {
        private List<ArmyUnit> units;

        public ArmyContainer()
        {
            units = new List<ArmyUnit>();
        }

        public ArmyUnit Get(int index)
        {
            if (index < 0 || index >= units.Count)
                throw new IndexOutOfRangeException($"Индекс {index} вне диапазона [0, {units.Count - 1}]");

            return units[index];
        }

        public void Set(int index, ArmyUnit unit)
        {
            if (index < 0 || index >= units.Count)
                throw new IndexOutOfRangeException($"Индекс {index} вне диапазона [0, {units.Count - 1}]");

            if (unit == null)
                throw new ArgumentNullException(nameof(unit), "Единица не может быть null");

            units[index] = unit;
        }

        public void AddUnit(ArmyUnit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), "Единица не может быть null");

            units.Add(unit);
        }

        public bool RemoveUnit(ArmyUnit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), "Единица не может быть null");

            return units.Remove(unit);
        }

        public bool RemoveUnitAt(int index)
        {
            if (index < 0 || index >= units.Count)
                throw new IndexOutOfRangeException($"Индекс {index} вне диапазона [0, {units.Count - 1}]");

            units.RemoveAt(index);
            return true;
        }

        public void DisplayAllUnits()
        {
            if (units.Count == 0)
                throw new ArmyContainerException("Контейнер армии пуст");

            Console.WriteLine("=== ВСЕ БОЕВЫЕ ЕДИНИЦЫ АРМИИ ===");
            for (int i = 0; i < units.Count; i++)
            {
                Console.Write($"[{i}] ");
                units[i].DisplayInfo();
            }
            Console.WriteLine($"Всего единиц: {units.Count}");
            Console.WriteLine("================================\n");
        }

        public int Count => units.Count;
        public List<ArmyUnit> GetAllUnits() => new List<ArmyUnit>(units);
    }

    // Класс-контроллер с обработкой исключений
    public class ArmyController
    {
        private ArmyContainer container;

        public ArmyContainer Container
        {
            get => container;
            set => container = value ?? throw new ArgumentNullException(nameof(value));
        }

        public ArmyController()
        {
            container = new ArmyContainer();
        }

        public ArmyController(ArmyContainer armyContainer)
        {
            Container = armyContainer;
        }

        public List<ArmyUnit> FindUnitsByYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentException($"Некорректный год: {year}");

            return container.GetAllUnits()
                          .Where(unit => unit.Year == year)
                          .ToList();
        }

        public List<string> GetTransformersByPower(int minPower)
        {
            if (minPower < 0)
                throw new ArgumentException("Минимальная мощность не может быть отрицательной");

            return container.GetAllUnits()
                          .Where(unit => unit is Transformer)
                          .Cast<Transformer>()
                          .Where(transformer => transformer.Specs.Power >= minPower)
                          .Select(transformer => transformer.Name)
                          .ToList();
        }

        public int GetTotalUnitsCount()
        {
            return container.Count;
        }

        // Метод с многоуровневой обработкой исключений
        public void LoadFromTextFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым");

            if (!File.Exists(filePath))
                throw new FileOperationException(filePath, "Файл не существует");

            try
            {
                var lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(lines[i])) continue;

                        var data = lines[i].Split(';');
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
                    catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                    {
                        // Фильтр исключений (требование №6)
                        throw new FileOperationException(filePath,
                            $"Ошибка формата данных в строке {i + 1}: {lines[i]}", ex);
                    }
                }
                Console.WriteLine($"Данные загружены из файла: {filePath}");
            }
            catch (IOException ex)
            {
                throw new FileOperationException(filePath, "Ошибка ввода-вывода", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FileOperationException(filePath, "Нет доступа к файлу", ex);
            }
        }

        // Метод для демонстрации проброса исключения
        public void DemonstrateCombatCalculation(string unitName)
        {
            try
            {
                var unit = container.GetAllUnits()
                                  .FirstOrDefault(u => u.Name == unitName);

                if (unit == null)
                    throw new InvalidUnitException(unitName, "Единица не найдена");

                var effectiveness = unit.CalculateCombatEffectiveness();
                Console.WriteLine($"Эффективность боя для {unitName}: {effectiveness}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Ошибка деления на ноль при расчете эффективности для {unitName}");
                throw new ArmyException($"Ошибка расчета боевой эффективности для {unitName}", ex);
            }
        }

        public void DemonstrateWork()
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАБОТЫ АРМИИ ===\n");

            container.DisplayAllUnits();

            Console.WriteLine("=== ЕДИНИЦЫ 2020 ГОДА ===");
            var units2020 = FindUnitsByYear(2020);
            foreach (var unit in units2020)
            {
                unit.DisplayInfo();
            }

            Console.WriteLine("\n=== ТРАНСФОРМЕРЫ С МОЩНОСТЬЮ >= 80 ===");
            var powerfulTransformers = GetTransformersByPower(80);
            foreach (var name in powerfulTransformers)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine($"\nОБЩЕЕ КОЛИЧЕСТВО БОЕВЫХ ЕДИНИЦ: {GetTotalUnitsCount()}");
        }
    }

    // ГЛАВНАЯ ПРОГРАММА С ОБРАБОТКОЙ ИСКЛЮЧЕНИЙ
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // Демонстрация различных исключений
                DemonstrateVariousExceptions();

                // Демонстрация работы с файлами
                DemonstrateFileOperations();

                // Демонстрация Assert и Debugger
                DemonstrateAssertAndDebugger();

                // Нормальная работа
                DemonstrateNormalWork();
            }
            catch (ArmyException ex)
            {
                Console.WriteLine($"\n ПЕРЕХВАЧЕНО АРМЕЙСКОЕ ИСКЛЮЧЕНИЕ:");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"Тип: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n⚠️ ПЕРЕХВАЧЕНО НЕОБРАБОТАННОЕ ИСКЛЮЧЕНИЕ:");
                Console.WriteLine($"Тип: {ex.GetType().Name}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
            }
            finally
            {
                // Блок finally (требование №6)
                Console.WriteLine("\n=== БЛОК FINALLY ===");
                Console.WriteLine("Программа завершила выполнение (успешно или с ошибками)");
                Console.WriteLine("Освобождение ресурсов...");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static void DemonstrateVariousExceptions()
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАЗЛИЧНЫХ ИСКЛЮЧЕНИЙ ===\n");

            var army = new ArmyContainer();
            var controller = new ArmyController(army);

            try
            {
                // 1. Исключение при создании объекта с невалидными данными
                Console.WriteLine("1. Попытка создать юнита с пустым именем:");
                var invalidUnit = new HumanUnit("", 1990, "Сержант", "Снайпер", 75, 60, "Винтовка");
            }
            catch (InvalidUnitException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }

            try
            {
                // 2. Исключение при некорректном годе
                Console.WriteLine("\n2. Попытка создать юнита с некорректным годом:");
                var invalidYearUnit = new HumanUnit("Иван", 1800, "Сержант", "Снайпер", 75, 60, "Винтовка");
            }
            catch (InvalidUnitException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }

            try
            {
                // 3. Исключение при работе с пустым контейнером
                Console.WriteLine("\n3. Попытка отобразить пустой контейнер:");
                army.DisplayAllUnits();
            }
            catch (ArmyContainerException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }

            // Добавим валидные данные для дальнейших тестов
            army.AddUnit(new HumanUnit("Иван Петров", 1995, "Сержант", "Снайпер", 75, 60, "Снайперская винтовка"));
            army.AddUnit(new Transformer("Оптимус", "Грузовик", 2020, 95, 85, "Энергетический меч"));

            try
            {
                // 4. Исключение при обращении по неверному индексу
                Console.WriteLine("\n4. Попытка получить юнита по неверному индексу:");
                var unit = army.Get(10);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }

            try
            {
                // 5. Исключение деления на ноль с пробросом
                Console.WriteLine("\n5. Попытка расчета боевой эффективности:");
                controller.DemonstrateCombatCalculation("Иван Петров");
            }
            catch (ArmyException ex)
            {
                Console.WriteLine($"Перехвачено после проброса: {ex.GetType().Name} - {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Исходное исключение: {ex.InnerException.GetType().Name}");
                }
            }

            try
            {
                // 6. Исключение при передаче null
                Console.WriteLine("\n6. Попытка добавить null в контейнер:");
                army.AddUnit(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"Параметр: {ex.ParamName}");
            }

            try
            {
                // 7. Исключение в методе трансформера
                Console.WriteLine("\n7. Попытка критической атаки слабым трансформером:");
                var weakTransformer = new Transformer("Слабак", "Машина", 2023, 30, 40, "Слабое оружие");
                weakTransformer.CriticalAttack();
            }
            catch (CombatException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }
        }

        static void DemonstrateFileOperations()
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ РАБОТЫ С ФАЙЛАМИ ===\n");

            var controller = new ArmyController();

            try
            {
                // 1. Исключение при несуществующем файле
                Console.WriteLine("1. Попытка загрузить несуществующий файл:");
                controller.LoadFromTextFile("nonexistent_file.txt");
            }
            catch (FileOperationException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }

            try
            {
                // 2. Создаем файл с ошибками формата
                File.WriteAllText("invalid_data.txt", "Human;Иван;не_число;Сержант\nTransformer;Робот;2020;не_число;100;Меч");

                Console.WriteLine("\n2. Попытка загрузить файл с ошибками формата:");
                controller.LoadFromTextFile("invalid_data.txt");
            }
            catch (FileOperationException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
                }
            }
        }

        static void DemonstrateAssertAndDebugger()
        {
            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ ASSERT И DEBUGGER ===\n");

            var human = new HumanUnit("Тестовый", 1990, "Сержант", "Тестер", 70, 60, "Тестовое оружие");

            try
            {
                Console.WriteLine("1. Успешная тренировка:");
                human.PerformTraining(5); // Assert сработает в Debug режиме

                Console.WriteLine("\n2. Попытка тренировки с отрицательными часами:");
                human.PerformTraining(-2); // Вызовет исключение
            }
            catch (InvalidUnitException ex)
            {
                Console.WriteLine($"Перехвачено: {ex.GetType().Name} - {ex.Message}");
            }

            // Демонстрация Debugger
            Console.WriteLine("\n3. Проверка отладчика:");
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Отладчик подключен");
                Debugger.Break(); // Точка останова в отладчике
            }
            else
            {
                Console.WriteLine("Отладчик не подключен");
            }

            // Использование Debug.Assert
            var testArray = new ArmyUnit[] { human };
            Debug.Assert(testArray != null, "Массив не должен быть null");
            Debug.Assert(testArray.Length > 0, "Массив не должен быть пустым");

            Console.WriteLine("Все проверки Assert пройдены");
        }

        static void DemonstrateNormalWork()
        {
            Console.WriteLine("\n=== НОРМАЛЬНАЯ РАБОТА БЕЗ ОШИБОК ===\n");

            // Создаем армию с валидными данными
            var army = new ArmyContainer();
            army.AddUnit(new HumanUnit("Анна Сидорова", 2000, "Лейтенант", "Командир", 80, 70, "Пистолет-пулемет"));
            army.AddUnit(new Transformer("Бамблби", "Спорткар", 2020, 75, 65, "Лазерная пушка"));

            var controller = new ArmyController(army);
            controller.DemonstrateWork();
        }
    }
}