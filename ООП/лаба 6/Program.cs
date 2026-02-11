using System;
using System.Diagnostics;

namespace Lab6_ArmyHierarchy
{
    // 1. ИЕРАРХИЯ КЛАССОВ ИСКЛЮЧЕНИЙ 
    public class ArmyException : Exception
    {
        public ArmyException(string message) : base(message) { }
        public ArmyException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidUnitException : ArmyException
    {
        public string UnitName { get; }

        public InvalidUnitException(string unitName, string message)
            : base($"Ошибка в единице '{unitName}': {message}")
        {
            UnitName = unitName;
        }
    }

    public class ArmyContainerException : ArmyException
    {
        public ArmyContainerException(string message) : base(message) { }
    }

    public class CombatException : ArmyException
    {
        public int PowerLevel { get; }

        public CombatException(int power, string message)
            : base($"Боевая ошибка (мощность: {power}): {message}")
        {
            PowerLevel = power;
        }
    }

    public class FileOperationException : ArmyException
    {
        public string FilePath { get; }

        public FileOperationException(string filePath, string message, Exception inner = null)
            : base($"Ошибка работы с файлом '{filePath}': {message}", inner)
        {
            FilePath = filePath;
        }
    }

    // Перечисление и структура
    public enum UnitType
    {
        Infantry,
        Mech,
        AirForce,
        Transformer,
        SpecialForces
    }

    public struct CombatSpecs
    {
        public int Power;
        public int Defense;
        public string Weapon;

        public CombatSpecs(int power, int defense, string weapon)
        {
            Power = power;
            Defense = defense;
            Weapon = weapon;
        }

        public override string ToString()
        {
            return $"Сила: {Power}, Защита: {Defense}, Оружие: {Weapon}";
        }
    }

    // Базовый абстрактный класс для боевых единиц
    public abstract class ArmyUnit
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public UnitType Type { get; set; }

        public ArmyUnit(string name, int year, UnitType type)
        {
            // Проверка валидности данных при создании
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidUnitException(name ?? "null", "Имя не может быть пустым");

            if (year < 1900 || year > DateTime.Now.Year)
                throw new InvalidUnitException(name, $"Некорректный год: {year}");

            Name = name;
            Year = year;
            Type = type;
        }

        public abstract void DisplayInfo();
        public abstract void StartCombat();
        public abstract void StopCombat();

        // Метод для демонстрации деления на ноль
        public virtual double CalculateCombatEffectiveness()
        {
            int divisor = Year - 1900;
            if (divisor == 0)
            {
                divisor = 0; // Намеренная ошибка для демонстрации
            }
            return 100.0 / divisor; // Возможное деление на ноль
        }

        public override string ToString()
        {
            return $"{Name} ({Type}), Год: {Year}";
        }
    }

    // Класс для людей в армии
    public class HumanUnit : ArmyUnit
    {
        public string Rank { get; set; }
        public string Specialization { get; set; }
        public CombatSpecs Specs { get; set; }

        public HumanUnit(string name, int birthYear, string rank, string specialization, int power, int defense, string weapon)
            : base(name, birthYear, UnitType.Infantry)
        {
            if (string.IsNullOrWhiteSpace(rank))
                throw new InvalidUnitException(name, "Звание не может быть пустым");

            if (power <= 0 || defense <= 0)
                throw new InvalidUnitException(name, "Сила и защита должны быть положительными");

            Rank = rank;
            Specialization = specialization;
            Specs = new CombatSpecs(power, defense, weapon);
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Человек: {Name}, Звание: {Rank}, Год рождения: {Year}, Специализация: {Specialization}, {Specs}");
        }

        public override void StartCombat()
        {
            Console.WriteLine($"{Rank} {Name} начинает боевые действия");
        }

        public override void StopCombat()
        {
            Console.WriteLine($"{Rank} {Name} прекращает боевые действия");
        }

        public void Train()
        {
            Console.WriteLine($"{Name} проходит боевую подготовку");
        }

        // Использование Assert (требование №7)
        public void PerformTraining(int trainingHours)
        {
            Debug.Assert(trainingHours > 0, "Количество часов тренировки должно быть положительным");

            if (trainingHours <= 0)
                throw new InvalidUnitException(Name, "Количество часов тренировки должно быть положительным");

            Console.WriteLine($"{Name} тренируется {trainingHours} часов");
        }
    }

    // Partial класс Трансформер
    public partial class Transformer : ArmyUnit
    {
        public string RobotName { get; set; }
        public string AltMode { get; set; }
        public bool CanTransform { get; set; }
        public CombatSpecs Specs { get; set; }

        public Transformer(string robotName, string altMode, int creationYear, int power, int defense, string weapon)
            : base(robotName, creationYear, UnitType.Transformer)
        {
            if (power <= 0)
                throw new InvalidUnitException(robotName, "Мощность должна быть положительной");

            if (defense <= 0)
                throw new InvalidUnitException(robotName, "Защита должна быть положительной");

            RobotName = robotName;
            AltMode = altMode;
            CanTransform = true;
            Specs = new CombatSpecs(power, defense, weapon);
        }

        public void Transform()
        {
            if (!CanTransform)
                throw new CombatException(Specs.Power, "Трансформер не может трансформироваться");

            Console.WriteLine($"{RobotName} трансформируется в {AltMode}!");
        }

        public override void StartCombat()
        {
            Console.WriteLine($"{RobotName} активирован. Режим: {AltMode}");
        }

        public override void StopCombat()
        {
            Console.WriteLine($"{RobotName} деактивирован");
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Трансформер: {RobotName}, Альт. режим: {AltMode}, Год создания: {Year}, {Specs}");
        }

        public override double CalculateCombatEffectiveness()
        {
            // Демонстрация работы с Debugger (требование №8)
            if (Debugger.IsAttached)
            {
                Debugger.Break(); // Точка останова в отладчике
            }

            return base.CalculateCombatEffectiveness();
        }

        public override string ToString()
        {
            return $"Трансформер: {RobotName}, Альт. режим: {AltMode}, Год создания: {Year}";
        }
    }

    public partial class Transformer
    {
        public void UpgradeWeapon(string newWeapon)
        {
            if (string.IsNullOrWhiteSpace(newWeapon))
                throw new InvalidUnitException(RobotName, "Новое оружие не может быть пустым");

            var newSpecs = new CombatSpecs(Specs.Power, Specs.Defense, newWeapon);
            Specs = newSpecs;
            Console.WriteLine($"{RobotName} улучшил оружие до: {newWeapon}");
        }

        public bool IsPowerfulEnough(int minPower)
        {
            return Specs.Power >= minPower;
        }

        public void DisplayPowerInfo()
        {
            Console.WriteLine($"{RobotName} имеет мощность: {Specs.Power} единиц");
        }

        // Метод с пробросом исключения (требование №5)
        public void CriticalAttack()
        {
            try
            {
                if (Specs.Power < 50)
                {
                    throw new CombatException(Specs.Power, "Недостаточно мощности для критической атаки");
                }
                Console.WriteLine($"{RobotName} выполняет критическую атаку!");
            }
            catch (CombatException ex)
            {
                Console.WriteLine($"Перехвачено в CriticalAttack: {ex.Message}");
                throw; // Проброс исключения выше (требование №5)
            }
        }
    }
}