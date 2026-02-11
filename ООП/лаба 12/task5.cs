using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

public class AVEFileManager
{
    public static void InspectDrive(string driveName)
    {
        try
        {
            DriveInfo drive = new DriveInfo(driveName);
            if (!drive.IsReady)
            {
                Console.WriteLine($"Диск {driveName} не доступен.");
                return;
            }

            // Создаем директорий XXXInspect
            string inspectDir = Path.Combine(driveName, "AVEInspect");
            Directory.CreateDirectory(inspectDir);
            AVELog.WriteLog("InspectDrive", $"Создана директория: {inspectDir}");

            // Создаем текстовый файл с информацией о директориях и файлах
            string infoFile = Path.Combine(inspectDir, "avedirinfo.txt");

            using (StreamWriter writer = new StreamWriter(infoFile))
            {
                writer.WriteLine($"Информация о диске {driveName}");
                writer.WriteLine($"=================================");
                writer.WriteLine($"Свободное место: {drive.AvailableFreeSpace} байт");
                writer.WriteLine($"Общий размер: {drive.TotalSize} байт");
                writer.WriteLine($"Файловая система: {drive.DriveFormat}");
                writer.WriteLine();

                // Записываем список директорий (первые 10)
                writer.WriteLine("Директории (первые 10):");
                var dirs = Directory.GetDirectories(driveName).Take(10);
                foreach (var dir in dirs)
                {
                    writer.WriteLine($"  {dir}");
                }
                writer.WriteLine();

                // Записываем список файлов (первые 10)
                writer.WriteLine("Файлы (первые 10):");
                var files = Directory.GetFiles(driveName).Take(10);
                foreach (var file in files)
                {
                    writer.WriteLine($"  {file}");
                }
            }

            AVELog.WriteLog("InspectDrive", $"Создан файл: {infoFile}");

            // Создаем копию файла
            string copiedFile = Path.Combine(inspectDir, "avedirinfo_copy.txt");
            File.Copy(infoFile, copiedFile, true);
            AVELog.WriteLog("InspectDrive", $"Создана копия файла: {copiedFile}");

            // Переименовываем копию
            string renamedFile = Path.Combine(inspectDir, "avedirinfo_renamed.txt");
            File.Move(copiedFile, renamedFile);
            AVELog.WriteLog("InspectDrive", $"Файл переименован: {renamedFile}");

            // Удаляем первоначальный файл
            File.Delete(infoFile);
            AVELog.WriteLog("InspectDrive", $"Удален файл: {infoFile}");

            Console.WriteLine($"Операции с диском {driveName} завершены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            AVELog.WriteLog("InspectDrive_ERROR", ex.Message);
        }
    }

    public static void CopyFiles(string sourceDir, string extension, string destDir)
    {
        try
        {
            // Создаем директорий XXXFiles
            string filesDir = Path.Combine(destDir, "AVEFiles");
            Directory.CreateDirectory(filesDir);
            AVELog.WriteLog("CopyFiles", $"Создана директория: {filesDir}");

            // Копируем файлы с заданным расширением
            var files = Directory.GetFiles(sourceDir, $"*.{extension}");

            Console.WriteLine($"Найдено {files.Length} файлов с расширением .{extension}");

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(filesDir, fileName);
                File.Copy(file, destFile, true);
                AVELog.WriteLog("CopyFiles", $"Скопирован файл: {fileName}");
            }

            // Перемещаем AVEFiles в AVEInspect
            string targetDir = Path.Combine(Path.GetDirectoryName(destDir), "AVEInspect", "AVEFiles");
            if (Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }

            Directory.Move(filesDir, targetDir);
            AVELog.WriteLog("CopyFiles", $"Директория перемещена в: {targetDir}");

            Console.WriteLine($"Скопировано {files.Length} файлов в {targetDir}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            AVELog.WriteLog("CopyFiles_ERROR", ex.Message);
        }
    }

    public static void CreateArchive(string sourceDir, string archivePath)
    {
        try
        {
            ZipFile.CreateFromDirectory(sourceDir, archivePath);
            AVELog.WriteLog("CreateArchive", $"Создан архив: {archivePath}");
            Console.WriteLine($"Архив создан: {archivePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании архива: {ex.Message}");
            AVELog.WriteLog("CreateArchive_ERROR", ex.Message);
        }
    }

    public static void ExtractArchive(string archivePath, string extractDir)
    {
        try
        {
            if (!Directory.Exists(extractDir))
            {
                Directory.CreateDirectory(extractDir);
            }

            ZipFile.ExtractToDirectory(archivePath, extractDir, true);
            AVELog.WriteLog("ExtractArchive", $"Архив распакован в: {extractDir}");
            Console.WriteLine($"Архив распакован в: {extractDir}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при распаковке архива: {ex.Message}");
            AVELog.WriteLog("ExtractArchive_ERROR", ex.Message);
        }
    }
}