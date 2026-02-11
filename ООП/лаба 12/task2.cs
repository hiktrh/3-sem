using System;
using System.IO;

public class AVEDiskInfo
{
    public static void ShowFreeSpace(string driveName)
    {
        try
        {
            DriveInfo drive = new DriveInfo(driveName);
            if (drive.IsReady)
            {
                Console.WriteLine($"Свободное место на диске {driveName}: {drive.AvailableFreeSpace / (1024 * 1024 * 1024):F2} GB");
                AVELog.WriteLog("ShowFreeSpace", $"Диск: {driveName}, Свободно: {drive.AvailableFreeSpace} байт");
            }
            else
            {
                Console.WriteLine($"Диск {driveName} не готов.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public static void ShowFileSystemInfo(string driveName)
    {
        try
        {
            DriveInfo drive = new DriveInfo(driveName);
            if (drive.IsReady)
            {
                Console.WriteLine($"Файловая система диска {driveName}: {drive.DriveFormat}");
                AVELog.WriteLog("ShowFileSystemInfo", $"Диск: {driveName}, ФС: {drive.DriveFormat}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public static void ShowAllDrivesInfo()
    {
        try
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            Console.WriteLine("Информация о всех дисках:");
            Console.WriteLine("=================================");

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Имя диска: {drive.Name}");
                    Console.WriteLine($"Метка тома: {drive.VolumeLabel}");
                    Console.WriteLine($"Общий объем: {drive.TotalSize / (1024 * 1024 * 1024):F2} GB");
                    Console.WriteLine($"Доступный объем: {drive.AvailableFreeSpace / (1024 * 1024 * 1024):F2} GB");
                    Console.WriteLine($"Тип диска: {drive.DriveType}");
                    Console.WriteLine($"Файловая система: {drive.DriveFormat}");
                    Console.WriteLine("---------------------------------");

                    AVELog.WriteLog("ShowAllDrivesInfo",
                        $"Диск: {drive.Name}, Метка: {drive.VolumeLabel}, Общий объем: {drive.TotalSize}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}