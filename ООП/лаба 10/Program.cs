using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQLab3
{
    // Класс Студент из лабораторной №2 (расширенный)
    public class Student : IComparable<Student>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Specialty { get; set; }
        public string Faculty { get; set; }
        public string Group { get; set; }
        public double AverageGrade { get; set; }

        public Student(string lastName, string firstName, string middleName,
                      DateTime birthDate, string specialty, string faculty,
                      string group, double averageGrade)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            BirthDate = birthDate;
            Specialty = specialty;
            Faculty = faculty;
            Group = group;
            AverageGrade = averageGrade;
        }

        public int CompareTo(Student other)
        {
            return LastName.CompareTo(other.LastName);
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}, {BirthDate:dd.MM.yyyy}, " +
                   $"{Specialty}, {Faculty}, {Group}, Ср. балл: {AverageGrade:F2}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Часть 1: Работа с месяцами ===");
            Part1_Months();

            Console.WriteLine("\n=== Часть 2-3: Работа со студентами (Вариант 3) ===");
            Part2_3_Students();

            Console.WriteLine("\n=== Часть 4: Сложный запрос с 5+ операторами ===");
            Part4_ComplexQuery();

            Console.WriteLine("\n=== Часть 5: Запрос с оператором Join ===");
            Part5_JoinQuery();

            Console.WriteLine("\n=== Ответы на теоретические вопросы ===");
            PrintAnswersToQuestions();
        }

        static void Part1_Months()
        {
            // 1. Массив месяцев
            string[] months = {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

            // a) Месяцы с длиной строки равной n
            int n = 5;
            var monthsLengthN = months.Where(m => m.Length == n);
            Console.WriteLine($"1a) Месяцы длиной {n} символов: {string.Join(", ", monthsLengthN)}");

            // b) Летние и зимние месяцы
            var summerWinterMonths = months.Where(m =>
                m == "June" || m == "July" || m == "August" ||
                m == "December" || m == "January" || m == "February");
            Console.WriteLine($"1b) Летние и зимние месяцы: {string.Join(", ", summerWinterMonths)}");

            // c) Месяцы в алфавитном порядке
            var orderedMonths = months.OrderBy(m => m);
            Console.WriteLine($"1c) Месяцы по алфавиту: {string.Join(", ", orderedMonths)}");

            // d) Месяцы с буквой 'u' длиной >= 4
            var monthsWithU = months.Where(m => m.Contains('u') && m.Length >= 4);
            int countMonthsWithU = monthsWithU.Count();
            Console.WriteLine($"1d) Месяцы с 'u' длиной >= 4 ({countMonthsWithU}): {string.Join(", ", monthsWithU)}");
        }

        static void Part2_3_Students()
        {
            // 2. Создание коллекции List<T> с 10+ элементами
            List<Student> students = new List<Student>
            {
                new Student("Иванов", "Иван", "Иванович", new DateTime(2000, 5, 15),
                           "Информатика", "ФКСиС", "951001", 8.5),
                new Student("Петров", "Петр", "Петрович", new DateTime(2001, 3, 22),
                           "Математика", "ФММ", "951002", 7.8),
                new Student("Сидоров", "Алексей", "Сергеевич", new DateTime(1999, 11, 10),
                           "Информатика", "ФКСиС", "951001", 9.2),
                new Student("Смирнова", "Анна", "Владимировна", new DateTime(2002, 7, 5),
                           "Физика", "ФФ", "951003", 8.9),
                new Student("Козлов", "Дмитрий", "Александрович", new DateTime(2000, 12, 30),
                           "Информатика", "ФКСиС", "951002", 7.5),
                new Student("Новикова", "Екатерина", "Игоревна", new DateTime(2001, 1, 18),
                           "Математика", "ФММ", "951002", 8.7),
                new Student("Федоров", "Сергей", "Викторович", new DateTime(1999, 9, 25),
                           "Информатика", "ФКСиС", "951001", 9.0),
                new Student("Морозова", "Ольга", "Дмитриевна", new DateTime(2002, 4, 12),
                           "Физика", "ФФ", "951003", 8.3),
                new Student("Волков", "Андрей", "Николаевич", new DateTime(2000, 8, 8),
                           "Информатика", "ФКСиС", "951002", 7.9),
                new Student("Алексеева", "Мария", "Петровна", new DateTime(2001, 6, 20),
                           "Математика", "ФММ", "951001", 9.1),
                new Student("Лебедев", "Артем", "Олегович", new DateTime(1999, 2, 14),
                           "Информатика", "ФКСиС", "951001", 8.6),
                new Student("Соколова", "Виктория", "Анатольевна", new DateTime(2002, 10, 3),
                           "Физика", "ФФ", "951003", 8.4)
            };

            Console.WriteLine("\nВсе студенты:");
            students.ForEach(s => Console.WriteLine($"  {s}"));

            // 3. Запросы по варианту 3

            // a) Список студентов заданной специальности по алфавиту (LINQ синтаксис)
            string targetSpecialty = "Информатика";
            var studentsBySpecialty = from s in students
                                      where s.Specialty == targetSpecialty
                                      orderby s.LastName
                                      select s;
            Console.WriteLine($"\n3a) Студенты специальности '{targetSpecialty}' по алфавиту:");
            foreach (var student in studentsBySpecialty)
            {
                Console.WriteLine($"  {student.LastName} {student.FirstName}");
            }

            // b) Список заданной учебной группы и факультета (методы расширения)
            string targetGroup = "951001";
            string targetFaculty = "ФКСиС";
            var studentsByGroupFaculty = students
                .Where(s => s.Group == targetGroup && s.Faculty == targetFaculty)
                .OrderBy(s => s.LastName);
            Console.WriteLine($"\n3b) Студенты группы {targetGroup} факультета {targetFaculty}:");
            foreach (var student in studentsByGroupFaculty)
            {
                Console.WriteLine($"  {student.LastName} {student.FirstName} ({student.Specialty})");
            }

            // c) Самого молодого студента
            var youngestStudent = students.OrderByDescending(s => s.BirthDate).FirstOrDefault();
            Console.WriteLine($"\n3c) Самый молодой студент: {youngestStudent}");

            // d) Количество студентов заданной группы упорядоченных по фамилии
            string groupToCount = "951002";
            var studentsInGroup = students
                .Where(s => s.Group == groupToCount)
                .OrderBy(s => s.LastName)
                .ToList();
            Console.WriteLine($"\n3d) Студенты группы {groupToCount} (всего {studentsInGroup.Count}):");
            foreach (var student in studentsInGroup)
            {
                Console.WriteLine($"  {student.LastName} {student.FirstName}");
            }

            // e) Первого студента с заданным именем
            string targetFirstName = "Анна";
            var firstStudentWithName = students.FirstOrDefault(s => s.FirstName == targetFirstName);
            Console.WriteLine($"\n3e) Первый студент с именем '{targetFirstName}':");
            if (firstStudentWithName != null)
                Console.WriteLine($"  {firstStudentWithName}");
            else
                Console.WriteLine($"  Студент с именем '{targetFirstName}' не найден");
        }

        static void Part4_ComplexQuery()
        {
            // Создаем дополнительную коллекцию для демонстрации
            List<Student> students = new List<Student>
            {
                new Student("Иванов", "Иван", "И", new DateTime(2000, 5, 15), "Информатика", "ФКСиС", "951001", 8.5),
                new Student("Петров", "Петр", "П", new DateTime(2001, 3, 22), "Математика", "ФММ", "951002", 7.8),
                new Student("Сидоров", "Алексей", "С", new DateTime(1999, 11, 10), "Информатика", "ФКСиС", "951001", 9.2),
                new Student("Смирнова", "Анна", "В", new DateTime(2002, 7, 5), "Физика", "ФФ", "951003", 8.9),
                new Student("Козлов", "Дмитрий", "А", new DateTime(2000, 12, 30), "Информатика", "ФКСиС", "951002", 7.5),
                new Student("Новикова", "Екатерина", "И", new DateTime(2001, 1, 18), "Математика", "ФММ", "951002", 8.7),
                new Student("Федоров", "Сергей", "В", new DateTime(1999, 9, 25), "Информатика", "ФКСиС", "951001", 9.0),
                new Student("Морозова", "Ольга", "Д", new DateTime(2002, 4, 12), "Физика", "ФФ", "951003", 8.3),
                new Student("Волков", "Андрей", "Н", new DateTime(2000, 8, 8), "Информатика", "ФКСиС", "951002", 7.9),
                new Student("Алексеева", "Мария", "П", new DateTime(2001, 6, 20), "Математика", "ФММ", "951001", 9.1)
            };

            Console.WriteLine("\nСложный запрос с использованием разных операторов:");

            // Сложный запрос с 5+ операторами из разных категорий:
            // 1. Where (условие)
            // 2. GroupBy (группировка)
            // 3. OrderBy (упорядочивание)
            // 4. Select (проекция)
            // 5. Take (разбиение)
            // 6. Average (агрегирование)
            // 7. Any (квантор)

            var complexQuery = students
                // Условие: студенты, родившиеся после 2000 года
                .Where(s => s.BirthDate.Year >= 2000)

                // Группировка по специальности
                .GroupBy(s => s.Specialty)

                // Упорядочивание групп по названию специальности
                .OrderBy(g => g.Key)

                // Проекция: создание нового объекта для каждой группы
                .Select(g => new
                {
                    Specialty = g.Key,
                    StudentsCount = g.Count(),
                    AverageGradeInGroup = g.Average(s => s.AverageGrade),
                    TopStudents = g
                        .OrderByDescending(s => s.AverageGrade)
                        .Take(2) // Разбиение: берем только 2 лучших студента
                        .Select(s => $"{s.LastName} ({s.AverageGrade:F1})")
                        .ToList(),
                    HasExcellentStudents = g.Any(s => s.AverageGrade >= 9.0) // Квантор
                })

                // Дополнительное условие: только группы с более чем 1 студентом
                .Where(g => g.StudentsCount > 1)

                // Финальное упорядочивание по среднему баллу
                .OrderByDescending(g => g.AverageGradeInGroup);

            Console.WriteLine("Результат сложного запроса:");
            foreach (var group in complexQuery)
            {
                Console.WriteLine($"\nСпециальность: {group.Specialty}");
                Console.WriteLine($"  Количество студентов: {group.StudentsCount}");
                Console.WriteLine($"  Средний балл: {group.AverageGradeInGroup:F2}");
                Console.WriteLine($"  Лучшие студенты: {string.Join(", ", group.TopStudents)}");
                Console.WriteLine($"  Есть отличники (балл ≥ 9.0): {(group.HasExcellentStudents ? "Да" : "Нет")}");
            }
        }

        static void Part5_JoinQuery()
        {
            // Создаем две коллекции для демонстрации Join
            List<Student> students = new List<Student>
            {
                new Student("Иванов", "Иван", "И", new DateTime(2000, 5, 15), "Информатика", "ФКСиС", "951001", 8.5),
                new Student("Петров", "Петр", "П", new DateTime(2001, 3, 22), "Математика", "ФММ", "951002", 7.8),
                new Student("Сидоров", "Алексей", "С", new DateTime(1999, 11, 10), "Информатика", "ФКСиС", "951001", 9.2),
                new Student("Смирнова", "Анна", "В", new DateTime(2002, 7, 5), "Физика", "ФФ", "951003", 8.9)
            };

            // Вторая коллекция: учебные дисциплины с баллами
            var subjects = new[]
            {
                new { StudentId = "Иванов", Subject = "Программирование", Grade = 9 },
                new { StudentId = "Иванов", Subject = "Математика", Grade = 8 },
                new { StudentId = "Петров", Subject = "Математика", Grade = 7 },
                new { StudentId = "Петров", Subject = "Физика", Grade = 8 },
                new { StudentId = "Сидоров", Subject = "Программирование", Grade = 10 },
                new { StudentId = "Сидоров", Subject = "Алгоритмы", Grade = 9 },
                new { StudentId = "Смирнова", Subject = "Физика", Grade = 9 },
                new { StudentId = "Смирнова", Subject = "Химия", Grade = 8 }
            };

            Console.WriteLine("\nЗапрос с оператором Join:");
            Console.WriteLine("Соединяем студентов с их оценками по предметам:");

            // Join студентов и предметов по фамилии
            var studentSubjects = students
                .Join(subjects,
                    student => student.LastName,
                    subject => subject.StudentId,
                    (student, subject) => new
                    {
                        student.LastName,
                        student.FirstName,
                        subject.Subject,
                        subject.Grade
                    })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.Subject);

            foreach (var item in studentSubjects)
            {
                Console.WriteLine($"  {item.LastName} {item.FirstName}: {item.Subject} - {item.Grade}");
            }

            // Пример GroupJoin (LEFT JOIN эквивалент)
            Console.WriteLine("\nGroupJoin (все студенты, даже без оценок):");
            var studentAllSubjects = students
                .GroupJoin(subjects,
                    student => student.LastName,
                    subject => subject.StudentId,
                    (student, subjectGroup) => new
                    {
                        Student = student,
                        Subjects = subjectGroup.DefaultIfEmpty(new { StudentId = student.LastName, Subject = "Нет предметов", Grade = 0 })
                    })
                .SelectMany(
                    x => x.Subjects,
                    (student, subject) => new
                    {
                        student.Student.LastName,
                        student.Student.FirstName,
                        Subject = subject.Subject,
                        Grade = subject.Grade
                    })
                .Where(x => x.Grade > 0); // Исключаем "Нет предметов"

            foreach (var item in studentAllSubjects)
            {
                Console.WriteLine($"  {item.LastName} {item.FirstName}: {item.Subject} - {item.Grade}");
            }
        }

        static void PrintAnswersToQuestions()
        {
            Console.WriteLine("\n=== Краткие ответы на теоретические вопросы ===");
            Console.WriteLine("1. LINQ (Language Integrated Query) - технология, позволяющая выполнять запросы к данным непосредственно в коде C#.");
            Console.WriteLine("2. Отложенные операции выполняются только при перечислении результатов, неотложные - сразу при вызове.");
            Console.WriteLine("3. Лямбда-выражения - анонимные функции для создания делегатов или деревьев выражений.");
            Console.WriteLine("4. Группы операций: фильтрация, проекция, сортировка, группировка, соединение, агрегирование, кванторы, разбиение.");
            Console.WriteLine("5. Where - фильтрация элементов по условию.");
            Console.WriteLine("6. Select - проекция (преобразование) элементов.");
            Console.WriteLine("7. Take - взять первые N элементов, Skip - пропустить первые N элементов.");
            Console.WriteLine("8. Concat - объединение двух последовательностей.");
            Console.WriteLine("9. OrderBy - сортировка по возрастанию, OrderByDescending - по убыванию.");
            Console.WriteLine("10. Join - соединение двух последовательностей по ключу.");
            Console.WriteLine("11. Distinct - уникальные элементы, Union - объединение, Except - разность, Intersect - пересечение.");
            Console.WriteLine("12. First/FirstOrDefault - первый элемент, Last/LastOrDefault - последний, Any - проверка наличия, All - все ли удовлетворяют условию, Contains - содержит ли элемент.");
            Console.WriteLine("13. Count - количество, Sum - сумма, Min - минимум, Max - максимум, Average - среднее значение.");
            Console.WriteLine("14. Данный код выведет: 10");
        }
    }
}