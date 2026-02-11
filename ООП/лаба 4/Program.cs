using System;
using System.Collections.Generic;

namespace Lab4_TransportHierarchy
{
    // 1. Интерфейс управления
    public interface IControl
    {
        void Start();
        void Stop();
        bool DoClone();
    }

    // 2. Абстрактный класс с одноименным методом
    public abstract class BaseControl
    {
        public abstract bool DoClone();
        public virtual void ShowInfo()
        {
            Console.WriteLine("Базовый класс управления");
        }
    }

    // 3. Абстрактный класс Транспортное средство
    public abstract class Transport : BaseControl, IControl
    {
        public string Name { get; set; }
        public int MaxSpeed { get; set; }
        public int Weight { get; set; }

        protected Transport(string name, int maxSpeed, int weight)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            Weight = weight;
        }

        // Реализация интерфейса IControl
        public virtual void Start()
        {
            Console.WriteLine($"{Name} запущен");
        }

        public virtual void Stop()
        {
            Console.WriteLine($"{Name} остановлен");
        }

        // Одноименные методы с разной реализацией
        public override abstract bool DoClone(); // из BaseControl - теперь абстрактный

        bool IControl.DoClone() // явная реализация интерфейса
        {
            Console.WriteLine("Клонирование через интерфейс IControl");
            return true;
        }

        public override string ToString()
        {
            return $"Транспорт: {Name}, Макс. скорость: {MaxSpeed} км/ч, Вес: {Weight} кг";
        }
    }

    // 4. Класс Двигатель
    public class Engine
    {
        public string Type { get; set; }
        public int Power { get; set; }
        public double Volume { get; set; }

        public Engine(string type, int power, double volume)
        {
            Type = type;
            Power = power;
            Volume = volume;
        }

        public override string ToString()
        {
            return $"Двигатель: {Type}, Мощность: {Power} л.с., Объем: {Volume} л";
        }
    }

    // 5. Класс Машина
    public class Car : Transport
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public Engine CarEngine { get; set; }
        public int DoorsCount { get; set; }

        public Car(string brand, string model, int maxSpeed, int weight,
                  Engine engine, int doors)
                  : base($"{brand} {model}", maxSpeed, weight)
        {
            Brand = brand;
            Model = model;
            CarEngine = engine;
            DoorsCount = doors;
        }

        public void ChangeGear(int gear)
        {
            Console.WriteLine($"{Name} переключена на {gear} передачу");
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"{Brand} {Model} завелась с рычанием двигателя");
        }

        // РЕАЛИЗАЦИЯ абстрактного метода DoClone из BaseControl
        public override bool DoClone()
        {
            Console.WriteLine("Клонирование машины через абстрактный класс");
            return false;
        }

        public override string ToString()
        {
            return $"Автомобиль: {Brand} {Model}, Дверей: {DoorsCount}, {CarEngine}";
        }
    }

    // 6. Абстрактный класс Разумное существо
    public abstract class IntelligentBeing : BaseControl, IControl
    {
        public string Species { get; set; }
        public int IntelligenceLevel { get; set; }
        public int Age { get; set; }

        protected IntelligentBeing(string species, int intelligence, int age)
        {
            Species = species;
            IntelligenceLevel = intelligence;
            Age = age;
        }

        public abstract void Think();
        public abstract void Communicate();

        // Реализация IControl
        public virtual void Start()
        {
            Console.WriteLine($"{Species} начал действовать");
        }

        public virtual void Stop()
        {
            Console.WriteLine($"{Species} прекратил действовать");
        }

        // Одноименные методы - делаем абстрактным для реализации в наследниках
        public override abstract bool DoClone();

        bool IControl.DoClone()
        {
            Console.WriteLine("Клонирование разумного существа через интерфейс");
            return true;
        }

        public override string ToString()
        {
            return $"Разумное существо: {Species}, Интеллект: {IntelligenceLevel}, Возраст: {Age} лет";
        }
    }

    // 7. Класс Человек
    public class Human : IntelligentBeing
    {
        public string Name { get; set; }
        public string Profession { get; set; }

        public Human(string name, string profession, int age)
            : base("Человек", 100, age)
        {
            Name = name;
            Profession = profession;
        }

        public void Work()
        {
            Console.WriteLine($"{Name} работает как {Profession}");
        }

        public override void Think()
        {
            Console.WriteLine($"{Name} размышляет о жизни");
        }

        public override void Communicate()
        {
            Console.WriteLine($"{Name} общается с другими людьми");
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine($"{Name} начал свой день");
        }

        // РЕАЛИЗАЦИЯ абстрактного метода DoClone из IntelligentBeing
        public override bool DoClone()
        {
            Console.WriteLine("Клонирование человека через абстрактный класс");
            return false;
        }

        public override string ToString()
        {
            return $"Человек: {Name}, Профессия: {Profession}, Возраст: {Age}";
        }
    }

    // 8. Sealed класс Трансформер
    public sealed class Transformer : Transport
    {
        public string RobotName { get; set; }
        public string AltMode { get; set; }
        public bool CanTransform { get; set; }

        public Transformer(string robotName, string altMode, int maxSpeed, int weight)
            : base(robotName, maxSpeed, weight)
        {
            RobotName = robotName;
            AltMode = altMode;
            CanTransform = true;
        }

        public void Transform()
        {
            if (CanTransform)
            {
                Console.WriteLine($"{RobotName} трансформируется в {AltMode}!");
            }
        }

        public override void Start()
        {
            Console.WriteLine($"{RobotName} активирован. Режим: {AltMode}");
        }

        // РЕАЛИЗАЦИЯ абстрактного метода DoClone из Transport
        public override bool DoClone()
        {
            Console.WriteLine("Клонирование трансформера невозможно - уникальная структура");
            return false;
        }

        public override string ToString()
        {
            return $"Трансформер: {RobotName}, Альт. режим: {AltMode}, " +
                   $"Макс. скорость: {MaxSpeed} км/ч";
        }
    }

    // 9. Класс Printer с полиморфным методом
    public class Printer
    {
        public void IAmPrinting(IControl obj)
        {
            if (obj == null) return;

            // Определение типа с помощью is и as
            if (obj is Transport transport)
            {
                Console.WriteLine($"Печатается транспорт: {transport.GetType().Name}");
            }
            else if (obj is IntelligentBeing being)
            {
                Console.WriteLine($"Печатается существо: {being.GetType().Name}");
            }

            // Вызов ToString()
            Console.WriteLine($"Информация: {obj.ToString()}");
            Console.WriteLine("---");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ДЕМОНСТРАЦИОННАЯ ПРОГРАММА ===\n");

            // Создание объектов
            Engine v8Engine = new Engine("V8 Бензиновый", 450, 4.0);
            Car bmw = new Car("BMW", "X5", 250, 2200, v8Engine, 5);

            Human john = new Human("Джон Коннор", "Механик", 25);

            Transformer optimus = new Transformer("Оптимус Прайм", "Грузовик", 120, 5000);

            // Работа через ссылки на абстрактные классы
            Transport[] transports = { bmw, optimus };
            IntelligentBeing[] beings = { john };

            Console.WriteLine("=== РАБОТА С ТРАНСПОРТОМ ===");
            foreach (var transport in transports)
            {
                transport.Start();
                transport.Stop();

                // Вызов одноименных методов
                transport.DoClone(); // из абстрактного класса
                ((IControl)transport).DoClone(); // из интерфейса

                Console.WriteLine();
            }

            Console.WriteLine("=== РАБОТА С РАЗУМНЫМИ СУЩЕСТВАМИ ===");
            foreach (var being in beings)
            {
                being.Start();
                being.Think();
                being.Communicate();

                // Вызов одноименных методов
                being.DoClone(); // из абстрактного класса
                ((IControl)being).DoClone(); // из интерфейса

                Console.WriteLine();
            }

            // Дополнительные методы конкретных классов
            Console.WriteLine("=== СПЕЦИФИЧЕСКИЕ МЕТОДЫ ===");
            bmw.ChangeGear(3);
            john.Work();
            optimus.Transform();

            Console.WriteLine("\n=== ИСПОЛЬЗОВАНИЕ OPERATORS IS/AS ===");
            CheckObjectType(bmw);
            CheckObjectType(john);
            CheckObjectType(optimus);
            CheckObjectType(v8Engine);

            Console.WriteLine("\n=== ПОЛИМОРФНЫЙ МЕТОД IAmPrinting ===");
            Printer printer = new Printer();

            // Массив с ссылками на интерфейс
            IControl[] controls = { bmw, john, optimus };

            foreach (var control in controls)
            {
                printer.IAmPrinting(control);
            }

            // Тестирование sealed класса
            Console.WriteLine("=== ТЕСТИРОВАНИЕ SEALED КЛАССА ===");
            TestSealedClass(optimus);

            Console.WriteLine("\n=== ВЫЗОВ TOSTRING() ДЛЯ ВСЕХ ОБЪЕКТОВ ===");
            Console.WriteLine(bmw.ToString());
            Console.WriteLine(john.ToString());
            Console.WriteLine(optimus.ToString());
            Console.WriteLine(v8Engine.ToString());
        }

        static void CheckObjectType(object obj)
        {
            Console.WriteLine($"Проверка объекта: {obj.GetType().Name}");

            // Использование is
            if (obj is Car car)
            {
                Console.WriteLine($"- Это автомобиль: {car.Brand}");
            }
            else if (obj is Human human)
            {
                Console.WriteLine($"- Это человек: {human.Name}");
            }
            else if (obj is Transformer transformer)
            {
                Console.WriteLine($"- Это трансформер: {transformer.RobotName}");
            }
            else if (obj is IControl control)
            {
                Console.WriteLine($"- Объект реализует IControl");
            }
            else
            {
                Console.WriteLine($"- Неизвестный тип объекта");
            }

            // Использование as
            Transport transport = obj as Transport;
            if (transport != null)
            {
                Console.WriteLine($"- Может использоваться как транспорт: {transport.Name}");
            }

            IntelligentBeing being = obj as IntelligentBeing;
            if (being != null)
            {
                Console.WriteLine($"- Может использоваться как разумное существо: {being.Species}");
            }
        }

        static void TestSealedClass(Transformer transformer)
        {
            Console.WriteLine($"Sealed класс Transformer: {transformer.RobotName}");
            // Попытка наследования от sealed класса приведет к ошибке компиляции
            // class NewTransformer : Transformer {} // ОШИБКА!
        }
    }
}