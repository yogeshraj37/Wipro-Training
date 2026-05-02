using System;

class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string message) : base(message)
    {


    }
}

class InvalidAmountException : Exception
{
     public InvalidAmountException(string message) : base(message) { }
}

class BankAccount
{
    public string AccountHolderName;
    public double Balance;

    public BankAccount(string accountHolderName, double balance)
    {
          
        AccountHolderName = accountHolderName;
        Balance = balance;
    }
    public void Deposit(double amount)

    {
        if (amount < 0)
        {
            throw new InvalidAmountException("The amount is less than Zero");
        }
        Balance += amount;
        
    }
    public void Withdraw(double amount)
    {
        if (amount < 0)
        {
            throw new InsufficientBalanceException("The amount must be greater thann zero");
        }
         if (Balance < amount)
        {
            throw new InsufficientBalanceException("Amount must be less than your current balance");
        }
       if (Balance - amount < 1000)
        {
            throw new InsufficientBalanceException("Minimum Account balance 1000 must be kept");
        }
        else
        {
            Balance -= amount;
        }
    }
      public void CheckBalance()
    {
        Console.WriteLine("The Current Balance of the Account is:" + Balance);
    }
    
}
class Program
{
    static void Main()
    {

        Console.WriteLine("Enetr the name of AccountHolder:");
        string Name = Console.ReadLine();

        Console.WriteLine("Enter the balance you wnat to add in the Account");
        double Balance = Convert.ToDouble(Console.ReadLine());


        BankAccount account = new BankAccount(Name, Balance);

        while (true)
        {

            Console.WriteLine("\n 1.Deposit \n 2.WithDraw \n 3.CheckBalance \n 4.Exit");
            Console.WriteLine("Chose a option:");
            int choice = Convert.ToInt32(Console.ReadLine());

            try
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the balance you want to deposit:");
                        double balance = Convert.ToDouble(Console.ReadLine());
                        account.Deposit(balance);
                        break;
                    case 2:
                        Console.WriteLine("Enter the amount you want to Withdraw");
                        double temp = Convert.ToDouble(Console.ReadLine());
                        account.Withdraw(temp);
                        break;
                    case 3:
                        account.CheckBalance();
                        break;
                    case 4:
                        return;
                    default:
                        throw new ArgumentException("Invalid Option");
                }
            }
            catch (InvalidAmountException ex)
            {
                Console.WriteLine("Error:" + ex.Message);

            }
            catch (InsufficientBalanceException ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            finally
            {
                Console.WriteLine("Thanks");
            }

        }
    }
}
