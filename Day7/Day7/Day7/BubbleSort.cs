using System;

namespace Day7
{
    internal class BubbleSort
    {
        public static void bubbleSort()
        {
            Console.WriteLine("Enter the length of Arr");
            int n = Convert.ToInt32(Console.ReadLine());

            int[] arr = new int[n];

            Console.WriteLine("Enter the elements:");
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
            }

            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        swapped = true;
                    }
                }

                if (!swapped)
                    break;
            }

            Console.WriteLine("Sorted Array:");
            foreach (int num in arr)
            {
                Console.Write(num + " ");
            }
        }
    }
}