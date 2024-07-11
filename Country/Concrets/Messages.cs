using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country.Concrets
{
    public static class Messages
    {
        public static void InvalidInput() => Console.WriteLine("Please enter valid input");
        public static void ErrorOcured() => Console.WriteLine("Something went wrong");
        public static void InputMessages(string title) => Console.Write($"Write {title}: ");
        public static void SuccessMessage(string title, string action) => Console.WriteLine($"Successfully {action} {title}");
        public static void Exist(string title, string name) => Console.WriteLine($"{title} - {name} already exist");
        public static void NotFound(string title) => Console.WriteLine($"{title} not found");
        public static void Opinion(string title, string action) => Console.Write($"Do you want to {action} {title} (y/n): ");
 

        public static void DisplayDetails(SqlDataReader reader)
        {
            for (int i = 1; i <= reader.FieldCount - 1; i++)
            {
                Console.Write($"{reader.GetName(i),-20}");
            }
            Console.WriteLine();
            while (reader.Read())
            {
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i],-20}");
                }
                Console.WriteLine();
            }
        }
    }
}
