using System;
using System.IO;

public class AVEFileInfo
{
    public static void GetFileInfo(string filePath)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                Console.WriteLine($"Информация о файле: {filePath}");
                Console.WriteLine($"Полный путь: {fileInfo.FullName}");
                Console.WriteLine($"Размер: {fileInfo.Length} байт");
                Console.WriteLine($"Расширение: {fileInfo.Extension}");
                Console.WriteLine($"Имя: {fileInfo.Name}");
                Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
                Console.WriteLine($"Дата изменения: {fileInfo.LastWriteTime}");
                Console.WriteLine($"Дата последнего доступа: {fileInfo.LastAccessTime}");

                AVELog.WriteLog("GetFileInfo",
                    $"Файл: {fileInfo.Name}, Размер: {fileInfo.Length}, Путь: {fileInfo.DirectoryName}");
            }
            else
            {
                Console.WriteLine($"Файл {filePath} не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}