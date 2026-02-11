using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ParallelProgrammingLab
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №15: Платформа параллельных вычислений ===\n");

            // 1. Длительная задача - перемножение матриц
            await Task1_MatrixMultiplication();

            // 2. Задача с токеном отмены
            Task2_CancellationToken();

            // 3. Три задачи с возвратом результата для четвертой задачи
            Task3_MultipleTasksWithResult();

            // 4. Задачи продолжения
            Task4_ContinuationTasks();

            // 5. Класс Parallel - распараллеливание циклов
            Task5_ParallelLoops();

            // 6. Parallel.Invoke
            Task6_ParallelInvoke();

            // 7. BlockingCollection - поставщики и покупатели
            Task7_BlockingCollection();

            // 8. Async/Await
            await Task8_AsyncAwait();

            Console.WriteLine("\n=== Все задачи выполнены ===");
        }

        #region Задание 1: Длительная задача - перемножение матриц
        static async Task Task1_MatrixMultiplication()
        {
            Console.WriteLine("=== 1. Длительная задача - перемножение матриц ===\n");

            int size = 500; // Размер матрицы

            // 1.1 Создаем и запускаем задачу
            Console.WriteLine($"Создание задачи для перемножения матриц {size}x{size}...");
            Task task = Task.Run(() => MultiplyMatrices(size));

            Console.WriteLine($"Идентификатор задачи: {task.Id}");
            Console.WriteLine($"Статус после запуска: {task.Status}");

            // 1.2 Проверяем статус во время выполнения
            await Task.Delay(100);
            Console.WriteLine($"Статус через 100 мс: {task.Status}");

            // 1.3 Оценка производительности
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Параллельная версия
            Console.WriteLine("\nПараллельное перемножение матриц:");
            stopwatch.Restart();
            await MultiplyMatricesParallelAsync(size);
            stopwatch.Stop();
            Console.WriteLine($"Время параллельного выполнения: {stopwatch.ElapsedMilliseconds} мс");

            // Последовательная версия (для сравнения)
            Console.WriteLine("\nПоследовательное перемножение матриц:");
            stopwatch.Restart();
            MultiplyMatricesSequential(size);
            stopwatch.Stop();
            Console.WriteLine($"Время последовательного выполнения: {stopwatch.ElapsedMilliseconds} мс");

            // Ждем завершения первой задачи
            await task;
            Console.WriteLine($"Статус после завершения: {task.Status}");
        }

        static void MultiplyMatrices(int size)
        {
            Random rand = new Random();
            double[,] matrixA = new double[size, size];
            double[,] matrixB = new double[size, size];
            double[,] result = new double[size, size];

            // Инициализация матриц случайными значениями
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrixA[i, j] = rand.NextDouble() * 100;
                    matrixB[i, j] = rand.NextDouble() * 100;
                }
            }

            // Перемножение матриц (упрощенный алгоритм)
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < size; k++)
                    {
                        sum += matrixA[i, k] * matrixB[k, j];
                    }
                    result[i, j] = sum;
                }

                // Имитация длительной операции
                if (i % 50 == 0)
                {
                    Thread.Sleep(10);
                }
            }

            Console.WriteLine($"Перемножение матриц {size}x{size} завершено");
        }

        static async Task MultiplyMatricesParallelAsync(int size)
        {
            await Task.Run(() =>
            {
                Random rand = new Random();
                double[,] matrixA = new double[size, size];
                double[,] matrixB = new double[size, size];
                double[,] result = new double[size, size];

                // Инициализация
                Parallel.For(0, size, i =>
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrixA[i, j] = rand.NextDouble() * 100;
                        matrixB[i, j] = rand.NextDouble() * 100;
                    }
                });

                // Параллельное перемножение
                Parallel.For(0, size, i =>
                {
                    for (int j = 0; j < size; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < size; k++)
                        {
                            sum += matrixA[i, k] * matrixB[k, j];
                        }
                        result[i, j] = sum;
                    }
                });
            });
        }

        static void MultiplyMatricesSequential(int size)
        {
            Random rand = new Random();
            double[,] matrixA = new double[size, size];
            double[,] matrixB = new double[size, size];
            double[,] result = new double[size, size];

            // Инициализация
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrixA[i, j] = rand.NextDouble() * 100;
                    matrixB[i, j] = rand.NextDouble() * 100;
                }
            }

            // Перемножение
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < size; k++)
                    {
                        sum += matrixA[i, k] * matrixB[k, j];
                    }
                    result[i, j] = sum;
                }
            }
        }
        #endregion

        #region Задание 2: Задача с токеном отмены
        static void Task2_CancellationToken()
        {
            Console.WriteLine("\n=== 2. Задача с токеном отмены ===\n");

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Console.WriteLine("Запуск длительной задачи...");

            Task longTask = Task.Run(() =>
            {
                Console.WriteLine("Задача начата");

                for (int i = 1; i <= 100; i++)
                {
                    // Проверяем запрос на отмену
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Задача отменена на шаге {i}");
                        token.ThrowIfCancellationRequested();
                    }

                    Console.Write($"{i}% ");
                    Thread.Sleep(100); // Имитация работы
                }

                Console.WriteLine("\nЗадача завершена успешно");
            }, token);

            // Даем задаче поработать немного
            Thread.Sleep(500);

            // Отменяем задачу
            Console.WriteLine("\nОтправка запроса на отмену...");
            cts.Cancel();

            try
            {
                longTask.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("Задача была отменена: " + e.Message);
                    else
                        Console.WriteLine("Исключение: " + e.Message);
                }
            }
            finally
            {
                cts.Dispose();
                Console.WriteLine($"Статус задачи после отмены: {longTask.Status}");
            }
        }
        #endregion

        #region Задание 3: Три задачи с возвратом результата
        static void Task3_MultipleTasksWithResult()
        {
            Console.WriteLine("\n=== 3. Три задачи с возвратом результата ===\n");

            Console.WriteLine("Запуск трех вычисляющих задач...");

            // Задача 1: Расчет факториала
            Task<int> task1 = Task.Run(() =>
            {
                Console.WriteLine("Задача 1: Расчет факториала 10");
                int result = 1;
                for (int i = 1; i <= 10; i++)
                {
                    result *= i;
                    Thread.Sleep(50);
                }
                return result;
            });

            // Задача 2: Сумма арифметической прогрессии
            Task<int> task2 = Task.Run(() =>
            {
                Console.WriteLine("Задача 2: Сумма арифметической прогрессии (1-100)");
                int sum = 0;
                for (int i = 1; i <= 100; i++)
                {
                    sum += i;
                    Thread.Sleep(10);
                }
                return sum;
            });

            // Задача 3: Количество простых чисел до 100
            Task<int> task3 = Task.Run(() =>
            {
                Console.WriteLine("Задача 3: Количество простых чисел до 100");
                int count = 0;
                for (int i = 2; i <= 100; i++)
                {
                    if (IsPrime(i)) count++;
                    Thread.Sleep(20);
                }
                return count;
            });

            // Ждем завершения всех задач
            Task.WaitAll(task1, task2, task3);

            // Задача 4: Используем результаты трех задач для расчета
            Console.WriteLine("\nЗадача 4: Расчет по формуле (task1 + task2) / task3");

            int result1 = task1.Result;
            int result2 = task2.Result;
            int result3 = task3.Result;

            double finalResult = (double)(result1 + result2) / result3;

            Console.WriteLine($"Результат 1 (факториал 10): {result1}");
            Console.WriteLine($"Результат 2 (сумма 1-100): {result2}");
            Console.WriteLine($"Результат 3 (простых чисел до 100): {result3}");
            Console.WriteLine($"Итоговый результат: ({result1} + {result2}) / {result3} = {finalResult:F2}");
        }

        static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            for (int i = 3; i * i <= n; i += 2)
                if (n % i == 0) return false;

            return true;
        }
        #endregion

        #region Задание 4: Задачи продолжения
        static void Task4_ContinuationTasks()
        {
            Console.WriteLine("\n=== 4. Задачи продолжения ===\n");

            // 4.1 ContinueWith
            Console.WriteLine("4.1 ContinueWith:");

            Task<int> initialTask = Task.Run(() =>
            {
                Console.WriteLine("Начальная задача: генерация случайного числа");
                Thread.Sleep(500);
                return new Random().Next(1, 100);
            });

            // Продолжение 1
            Task continuation1 = initialTask.ContinueWith(prevTask =>
            {
                Console.WriteLine($"Продолжение 1: предыдущий результат = {prevTask.Result}");
                Console.WriteLine($"Продолжение 1: удваиваем результат = {prevTask.Result * 2}");
            });

            // Продолжение 2 (запускается после продолжения 1)
            Task continuation2 = continuation1.ContinueWith(prevTask =>
            {
                Console.WriteLine("Продолжение 2: выводим завершающее сообщение");
            });

            continuation2.Wait();

            // 4.2 GetAwaiter(), GetResult()
            Console.WriteLine("\n4.2 GetAwaiter(), GetResult():");

            Task<string> asyncTask = Task.Run(() =>
            {
                Console.WriteLine("Асинхронная задача: получение строки");
                Thread.Sleep(300);
                return "Результат асинхронной операции";
            });

            // Использование GetAwaiter()
            var awaiter = asyncTask.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                Console.WriteLine("OnCompleted: задача завершена");
                string result = awaiter.GetResult();
                Console.WriteLine($"OnCompleted: получен результат - {result}");
            });

            Thread.Sleep(500);
        }
        #endregion

        #region Задание 5: Класс Parallel
        static void Task5_ParallelLoops()
        {
            Console.WriteLine("\n=== 5. Класс Parallel - распараллеливание циклов ===\n");

            const int size = 1000000;
            double[] array = new double[size];
            Random rand = new Random();

            // Инициализация массива
            for (int i = 0; i < size; i++)
            {
                array[i] = rand.NextDouble() * 100;
            }

            // 5.1 Parallel.For
            Console.WriteLine("5.1 Parallel.For - обработка массива:");

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Последовательная обработка
            double[] sequentialResult = new double[size];
            for (int i = 0; i < size; i++)
            {
                sequentialResult[i] = Math.Sqrt(array[i]) * Math.Log(array[i] + 1);
            }
            stopwatch.Stop();
            Console.WriteLine($"Последовательный For: {stopwatch.ElapsedMilliseconds} мс");

            // Параллельная обработка
            double[] parallelResult = new double[size];
            stopwatch.Restart();
            Parallel.For(0, size, i =>
            {
                parallelResult[i] = Math.Sqrt(array[i]) * Math.Log(array[i] + 1);
            });
            stopwatch.Stop();
            Console.WriteLine($"Parallel.For: {stopwatch.ElapsedMilliseconds} мс");

            // 5.2 Parallel.ForEach
            Console.WriteLine("\n5.2 Parallel.ForEach - генерация массивов:");

            List<double[]> arrays = new List<double[]>();
            for (int i = 0; i < 10; i++)
            {
                arrays.Add(new double[100000]);
            }

            stopwatch.Restart();
            Parallel.ForEach(arrays, (arr, state, index) =>
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    arr[j] = Math.Sin(index) * Math.Cos(j);
                }
            });
            stopwatch.Stop();
            Console.WriteLine($"Parallel.ForEach (10 массивов по 100000): {stopwatch.ElapsedMilliseconds} мс");
        }
        #endregion

        #region Задание 6: Parallel.Invoke
        static void Task6_ParallelInvoke()
        {
            Console.WriteLine("\n=== 6. Parallel.Invoke ===\n");

            Console.WriteLine("Запуск 5 операций параллельно:");

            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.Invoke(
                () =>
                {
                    Console.WriteLine($"Операция 1 начата в потоке {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(800);
                    Console.WriteLine("Операция 1 завершена");
                },
                () =>
                {
                    Console.WriteLine($"Операция 2 начата в потоке {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(600);
                    Console.WriteLine("Операция 2 завершена");
                },
                () =>
                {
                    Console.WriteLine($"Операция 3 начата в потоке {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(400);
                    Console.WriteLine("Операция 3 завершена");
                },
                () =>
                {
                    Console.WriteLine($"Операция 4 начата в потоке {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(200);
                    Console.WriteLine("Операция 4 завершена");
                },
                () =>
                {
                    Console.WriteLine($"Операция 5 начата в потоке {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(300);
                    Console.WriteLine("Операция 5 завершена");
                }
            );

            stopwatch.Stop();
            Console.WriteLine($"\nВсе операции завершены за {stopwatch.ElapsedMilliseconds} мс");
        }
        #endregion

        #region Задание 7: BlockingCollection
        static void Task7_BlockingCollection()
        {
            Console.WriteLine("\n=== 7. BlockingCollection - поставщики и покупатели ===\n");

            BlockingCollection<string> warehouse = new BlockingCollection<string>(boundedCapacity: 20);
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.WriteLine("Склад пустой. Запуск поставщиков и покупателей...\n");

            // 5 поставщиков
            List<Task> suppliers = new List<Task>();
            for (int i = 1; i <= 5; i++)
            {
                int supplierId = i;
                suppliers.Add(Task.Run(() => Supplier(supplierId, warehouse, cts.Token)));
            }

            // 10 покупателей
            List<Task> customers = new List<Task>();
            for (int i = 1; i <= 10; i++)
            {
                int customerId = i;
                customers.Add(Task.Run(() => Customer(customerId, warehouse, cts.Token)));
            }

            // Даем поработать 10 секунд
            Thread.Sleep(10000);

            // Останавливаем
            Console.WriteLine("\nОстановка работы...");
            cts.Cancel();

            Task.WaitAll(suppliers.Concat(customers).ToArray());

            Console.WriteLine($"\nОстаток на складе: {warehouse.Count} товаров");
            foreach (var item in warehouse)
            {
                Console.WriteLine($"  - {item}");
            }

            warehouse.Dispose();
            cts.Dispose();
        }

        static void Supplier(int id, BlockingCollection<string> warehouse, CancellationToken token)
        {
            string[] products = { "Холодильник", "Телевизор", "Стиральная машина", "Пылесос", "Микроволновка",
                                "Кофеварка", "Утюг", "Чайник", "Блендер", "Тостер" };

            Random rand = new Random();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Случайная задержка поставки (разная скорость)
                    int delay = rand.Next(500, 2000);
                    Thread.Sleep(delay);

                    string product = $"Поставщик {id}: {products[rand.Next(products.Length)]}";

                    if (warehouse.TryAdd(product, 100, token))
                    {
                        Console.WriteLine($"[+] {product} добавлен на склад");
                        PrintWarehouseState(warehouse);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            Console.WriteLine($"Поставщик {id} завершил работу");
        }

        static void Customer(int id, BlockingCollection<string> warehouse, CancellationToken token)
        {
            Random rand = new Random();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Покупатель приходит с разной частотой
                    Thread.Sleep(rand.Next(300, 1500));

                    if (warehouse.TryTake(out string product, 500, token)) // Ждет 500мс
                    {
                        Console.WriteLine($"[-] Покупатель {id} купил: {product}");
                        PrintWarehouseState(warehouse);
                    }
                    else
                    {
                        Console.WriteLine($"[!] Покупатель {id} ушел без покупки (товара нет)");
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            Console.WriteLine($"Покупатель {id} завершил покупки");
        }

        static void PrintWarehouseState(BlockingCollection<string> warehouse)
        {
            Console.Write($"Склад [{warehouse.Count}]: ");
            foreach (var item in warehouse)
            {
                Console.Write($"{item.Split(':')[1].Trim()}, ");
            }
            Console.WriteLine();
        }
        #endregion

        #region Задание 8: Async/Await
        static async Task Task8_AsyncAwait()
        {
            Console.WriteLine("\n=== 8. Async/Await ===\n");

            Console.WriteLine("Запуск асинхронных операций...");

            // Асинхронный метод
            var result1 = await CalculateAsync("Операция 1", 1500);
            Console.WriteLine($"Результат 1: {result1}");

            // Параллельное выполнение асинхронных операций
            Console.WriteLine("\nПараллельное выполнение 3 операций:");

            Task<int> task1 = CalculateAsync("Операция A", 1200);
            Task<int> task2 = CalculateAsync("Операция B", 800);
            Task<int> task3 = CalculateAsync("Операция C", 1600);

            // Ожидаем все задачи
            await Task.WhenAll(task1, task2, task3);

            Console.WriteLine($"\nВсе операции завершены:");
            Console.WriteLine($"Результат A: {task1.Result}");
            Console.WriteLine($"Результат B: {task2.Result}");
            Console.WriteLine($"Результат C: {task3.Result}");

            // Асинхронный метод с возвратом результата
            Console.WriteLine("\nАсинхронное чтение файла:");
            string fileContent = await ReadFileAsync("example.txt");
            Console.WriteLine($"Содержимое файла: {fileContent}");
        }

        static async Task<int> CalculateAsync(string operationName, int delay)
        {
            Console.WriteLine($"{operationName} начата в потоке {Thread.CurrentThread.ManagedThreadId}");

            await Task.Delay(delay); // Асинхронная задержка

            int result = new Random().Next(1, 100);
            Console.WriteLine($"{operationName} завершена");

            return result;
        }

        static async Task<string> ReadFileAsync(string filename)
        {
            // Имитация асинхронного чтения файла
            await Task.Delay(500);

            // Если файла нет - создаем пример
            if (!System.IO.File.Exists(filename))
            {
                // Для .NET Framework используем синхронную версию
                System.IO.File.WriteAllText(filename,
                    "Пример содержимого файла для асинхронного чтения.\n" +
                    "Вторая строка файла.\n" +
                    "Третья строка.");
            }

            // Асинхронное чтение файла через Task.Run
            return await Task.Run(() => System.IO.File.ReadAllText(filename));
        }
    }
}
#endregion 