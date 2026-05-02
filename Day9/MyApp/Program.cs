using System;
using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;
namespace Progam
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            int a = 4;
            int b = 5;
            Console.WriteLine("Sum is:" + (a + b));

            List<string> todo = new List<string>();
            int choose;
            do
            {
                System.Console.WriteLine("\n 1.Add task \n 2. Display task \n 3.Exit");
                choose = Convert.ToInt32(Console.ReadLine());
                switch (choose)
                {
                    case 1:
                        System.Console.WriteLine("Enter the task you want to Add");
                        string task = Console.ReadLine();
                        todo.Add(task);
                        System.Console.WriteLine("Task Added Successfully");
                        break;
                    case 2:
                        System.Console.WriteLine("Display All taks");
                        if (todo.Count == 0)
                        {
                            System.Console.WriteLine("Please Add Some Taks");
                        }
                        foreach (var temp in todo)
                        {
                            System.Console.WriteLine(temp);
                        }
                        break;
                    case 3:
                        System.Console.WriteLine("Thanks");
                        break;
                    default:
                        System.Console.WriteLine("Invalid Choice");
                        break;


                }
            } while (choose != 3);


        }
    }
}