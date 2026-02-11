using System;

namespace Lab5_ArmyHierarchy
{
    // 1. ПЕРЕЧИСЛЕНИЕ И СТРУКТУРА (требование №1)
    public enum UnitType
    {
        Infantry,       // Пехота
        Mech,           // Мехи
        AirForce,       // Военно-воздушные силы  
        Transformer,    // Трансформеры
        SpecialForces   // Спецназ
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
        public int Year { get; set; } // Год рождения/создания
        public UnitType Type { get; set; }

        public ArmyUnit(string name, int year, UnitType type)
        {
            Name = name;
            Year = year;
            Type = type;
        }

        public abstract void DisplayInfo();
        public abstract void StartCombat();
        public abstract void StopCombat();

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
    }

    // 2. PARTIAL КЛАСС - ПЕРВАЯ ЧАСТЬ (требование №2)
    public partial class Transformer : ArmyUnit
    {
        public string RobotName { get; set; }
        public string AltMode { get; set; }
        public bool CanTransform { get; set; }
        public CombatSpecs Specs { get; set; }

        public Transformer(string robotName, string altMode, int creationYear, int power, int defense, string weapon)
            : base(robotName, creationYear, UnitType.Transformer)
        {
            RobotName = robotName;
            AltMode = altMode;
            CanTransform = true;
            Specs = new CombatSpecs(power, defense, weapon);
        }

        public void Transform()
        {
            if (CanTransform)
            {
                Console.WriteLine($"{RobotName} трансформируется в {AltMode}!");
            }
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

        public override string ToString()
        {
            return $"Трансформер: {RobotName}, Альт. режим: {AltMode}, Год создания: {Year}";
        }
    }

    // 2. PARTIAL КЛАСС - ВТОРАЯ ЧАСТЬ (требование №2)
    public partial class Transformer
    {
        public void UpgradeWeapon(string newWeapon)
        {
            // Исправление ошибки CS1612 - создаем новую структуру
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
    }
}