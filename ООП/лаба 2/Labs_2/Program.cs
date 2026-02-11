using System;
using System.Collections.Generic;
using System.Linq;
// ex 1
public class Student
{
    private static int objectCount;
    private readonly int id; // присваивается значение и в конструктуре и при инициализации
    private string lastName;
    private string firstName;
    private string patronymic;
    private DateTime dateOfBirth;
    private string address;
    private string phone;
    private int course;
    private string group;
    private const string DefaultFaculty = "Computer Science"; // только при инициализации 
     
    // cтатический
    static Student()
    {
        Console.WriteLine($"Добро пожаловать в {DefaultFaculty}!");
        objectCount = 0;
    }

    // закрытый
    private Student()
    {
        this.id = GenerateUniqueId();
    }

    // с параметрами
    public Student(string lastName, string firstName, string patronymic, DateTime dateOfBirth,
                   string address, string phone, int course, string group)
    {

        this.id = GenerateUniqueId();
        LastName = lastName;
        FirstName = firstName;
        this.patronymic = patronymic;
        DateOfBirth = dateOfBirth;
        Address = address;
        Phone = phone;
        Course = course;
        Group = group;

        objectCount++;
    }

    // с параметрами по умолчанию
    public Student(string lastName, string firstName) : this(lastName, firstName, "", DateTime.MinValue, "", "", 1, "Default")
    {
    }
    public int Id
    {
        get
        {
            return id;
        }
    }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    lastName = value;
                }
            }
        }

    public string FirstName 
    {
        get
        {
            return firstName;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                firstName = value;
            }
        }
    }

    public string Patronymic
    {
        get
        {
            return patronymic;
        }
    }

    public DateTime DateOfBirth
    {
        get
        {
            return dateOfBirth;
        }
        set
        {
            if (value < DateTime.Now) // что бы люди позже не рождались 
            {
                dateOfBirth = value;
            }
        }
    }

    public string Address
    {
        get
        {
            return address;
        }
        set
        {
            address = value;
        }
    }

    public string Phone
    {
        get
        {
            return phone;
        }
        set
        {
            phone = value;
        }
    }

    public int Course
    {
        get
        {
            return course;
        }
        set
        {
            if (value >= 1 && value <= 5)
            {
                course = value;
            }
        }
    }

    public string Group
    {
        get
        {
            return group;
        }
        set
        {
            group = value;
        }
    }

    // возраст студента
    public int CalculateAge()
    {
        int age = DateTime.Now.Year - DateOfBirth.Year;
        return age;
    }

    // ref и out
    public void GetStudentInfo(out string fullName, ref int age)
    {
        fullName = LastName + " " + FirstName + " " + Patronymic;
        age = CalculateAge();
    }

    public static int ObjectCount
    {
        get
        {
            return objectCount;
        }
    }

    public static void PrintClassInfo()
    {
        Console.WriteLine($"Количество объектов: {objectCount}");
    }

    private int GenerateUniqueId()
    {
        return ++objectCount; // ID
    }

    //Equals
    public override bool Equals(object obj)
    {
        if (obj is Student student)
        {
            return id == student.id;
        }
        return false;
    }

    // GetHashCode
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }

    // ToString
    public override string ToString()
    {
        return $"{LastName} {FirstName} {Patronymic}, ID: {id}, Возраст: {CalculateAge()}, Группа: {Group}, Адрес: {Address}, Телефон: {Phone}";
    }
}

public class Program
{
    public static void Main()
    {
        Student student1 = new Student("Иванов", "Иван", "Иванович", new DateTime(2000, 1, 1), "Улица 1", "123456789", 1, "Группа А");
        Student student2 = new Student("Петров", "Петр", "Петрович", new DateTime(1999, 1, 1), "Улица 2", "987654321", 2, "Группа Б");
        Student student3 = new Student("Сидоров", "Сидор", "Сидорович", new DateTime(2001, 1, 1), "Улица 3", "456789123", 1, "Группа А");

        var facultyStudents = new List<Student> { student1, student2, student3 };

        PrintStudentsByGroup(facultyStudents, "Группа А");
        PrintStudentsByGroup(facultyStudents, "Группа Б");

        Student.PrintClassInfo();
    }

    private static void PrintStudentsByGroup(List<Student> students, string group)
    {
        var groupStudents = students.Where(s => s.Group == group).ToList(); // s => s.Group == group: Лямбда-выражение, которое проверяет, соответствует ли группа студента (s.Group) переданному имени группы (group).
        Console.WriteLine($"Студенты группы {group}:");
        foreach (var student in groupStudents)
        {
            Console.WriteLine(student);
        }
        Console.WriteLine();
    }
}


/*public class Program
{
    public static void Main()
    {
        Student student1 = new Student("Иванов", "Иван", "Иванович", new DateTime(2000, 1, 1),
                                        "Улица 1", "123456789", 1, "Группа А");
        Student student2 = new Student("Петров", "Петр", "Петрович", new DateTime(1999, 1, 1),
                                        "Улица 2", "987654321", 2, "Группа Б");
        Student student3 = new Student("Сидоров", "Сидор");

        Console.WriteLine($"Студент 1: {student1}");
        Console.WriteLine($"Студент 2: {student2}");
        Console.WriteLine($"Студент 3: {student3}");

        student3.Address = "Улица 3";
        student3.Phone = "456789123";
        student3.Course = 1;

        string fullName;
        int age = 0;
        student3.GetStudentInfo(out fullName, ref age);
        Console.WriteLine($"ФИО: {fullName}, Возраст: {age}, Адрес: {student3.Address}, Телефон: {student3.Phone}");

        Console.WriteLine($"Сравнение student1 и student2: {student1.Equals(student2)}");
        Console.WriteLine($"Сравнение student1 и student3: {student1.Equals(student3)}");

        Console.WriteLine($"Тип student1: {student1.GetType()}");
        Console.WriteLine($"Тип student2: {student2.GetType()}");
        Console.WriteLine($"Тип student3: {student3.GetType()}");

        Student.PrintClassInfo();
    }
}*/


// ex 3
/*public class Program
{
    public static void Main()
    {
        // массив объектов Student
        Student[] students = new Student[]
        {
            new Student("Иванов", "Иван", "Иванович", new DateTime(2000, 1, 1),
                        "Улица 1", "123456789", 1, "Группа А"),
            new Student("Петров", "Петр", "Петрович", new DateTime(1999, 1, 1),
                        "Улица 2", "987654321", 2, "Группа Б"),
            new Student("Сидоров", "Сидор", "Сидорович", new DateTime(2001, 1, 1),
                        "Улица 3", "456789123", 1, "Группа А"),
            new Student("Алексеев", "Алексей", "Алексеевич", new DateTime(2000, 5, 5),
                        "Улица 4", "321654987", 2, "Группа Б"),
            new Student("Николаев", "Николай", "Николаевич", new DateTime(1998, 3, 15),
                        "Улица 5", "654987321", 3, "Группа А")
        };

        // факультет
        string faculty = "Computer Science";
        Console.WriteLine($"Студенты факультета {faculty}:");
        foreach (var student in students)
        {
            Console.WriteLine(student);
        }

        // группа
        string groupToShow = "Группа А";
        Console.WriteLine($"\nСтуденты группы {groupToShow}:");
        foreach (var student in students.Where(s => s.Group == groupToShow))
        {
            Console.WriteLine(student);
        }

        // еще группа
        groupToShow = "Группа Б";
        Console.WriteLine($"\nСтуденты группы {groupToShow}:");
        foreach (var student in students.Where(s => s.Group == groupToShow))
        {
            Console.WriteLine(student);
        }

        Student.PrintClassInfo();
    }
}
*/

// ex 4
/*public class Program
{
    public static void Main()
    {
        // анонимный тип
        var student = new
        {
            Id = 1,
            LastName = "Иванов",
            FirstName = "Иван",
            Patronymic = "Иванович",
            DateOfBirth = new DateTime(2000, 1, 1),
            Address = "Улица 1",
            Phone = "123456789",
            Course = 1,
            Group = "Группа А",
            Faculty = "Computer Science"
        };

        Console.WriteLine($"ID: {student.Id}");
        Console.WriteLine($"Фамилия: {student.LastName}");
        Console.WriteLine($"Имя: {student.FirstName}");
        Console.WriteLine($"Отчество: {student.Patronymic}");
        Console.WriteLine($"Дата рождения: {student.DateOfBirth.ToShortDateString()}");
        Console.WriteLine($"Адрес: {student.Address}");
        Console.WriteLine($"Телефон: {student.Phone}");
        Console.WriteLine($"Курс: {student.Course}");
        Console.WriteLine($"Группа: {student.Group}");
        Console.WriteLine($"Факультет: {student.Faculty}");
    }
}*/

