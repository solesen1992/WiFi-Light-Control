using BCrypt.Net;

namespace HashingTest
{
    internal class Program
    {

        /* This class demonstrates how to hash a password using the BCrypt.Net library.
       It shows an example of converting a plain-text password into a secure hash,
       which can be stored safely for authentication purposes*/

        static void Main(string[] args)
        {
            // Define a password string (currently empty)
            string password = "";

            // Generate a hashed version of the password using the BCrypt algorithm
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Output the hashed password to the console
            Console.WriteLine($"Hashed password: {hashedPassword}");

        }
    }
}
