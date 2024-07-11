using Country.Concrets;
using Country.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country.Services.Command
{
    public static class Delete
    {
        public static void DeleteCountry()
        {
            NameLabel: Messages.InputMessages("country name");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInput();
                goto NameLabel;
            }

            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand command = new("SELECT * FROM Countries WHERE Name=@name", connection);
            command.Parameters.AddWithValue("@name", name);

            int id = Convert.ToInt32(command.ExecuteScalar());

            if (id <= 0)
            {
                Messages.NotFound(name);
                return;
            }

        OpinionLabel: Messages.Opinion(name, "delete");
            string? input = Console.ReadLine();
            bool isSucceded = char.TryParse(input, out char choice);

            if (string.IsNullOrWhiteSpace(input) || !isSucceded || !input.IsValidChoice())
            {
                Messages.InvalidInput();
                goto OpinionLabel;
            }

            if (choice.Equals('y'))
            {
                command = new("DELETE Countries WHERE Id=@id", connection);
                command.Parameters.AddWithValue("@id", id);

                int affected = command.ExecuteNonQuery();

                if (affected > 0)
                    Messages.SuccessMessage(name, "deleted");
                else
                    Messages.ErrorOcured();
            }
        }

        public static void DeleteCity()
        {
        NameLabel: Messages.InputMessages("city name");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInput();
                goto NameLabel;
            }

            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand command = new("SELECT * FROM Cities WHERE Name=@name", connection);
            command.Parameters.AddWithValue("@name", name);

            int id = Convert.ToInt32(command.ExecuteScalar());

            if (id <= 0)
            {
                Messages.NotFound(name);
                return;
            }

        OpinionLabel: Messages.Opinion(name, "delete");
            string? input = Console.ReadLine();
            bool isSucceded = char.TryParse(input, out char choice);

            if (string.IsNullOrWhiteSpace(input) || !isSucceded || !input.IsValidChoice())
            {
                Messages.InvalidInput();
                goto OpinionLabel;
            }

            if (choice.Equals('y'))
            {
                command = new("DELETE Cities WHERE Id=@id", connection);
                command.Parameters.AddWithValue("@id", id);

                int affected = command.ExecuteNonQuery();

                if (affected > 0)
                    Messages.SuccessMessage(name, "deleted");
                else
                    Messages.ErrorOcured();
            }
        }
    }
}
