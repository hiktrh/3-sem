using System;
using System.IO;
using System.Linq;
using System.IO.Compression;
public class AVELog
{
    private const string LogFileName = "avelogfile.txt";

    public static void WriteLog(string action, string details)
    {
        try
        {
            string logEntry = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] Действие: {action} | Детали: {details}";

            using (StreamWriter writer = new StreamWriter(LogFileName, true))
            {
                writer.WriteLine(logEntry);
            }

            Console.WriteLine($"Запись в лог выполнена: {logEntry}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в лог: {ex.Message}");
        }
    }

    public static void ReadLog()
    {
        try
        {
            if (File.Exists(LogFileName))
            {
                using (StreamReader reader = new StreamReader(LogFileName))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine("Содержимое лог-файла:");
                    Console.WriteLine(content);
                }
            }
            else
            {
                Console.WriteLine("Лог-файл не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении лога: {ex.Message}");
        }
    }

    public static void SearchLogByKeyword(string keyword)
    {
        try
        {
            if (File.Exists(LogFileName))
            {
                var lines = File.ReadAllLines(LogFileName)
                    .Where(line => line.Contains(keyword, StringComparison.OrdinalIgnoreCase));

                Console.WriteLine($"Найдено {lines.Count()} записей с ключевым словом '{keyword}':");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске в логе: {ex.Message}");
        }
    }

    public static void SearchLogByDate(DateTime date)
    {
        try
        {
            if (File.Exists(LogFileName))
            {
                string dateStr = date.ToString("dd.MM.yyyy");
                var lines = File.ReadAllLines(LogFileName)
                    .Where(line => line.Contains(dateStr));

                Console.WriteLine($"Найдено {lines.Count()} записей за {dateStr}:");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске по дате: {ex.Message}");
        }
    }

    public static int GetLogEntriesCount()
    {
        try
        {
            if (File.Exists(LogFileName))
            {
                return File.ReadAllLines(LogFileName).Length;
            }
            return 0;
        }
        catch
        {
            return -1;
        }
    }

    public static void KeepOnlyCurrentHourEntries()
    {
        try
        {
            if (File.Exists(LogFileName))
            {
                string currentHour = DateTime.Now.ToString("dd.MM.yyyy HH");
                var lines = File.ReadAllLines(LogFileName)
                    .Where(line => line.Contains(currentHour))
                    .ToArray();

                File.WriteAllLines(LogFileName, lines);
                Console.WriteLine($"В логе оставлены только записи за текущий час: {currentHour}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при фильтрации лога: {ex.Message}");
        }
    }
}