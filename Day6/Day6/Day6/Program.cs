    //Console.WriteLine("Hello, World!");
    //// Delgates

    //Console.WriteLine("-------------------------");
    //int  Sum(int a,int b)

    //{
    //    return a + b;
    //}
    //Console.WriteLine(Sum(3, 4));
    //Console.WriteLine("_____________________");
    // Problem statement based on use of delgates in C#:
    //1. Create a delegate type called "MathOperation" that takes two integers as parameters and
    //returns an integer as output.
    //2. Implement three methods that match the signature of the "MathOperation" delegate:
    //   a. Add: This method should return the sum of the two integers.
    //   b. Subtract: This method should return the difference of the two integers.
    //   c. Multiply: This method should return the product of the two integers.
    //3. Create an instance of the "MathOperation" delegate and assign it the "Add" method.
    //4. Invoke the delegate with two integers and display the result.
    //5. Change the delegate assignment to the "Subtract" method and invoke it again with the same integers, displaying the result.
 

    //Creating an instance of the "MathOperation" delegate and assigning it the "Add" method

    

    namespace DelegateExample
    {
   

        class Program
        {
            public delegate int MathOperation(int a, int b);


            public int Add(int a, int b)
            {
                return a + b;
            }

            public int Subtract(int a, int b)
            {
                return a - b;
            }

            public int Multiply(int a, int b)
            {
                return a * b;
            }

            static void Main(string[] args)
            {
                int x = 10, y = 5;

 
                Program obj = new Program();

            
                MathOperation operation = obj.Add;

                int result = operation(x, y);
                Console.WriteLine("Addition Result: " + result);

                operation = obj.Subtract;
                result = operation(x, y);
                Console.WriteLine("Subtraction Result: " + result);

                operation = obj.Multiply;
                result = operation(x, y);
                Console.WriteLine("Multiplication Result: " + result);
            }
        }
    }