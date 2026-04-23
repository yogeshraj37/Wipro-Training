//Step 1 : Create a method for linear search and binary search
//Step 2 : Take input from user for array and element to be searched    
//Step 3 : Call the method for linear search and binary search and pass the array and element to be searched as parameters
//Steo 4 : Display the result of linear search and binary search
//Step 5: We can show time complexity of linear search and binary search in milliseconds.
using Day7;

ProductList.solve();

//int LinearSearch(int[]arr, int target)
//{
//    for(int i = 0; i < arr.Length; i++)
//    {
//          if(arr[i] == target) return i;
//    }
//    return -1;
//}
//Console.WriteLine("Enter the Sixe of array");

//int len = Convert.ToInt32(Console.ReadLine());
//int[] arr  = new int[len];
//for(int i = 0; i < len; i++)
//{
//    arr[i] = Convert.ToInt32(Console.ReadLine());
//}

//Console.WriteLine("Liner Serach");
//Console.WriteLine("Enter the target value");
//int target  = Convert.ToInt32(Console.ReadLine());

//int temp = LinearSearch(arr, target);

//if (temp != -1) {
//    Console.WriteLine("Target Found");
//}
//else
//{
//    Console.WriteLine("Target Not Found");
//}
//Console.WriteLine(" Binary Search -----------------------");
//int result = BinarySearch.Search(arr, target);

//if (result != -1)
//{
//    Console.WriteLine("Element Found");
//}
//else
//{
//    Console.WriteLine("Not Found");
//}


//Console.WriteLine("Sorting---------------------");

//BubbleSort.bubbleSort();

//Console.WriteLine("--------------------------");



//class MyCollection// This class represents a collection of integers and implements an indexer to access its elements.
//{
//    private int[] data = new int[10]; // This array will hold the integers in the collection.
//                                      // Implementing the indexer
//    public int this[int index] // This is the indexer definition. It allows us to access elements of the collection using an index.
//    {
//        get// The 'get' accessor is used to retrieve the value at the specified index.
//        {
//            if (index < 0 || index >= data.Length) // OR condition to check if the index is within the bounds of the array.
//            {
//                throw new IndexOutOfRangeException("Index out of range");
//            }
//            return data[index];
//        }
//    }
//    set // The 'set' accessor is used to assign a value to the specified index.
//            {
//                if (index< 0 || index >= data.Length)
//                {
//                    throw new IndexOutOfRangeException("Index out of range");
//}
//data[index] = value;
//            }
//        }
//    }

///With the help of indexing we can access the elements of an array or a collection using their index.
//Steps for implemting indexing in C#:
//Step 1: Define a class that will represent the collection you want to index.
//Step 2: Implement the indexer in the class. An indexer is defined using the 'this' keyword followed by square brackets [].
//Step 3: Inside the indexer, you can define the logic to get or set the value based on the index provided.
//Step 4: Create an instance of the class and use the indexer to access or modify the elements of the collection.