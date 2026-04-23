using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FileHandling


{

      //Why We need File Handling?
    //With the help of File handling we are able to store data temporary in files.
    //we can read files
    //we can write into files
    //we can merge/append files
    //we can have binary file

    //Steps for file //Steps for file handling in C#
    //Step 1: Import the namespace ie using System.IO
    //Step 2: Specify the file path which we want to read/write
    //Step 3: Choose operations creating, reading, writing deleting etc
    //Step 4: Use streams and exception handling
    //Step 5: Close the file
    internal class Program
    {
        static void Main(string[] args)
        {

            //string FilePath = "C:\\Users\\ASUS\\Desktop\\Wipro Training\\Day7\\FileHandling\\FileHandling";
            string FileName = "demo.txt";
            try
            {
                Console.WriteLine("Created a file");
                File.Create(FileName).Close();
                Console.WriteLine("Adding text to file");
                File.WriteAllText(FileName, "hii how are you");
                Console.WriteLine("Appending text to file");
                File.AppendAllText(FileName, "\nI am good");
                Console.WriteLine("Reading from the file");
                string content = File.ReadAllText(FileName);
                Console.WriteLine(content);
            }catch(Exception e)
            {
                Console.WriteLine("The error is:" + e.Message);
                throw;
            }
            finally
            {
                Console.WriteLine("hii");
            }



        }
    }
}
