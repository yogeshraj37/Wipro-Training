using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AnonymousAndLamda


{

    class Student
    {
        public string Name;
        public int Marks;
        public int Attendance;
        public int Participation;
    }

    internal class Program
    {
        // A college wants to build a Student Activity Evaluation
        // System that processes and evaluates student performance dynamically
        // 1without creating multiple separate methods.
        //Design a C# program where:
        //The system evaluates student scores, attendance, and participation
        //Instead of defining separate named methods, use:
        //Anonymous Methods for inline logic execution
        //Lambda Expressions for concise and readable operations
        //Create a delegate to handle student evaluation logic
        //Use an anonymous method to:
        //Calculate total marks
        //Display student performance
        //Use a lambda expression to:
        //Check if a student is eligible (e.g., marks > 50)
        //Filter a list of students based on performance
        public delegate void EvaluateStudent(Student s);
        static void Main(string[] args)
        {

            List<Student> students = new List<Student>()
            {
                new Student { Name = "Yogesh", Marks = 70, Attendance = 80, Participation = 60 },
                new Student { Name = "Amit", Marks = 45, Attendance = 60, Participation = 50 },
                new Student { Name = "Riya", Marks = 85, Attendance = 90, Participation = 80 },
                new Student { Name = "Neha", Marks = 40, Attendance = 55, Participation = 45 }
            };
 
            EvaluateStudent evaluator = delegate (Student s)
            {
                int total = s.Marks + s.Attendance + s.Participation;

                Console.WriteLine("----- Student Report -----");
                Console.WriteLine("Name: " + s.Name);
                Console.WriteLine("Total Score: " + total);

                if (total > 200)
                    Console.WriteLine("Performance: Excellent");
                else if (total > 150)
                    Console.WriteLine("Performance: Good");
                else
                    Console.WriteLine("Performance: Average");

                Console.WriteLine();
            };

          
            foreach (var student in students)
            {
                evaluator(student);
            }

     
            Func<Student, bool> isEligible = s => s.Marks > 50;

            Console.WriteLine("---- Eligible Students (Marks > 50) ----");

            var eligibleStudents = students.Where(isEligible);

            foreach (var s in eligibleStudents)
            {
                Console.WriteLine(s.Name);
            }
 
            Console.WriteLine("\n---- Top Performers (Total > 200) ----");

            var topStudents = students.Where(s =>
                (s.Marks + s.Attendance + s.Participation) > 200);

            foreach (var s in topStudents)
            {
                Console.WriteLine(s.Name);
            }
        }
    }
    }

