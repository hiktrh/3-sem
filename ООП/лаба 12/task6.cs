using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная работа №12: Работа с потоковыми классами ===");

        // 1. Демонстрация работы с логом
        Console.WriteLine("\n1. Работа с классом AVELog:");
        AVELog.WriteLog("Запуск программы", "Программа начала выполнение");

        // 2. Демонстрация работы с дисками
        Console.WriteLine("\n2. Работа с классом AVEDiskInfo:");
        AVEDiskInfo.ShowFreeSpace("C:");
        AVEDiskInfo.ShowFileSystemInfo("C:");
        AVEDiskInfo.ShowAllDrivesInfo();

        // 3. Демонстрация работы с файлами
        Console.WriteLine("\n3. Работа с классом AVEFileInfo:");
        string sampleFile = "sample.txt";
        File.WriteAllText(sampleFile, "Это тестовый файл для демонстрации.");
        AVEFileInfo.GetFileInfo(sampleFile);

        // 4. Демонстрация работы с директориями
        Console.WriteLine("\n4. Работа с классом AVEDirInfo:");
        AVEDirInfo.GetDirectoryInfo(".");

        // 5. Демонстрация работы с файловым менеджером
        Console.WriteLine("\n5. Работа с классом AVEFileManager:");

        // Часть A: Инспекция диска
        AVEFileManager.InspectDrive("C:");

        // Часть B: Копирование файлов
        string tempDir = Path.Combine(Path.GetTempPath(), "AVETest");
        Directory.CreateDirectory(tempDir);
        File.WriteAllText(Path.Combine(tempDir, "test1.txt"), "Файл 1");
        File.WriteAllText(Path.Combine(tempDir, "test2.txt"), "Файл 2");
        File.WriteAllText(Path.Combine(tempDir, "test3.doc"), "Файл 3");

        AVEFileManager.CopyFiles(tempDir, "txt", "C:\\AVEInspect");

        // Часть C: Архивирование
        string sourceForArchive = "C:\\AVEInspect\\AVEFiles";
        if (Directory.Exists(sourceForArchive))
        {
            string archivePath = "C:\\AVEInspect\\archive.zip";
            AVEFileManager.CreateArchive(sourceForArchive, archivePath);

            string extractDir = "C:\\AVEInspect\\Extracted";
            AVEFileManager.ExtractArchive(archivePath, extractDir);
        }

        // 6. Работа с лог-файлом
        Console.WriteLine("\n6. Анализ лог-файла:");

        int logCount = AVELog.GetLogEntriesCount();
        Console.WriteLine($"Всего записей в логе: {logCount}");

        Console.WriteLine("\nПоиск записей с ключевым словом 'файл':");
        AVELog.SearchLogByKeyword("файл");

        Console.WriteLine("\nПоиск записей за сегодня:");
        AVELog.SearchLogByDate(DateTime.Today);

        // Оставляем только записи за текущий час
        AVELog.KeepOnlyCurrentHourEntries();

        Console.WriteLine("\nОчистка временных файлов...");
        try
        {
            if (File.Exists(sampleFile))
                File.Delete(sampleFile);

            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
        }
        catch { }

        AVELog.WriteLog("Завершение программы", "Программа завершила выполнение");

        Console.WriteLine("\n=== Работа программы завершена ===");
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}