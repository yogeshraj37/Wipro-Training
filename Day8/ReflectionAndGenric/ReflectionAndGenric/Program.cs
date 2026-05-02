using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionAndGenric

{
      class GenricClass<T>
    {
        public T Data;
        public void Display(T value)
        {
            Console.WriteLine("Value is"+ value);
        }
    }

    class Student
    {
        public string Name { get; set; }
        public int Marks { get; set; }

        public void Display()
        {
            Console.WriteLine("Student Details");
        }

        public int GetMarks()
        {
            return Marks;
        }
    }
    internal class Program
    {

          static void usingRef(ref int x)
        {
            x += 10;
        }
        static void usingOut(out int y)
        {
            y = 20;
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Ref And Out ________________________:");

            int a = 10;
            usingRef(ref a);
            Console.WriteLine("Ref Value:"+ a);
            int b = 20;
            usingOut(out b);
            Console.WriteLine("out value:" + b);


            Console.WriteLine("GenricClass _____________________:");
            GenricClass<int> obj1 = new GenricClass<int>();
            obj1.Data = 123;
            obj1.Display(obj1.Data);



            Console.WriteLine("Reflection ----------------------:");
            Type t = typeof(Student);

            // Display Class Name
            Console.WriteLine("Class Name: " + t.Name);

            // Display Properties
            Console.WriteLine("\nProperties:");
            foreach (PropertyInfo prop in t.GetProperties())
            {
                Console.WriteLine(prop.Name + " : " + prop.PropertyType.Name);
            }

            // Display Methods
            Console.WriteLine("\nMethods:");
            foreach (MethodInfo method in t.GetMethods())
            {
                // To skip default methods like ToString(), Equals(), etc.
                if (method.DeclaringType == t)
                {
                    Console.WriteLine(method.Name);
                }
            }



        }
    }
}
