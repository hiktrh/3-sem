using System;

class zad6
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        
        void CheckedOverflow()
        {
            try
            {
                checked
                {
                    int max = int.MaxValue;
                    Console.WriteLine("Checked: исходное значение = " + max);
                    int result = max + 1;
                    Console.WriteLine("Checked: результат = " + result);
                }
            }
            catch (OverflowException ex)
            {
                Console.WriteLine("Checked: переполнение обнаружено → " + ex.Message);
            }
        }

        void UncheckedOverflow()
        {
            unchecked
            {
                int max = int.MaxValue;
                Console.WriteLine("Unchecked: исходное значение = " + max);
                int result = max + 1; 
                Console.WriteLine("Unchecked: результат = " + result);
            }
        }

        
        CheckedOverflow();

        Console.WriteLine("\n========================================================");
        UncheckedOverflow();

        Console.WriteLine("\nАнализ:");
        Console.WriteLine("В checked-блоке переполнение вызывает исключение.");
        Console.WriteLine("В unchecked-блоке переполнение игнорируется, и результат «переворачивается» в отрицательное значение.");

        Console.ReadKey();
    }
}
