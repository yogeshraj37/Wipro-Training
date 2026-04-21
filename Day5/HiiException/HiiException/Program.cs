using System;

namespace ConsoleApp1
{
    class sleep : Exception
    {
        public sleep(string message) :base(message)
        {

        }
    }

    class  Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.WriteLine("Enter the hour");
                int time = Convert.ToInt32(Console.ReadLine());

                if (time > 6)
                {
                    Console.WriteLine("You Sleep well");
                }
                else
                {
                    throw new sleep("Sleeping less than 6 hour is not god");
                }
            }
            catch (sleep e)
            {
                Console.WriteLine("Custome Exception:" + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Global Exception:" + e.Message);
            }
            finally
            {
                Console.WriteLine("Hii i am done with it");
            }

        }
        }
    }