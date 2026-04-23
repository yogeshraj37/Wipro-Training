using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Day7
{
    internal class BinarySearch
    {

      public static  int  Search(int[] arr, int target)
        {
            int len = arr.Length;
            int low = 0, high = len - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                if (arr[mid] == target)
                {
                    return mid;
                }else if (arr[mid] > target)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }
            return -1;
        }
    }
}
