using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
namespace Program
{
    class Program
    {
        static void Main()
        {
            string password = EncryptPassword("my_secure_password");
            System.Console.WriteLine($"Encrypted Password: {password}");
            int sum = 5 + 10;

            Console.WriteLine("The Sum is:" + sum);



        }
        public static string EncryptPassword(string password)
        {
            // Simple encryption logic (for demonstration purposes only)
            using (SHA256 sHA256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);

                var hash = sHA256.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
