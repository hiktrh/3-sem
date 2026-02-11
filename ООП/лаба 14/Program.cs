using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Timer = System.Timers.Timer;  
using ThreadingTimer = System.Threading.Timer; 

namespace ThreadingLab
{
    class Program
    {
        static object fileLock = new object();
        static int sharedCounter = 0;
        static object counterLock = new object();
        static Semaphore semaphore = new Semaphore(1, 1); // Для синхронизации
        static ManualResetEventSlim mre = new ManualResetEventSlim(false);
        static bool stopThreads = false;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №14: Работа с потоками ===\n");

            // 1. Информация о процессах
            Task1_ProcessInfo();

            // 2. Работа с доменами приложений
            Task2_AppDomains();

            // 3. Поток для поиска простых чисел
            Task3_PrimeNumbersThread();

            // 4. Два потока: четные и нечетные числа
            Task4_EvenOddThreads();

            // 5. Класс Timer
            Task5_TimerExample();

            Console.WriteLine("\n=== Все задачи выполнены ===");
            Console.ReadKey();
        }

        #region Задание 1: Информация о процессах
        static void Task1_ProcessInfo()
        {
            Console.WriteLine("=== 1. Информация о запущенных процессах ===\n");

            try
            {
                Process[] processes = Process.GetProcesses();

                using (StreamWriter writer = new StreamWriter("processes_info.txt"))
                {
                    writer.WriteLine($"Всего процессов: {processes.Length}");
                    writer.WriteLine($"Время сбора информации: {DateTime.Now}");
                    writer.WriteLine(new string('-', 100));

                    Console.WriteLine($"Всего процессов: {processes.Length}");
                    Console.WriteLine("Топ-10 процессов по использованию CPU:\n");

                    // Сортируем по использованию процессора и берем топ-10
                    List<Process> topProcesses = new List<Process>();
                    foreach (var process in processes)
                    {
                        try
                        {
                            if (process.Id > 0) // Исключаем системные
                                topProcesses.Add(process);
                        }
                        catch { }
                    }

                    // Выводим информацию
                    int count = 0;
                    foreach (var process in processes)
                    {
                        try
                        {
                            if (count++ >= 20) break; // Ограничиваем вывод

                            writer.WriteLine($"ID: {process.Id}");
                            writer.WriteLine($"Имя: {process.ProcessName}");
                            writer.WriteLine($"Приоритет: {process.BasePriority}");

                            if (process.StartTime != DateTime.MinValue)
                                writer.WriteLine($"Время запуска: {process.StartTime}");

                            writer.WriteLine($"Состояние: {(process.Responding ? "Работает" : "Не отвечает")}");

                            if (process.TotalProcessorTime != TimeSpan.MinValue)
                                writer.WriteLine($"Общее время CPU: {process.TotalProcessorTime}");

                            writer.WriteLine($"Память: {process.WorkingSet64 / 1024 / 1024} MB");
                            writer.WriteLine(new string('-', 50));

                            // Вывод в консоль только для некоторых процессов
                            if (count <= 10)
                            {
                                Console.WriteLine($"{process.ProcessName,-30} ID: {process.Id,-8} " +
                                                  $"CPU: {process.TotalProcessorTime.TotalSeconds:F1}с " +
                                                  $"Mem: {process.WorkingSet64 / 1024 / 1024}MB");
                            }
                        }
                        catch (Exception ex)
                        {
                            writer.WriteLine($"Ошибка при получении информации: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine($"\nПолная информация сохранена в файл: processes_info.txt");

                // Информация о текущем процессе
                Process currentProcess = Process.GetCurrentProcess();
                Console.WriteLine($"\nТекущий процесс:");
                Console.WriteLine($"  ID: {currentProcess.Id}");
                Console.WriteLine($"  Имя: {currentProcess.ProcessName}");
                Console.WriteLine($"  Приоритет: {currentProcess.BasePriority}");
                Console.WriteLine($"  Память: {currentProcess.WorkingSet64 / 1024 / 1024} MB");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        #endregion

        #region Задание 2: Домены приложений
        static void Task2_AppDomains()
        {
            Console.WriteLine("\n=== 2. Работа с доменами приложений ===\n");

            // Текущий домен
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Console.WriteLine($"Текущий домен:");
            Console.WriteLine($"  Имя: {currentDomain.FriendlyName}");
            Console.WriteLine($"  ID: {currentDomain.Id}");
            Console.WriteLine($"  Базовый каталог: {currentDomain.BaseDirectory}");

            // Конфигурация
            Console.WriteLine($"\nДетали конфигурации:");
            Console.WriteLine($"  Setup Information: {currentDomain.SetupInformation}");

            // Загруженные сборки
            Console.WriteLine($"\nЗагруженные сборки ({currentDomain.GetAssemblies().Length}):");
            foreach (var assembly in currentDomain.GetAssemblies())
            {
                Console.WriteLine($"  - {assembly.GetName().Name} (v{assembly.GetName().Version})");
            }

            // Создание нового домена
            try
            {
                Console.WriteLine("\nСоздание нового домена...");
                AppDomain newDomain = AppDomain.CreateDomain("NewTestDomain");

                Console.WriteLine($"Новый домен создан: {newDomain.FriendlyName}");

                // Загрузка сборки в новый домен
                string assemblyPath = typeof(Program).Assembly.Location;
                newDomain.Load(AssemblyName.GetAssemblyName(assemblyPath));

                Console.WriteLine("Сборка загружена в новый домен");

                // Выгрузка домена
                AppDomain.Unload(newDomain);
                Console.WriteLine("Домен выгружен");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с доменами: {ex.Message}");
            }
        }
        #endregion

        #region Задание 3: Поток для простых чисел
        static void Task3_PrimeNumbersThread()
        {
            Console.WriteLine("\n=== 3. Поток для расчета простых чисел ===\n");

            Console.Write("Введите n (максимальное число для поиска простых чисел): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                n = 100;
                Console.WriteLine($"Используется значение по умолчанию: {n}");
            }

            // Создание и настройка потока
            Thread primeThread = new Thread(() => FindPrimes(n))
            {
                Name = "PrimeNumbersThread",
                Priority = ThreadPriority.Normal,
                IsBackground = true
            };

            Console.WriteLine($"\nСоздан поток: {primeThread.Name}");
            Console.WriteLine($"Приоритет: {primeThread.Priority}");
            Console.WriteLine($"Состояние перед запуском: {primeThread.ThreadState}");

            // Запуск потока
            primeThread.Start();
            Thread.Sleep(100); // Даем немного времени на старт

            Console.WriteLine($"Состояние после запуска: {primeThread.ThreadState}");
            Console.WriteLine($"ID потока: {primeThread.ManagedThreadId}");

            // Даем потоку поработать немного
            Thread.Sleep(500);

            // Приостановка (Deprecated в .NET, но покажем альтернативу)
            Console.WriteLine("\nИмитация паузы...");
            mre.Reset(); // Сигнализируем потоку о паузе
            Thread.Sleep(1000);
            Console.WriteLine($"Состояние во время паузы: {primeThread.ThreadState}");

            // Возобновление
            Console.WriteLine("Возобновление работы...");
            mre.Set();

            // Ждем завершения потока
            Console.WriteLine("Ожидание завершения потока...");
            primeThread.Join();

            Console.WriteLine($"Состояние после завершения: {primeThread.ThreadState}");
            Console.WriteLine("Поиск простых чисел завершен!");
        }

        static void FindPrimes(int n)
        {
            string fileName = "prime_numbers.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"Простые числа от 1 до {n}");
                    writer.WriteLine(new string('=', 30));

                    Console.WriteLine($"Простые числа от 1 до {n}:");

                    for (int i = 2; i <= n; i++)
                    {
                        // Проверка на паузу
                        if (!mre.IsSet)
                        {
                            mre.Wait();
                        }

                        if (IsPrime(i))
                        {
                            lock (fileLock)
                            {
                                writer.WriteLine(i);
                            }
                            Console.Write($"{i} ");
                            Thread.Sleep(50); // Имитация задержки расчета
                        }

                        // Проверка на остановку
                        if (stopThreads) break;
                    }
                }

                Console.WriteLine($"\nРезультаты сохранены в файл: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в потоке поиска простых чисел: {ex.Message}");
            }
        }

        static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
        #endregion

        #region Задание 4: Четные и нечетные числа
        static void Task4_EvenOddThreads()
        {
            Console.WriteLine("\n=== 4. Два потока: четные и нечетные числа ===\n");

            Console.Write("Введите n (максимальное число): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                n = 20;
                Console.WriteLine($"Используется значение по умолчанию: {n}");
            }

            stopThreads = false;
            sharedCounter = 0;

            // Создание потоков
            Thread evenThread = new Thread(() => PrintNumbers(n, true))
            {
                Name = "EvenThread",
                Priority = ThreadPriority.BelowNormal // Меньший приоритет для четных
            };

            Thread oddThread = new Thread(() => PrintNumbers(n, false))
            {
                Name = "OddThread",
                Priority = ThreadPriority.AboveNormal // Больший приоритет для нечетных
            };

            Console.WriteLine($"\nПоток четных чисел: {evenThread.Name}, Приоритет: {evenThread.Priority}");
            Console.WriteLine($"Поток нечетных чисел: {oddThread.Name}, Приоритет: {oddThread.Priority}");

            // 4a. Меняем приоритет
            Console.WriteLine("\n4a. Меняем приоритеты...");
            evenThread.Priority = ThreadPriority.Normal;
            oddThread.Priority = ThreadPriority.Normal;
            Console.WriteLine($"Новый приоритет четных: {evenThread.Priority}");
            Console.WriteLine($"Новый приоритет нечетных: {oddThread.Priority}");

            // Запуск потоков без синхронизации
            Console.WriteLine("\nЗапуск потоков БЕЗ синхронизации:");
            evenThread.Start();
            oddThread.Start();

            evenThread.Join();
            oddThread.Join();

            // 4b.i. Сначала четные, потом нечетные
            Console.WriteLine("\n4b.i. Сначала четные, потом нечетные:");
            ResetForSync(n);

            Thread evenFirst = new Thread(() => PrintEvenFirst(n));
            Thread oddAfter = new Thread(() => PrintOddAfter(n));

            evenFirst.Start();
            oddAfter.Start();

            evenFirst.Join();
            oddAfter.Join();

            // 4b.ii. Поочередно: четное, нечетное
            Console.WriteLine("\n4b.ii. Поочередно: четное, нечетное:");
            ResetForSync(n);

            Thread evenAlternate = new Thread(() => PrintAlternate(n, true));
            Thread oddAlternate = new Thread(() => PrintAlternate(n, false));

            evenAlternate.Start();
            oddAlternate.Start();

            evenAlternate.Join();
            oddAlternate.Join();
        }

        static void PrintNumbers(int n, bool isEven)
        {
            string threadType = isEven ? "ЧЕТНЫЕ" : "НЕЧЕТНЫЕ";
            string fileName = isEven ? "even_numbers.txt" : "odd_numbers.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine($"\n=== {threadType} числа (поток: {Thread.CurrentThread.Name}) ===");

                    int start = isEven ? 2 : 1;
                    int step = 2;

                    for (int i = start; i <= n && !stopThreads; i += step)
                    {
                        // Запись в файл
                        lock (fileLock)
                        {
                            writer.WriteLine($"{i} (поток: {Thread.CurrentThread.Name})");
                        }

                        // Вывод в консоль
                        Console.WriteLine($"{threadType}: {i} (поток: {Thread.CurrentThread.Name})");

                        // Разная скорость расчета
                        Thread.Sleep(isEven ? 100 : 150);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в потоке {threadType}: {ex.Message}");
            }
        }

        static void PrintEvenFirst(int n)
        {
            // Используем семафор для синхронизации
            Console.WriteLine("Четный поток: жду разрешения...");
            semaphore.WaitOne();

            try
            {
                Console.WriteLine("Четный поток: начал работу");
                using (StreamWriter writer = new StreamWriter("sync_numbers.txt", true))
                {
                    writer.WriteLine("\n=== Сначала четные ===");

                    for (int i = 2; i <= n; i += 2)
                    {
                        writer.WriteLine($"Четное: {i}");
                        Console.WriteLine($"Четное: {i}");
                        Thread.Sleep(100);
                    }
                }
            }
            finally
            {
                semaphore.Release();
                Console.WriteLine("Четный поток: завершил работу");
            }
        }

        static void PrintOddAfter(int n)
        {
            Console.WriteLine("Нечетный поток: жду разрешения...");
            semaphore.WaitOne();

            try
            {
                Console.WriteLine("Нечетный поток: начал работу");
                using (StreamWriter writer = new StreamWriter("sync_numbers.txt", true))
                {
                    writer.WriteLine("\n=== Потом нечетные ===");

                    for (int i = 1; i <= n; i += 2)
                    {
                        writer.WriteLine($"Нечетное: {i}");
                        Console.WriteLine($"Нечетное: {i}");
                        Thread.Sleep(100);
                    }
                }
            }
            finally
            {
                semaphore.Release();
                Console.WriteLine("Нечетный поток: завершил работу");
            }
        }

        static void PrintAlternate(int n, bool isEven)
        {
            string type = isEven ? "ЧЕТНЫЙ" : "НЕЧЕТНЫЙ";

            while (sharedCounter <= n)
            {
                lock (counterLock)
                {
                    if ((isEven && sharedCounter % 2 == 0) || (!isEven && sharedCounter % 2 != 0))
                    {
                        if (sharedCounter > 0 && sharedCounter <= n)
                        {
                            using (StreamWriter writer = new StreamWriter("alternate_numbers.txt", true))
                            {
                                string numType = sharedCounter % 2 == 0 ? "Четное" : "Нечетное";
                                writer.WriteLine($"{numType}: {sharedCounter}");
                                Console.WriteLine($"{numType}: {sharedCounter} (поток: {type})");
                            }
                        }
                        sharedCounter++;
                    }
                }
                Thread.Sleep(50);
            }
        }

        static void ResetForSync(int n)
        {
            sharedCounter = 1;
            stopThreads = false;

            // Очищаем файлы
            File.WriteAllText("sync_numbers.txt", "");
            File.WriteAllText("alternate_numbers.txt", "");
        }
        #endregion

        #region Задание 5: Класс Timer
        static void Task5_TimerExample()
        {
            Console.WriteLine("\n=== 5. Пример использования класса Timer ===\n");

            Console.WriteLine("Создаем таймер для периодической задачи...");

            // Создаем System.Timers.Timer
            Timer timer = new Timer(1000); // 1 секунда - используем псевдоним
            int tickCount = 0;

            timer.Elapsed += (sender, e) =>
            {
                tickCount++;
                Console.WriteLine($"Таймер сработал #{tickCount} в {e.SignalTime:T}");

                // Пример задачи: запись в лог
                using (StreamWriter writer = new StreamWriter("timer_log.txt", true))
                {
                    writer.WriteLine($"[{DateTime.Now:T}] Тик #{tickCount}");
                }

                // Останавливаем после 5 тиков
                if (tickCount >= 5)
                {
                    Console.WriteLine("Останавливаем таймер после 5 тиков");
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.AutoReset = true;
            timer.Enabled = true;

            Console.WriteLine("Таймер запущен. Ожидаем 5 тиков...");

            // Ждем завершения
            while (tickCount < 5)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("Таймер остановлен. Лог сохранен в timer_log.txt");

            // Другой пример: System.Threading.Timer
            Console.WriteLine("\nДругой пример: ThreadingTimer (System.Threading.Timer)");

            int threadTimerCount = 0;
            TimerCallback callback = state =>
            {
                threadTimerCount++;
                Console.WriteLine($"Threading Timer тик #{threadTimerCount}");

                if (threadTimerCount >= 3)
                {
                    Console.WriteLine("Threading Timer остановлен");
                    (state as ThreadingTimer)?.Dispose();
                }
            };

            using (ThreadingTimer threadingTimer = new ThreadingTimer(callback, null, 0, 500))
            {
                // Ждем завершения
                while (threadTimerCount < 3)
                {
                    Thread.Sleep(100);
                }
            }
        }
        #endregion
    }
}