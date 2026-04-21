using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // tuple function
            (double area, double perimeter) CalculateRectangle(double length, double width)
            {
                double area = length * width;
                double perimeter = 2 * (length + width);
                return (area, perimeter);
            }

            var result = CalculateRectangle(10, 10);
            Console.WriteLine($"Area: {result.area}, Perimeter: {result.perimeter}");

            // Pattern Matching
            Console.WriteLine("Enter the Input:");
            string temp = Console.ReadLine();

            if (int.TryParse(temp, out int i))
            {
                Console.WriteLine($"The input is integer: {i}");
            }
            else if (double.TryParse(temp, out double d))
            {
                Console.WriteLine($"The input is double: {d}");
            }
            else
            {
                Console.WriteLine($"The input is string: {temp}");
            }
        }
    }
}