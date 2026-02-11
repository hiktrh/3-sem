using System;
using System.IO;
using System.Linq;

public class AVEDirInfo
{
    public static void GetDirectoryInfo(string dirPath)
    {
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

            if (dirInfo.Exists)
            {
                Console.WriteLine($"Информация о директории: {dirPath}");
                Console.WriteLine($"Количество файлов: {dirInfo.GetFiles().Length}");
                Console.WriteLine($"Время создания: {dirInfo.CreationTime}");
                Console.WriteLine($"Количество поддиректорий: {dirInfo.GetDirectories().Length}");
                Console.WriteLine($"Родительская директория: {dirInfo.Parent?.FullName}");

                // Список родительских директорий
                Console.WriteLine("Список родительских директорий:");
                DirectoryInfo current = dirInfo;
                while (current.Parent != null)
                {
                    Console.WriteLine($"  - {current.Parent.FullName}");
                    current = current.Parent;
                }

                AVELog.WriteLog("GetDirectoryInfo",
                    $"Директория: {dirInfo.Name}, Файлов: {dirInfo.GetFiles().Length}, Поддиректорий: {dirInfo.GetDirectories().Length}");
            }
            else
            {
                Console.WriteLine($"Директория {dirPath} не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}