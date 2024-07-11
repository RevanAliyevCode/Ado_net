using Country.Concrets;
using Country.Extensions;
using Country.Services.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country.Services.Command
{
    public class Update
    {
        public static void UpdateCountry()
        {
        NameLable: Messages.InputMessages("country name");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInput();
                goto NameLable;
            }

            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand sqlCommand = new("SELECT * FROM Countries WHERE Name=@name", connection);
            sqlCommand.Parameters.AddWithValue("@name", name);
            int id = Convert.ToInt32(sqlCommand.ExecuteScalar());

            if (id <= 0)
            {
                Messages.NotFound(name);
                return;
            }

        OpinionLabel: Messages.Opinion("name", "change");
            string? input = Console.ReadLine();
            bool isSucceded = char.TryParse(input, out char choice);

            if (string.IsNullOrWhiteSpace(input) || !isSucceded || !input.IsValidChoice())
            {
                Messages.InvalidInput();
                goto OpinionLabel;
            }

            string? newName = "";

            if (choice.Equals('y'))
            {
            NewNameLabel: Messages.InputMessages("new name");
                newName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Messages.InvalidInput();
                    goto NewNameLabel;
                }

                SqlCommand searchedCountry = new("SELECT * FROM Countries WHERE Name=@name AND Id != @id", connection);
                searchedCountry.Parameters.AddWithValue("@name", newName);
                searchedCountry.Parameters.AddWithValue("@id", id);

                int existId = Convert.ToInt32(searchedCountry.ExecuteScalar());

                if (existId > 0)
                {
                    Messages.Exist("Country", newName);
                    goto NewNameLabel;
                }
            }

        OpinionAreaLabel: Messages.Opinion("area", "change");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out choice);

            if (string.IsNullOrWhiteSpace(input) && !isSucceded && input!.IsValidChoice())
            {
                Messages.InvalidInput();
                goto OpinionAreaLabel;
            }

            decimal newArea = 0;

            if (choice.Equals('y'))
            {
            NewAreaLabel: Messages.InputMessages("new area");
                bool isValid = decimal.TryParse(Console.ReadLine(), out newArea);

                if (!isValid || newArea <= 0)
                {
                    Messages.InvalidInput();
                    goto NewAreaLabel;
                }
            }

            List<string> clouseList = [];
            SqlCommand command = new("UPDATE Countries SET ", connection);


            if (newName != "")
            {
                clouseList.Add("Name=@name");
                command.Parameters.AddWithValue("@name", newName);
            }

            if (newArea != 0)
            {
                clouseList.Add("Area=@area");
                command.Parameters.AddWithValue("@area", newArea);
            }

            if (clouseList.Count > 0)
            {
                command.CommandText += string.Join(", ", clouseList) + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                int affected = command.ExecuteNonQuery();

                if (affected > 0)
                    Messages.SuccessMessage("Country", "changed");
                else
                    Messages.ErrorOcured();
            }

        }

        public static void UpdateCity()
        {
        NameLable: Messages.InputMessages("city name");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInput();
                goto NameLable;
            }

            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand sqlCommand = new("SELECT * FROM Cities WHERE Name=@name", connection);
            sqlCommand.Parameters.AddWithValue("@name", name);
            int id = Convert.ToInt32(sqlCommand.ExecuteScalar());

            if (id <= 0)
            {
                Messages.NotFound(name);
                return;
            }

            int countryId = 0;
            decimal cityArea = 0;
            using (var reader = sqlCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    countryId = reader.GetInt32(reader.GetOrdinal("CountryId"));
                    cityArea = reader.GetDecimal(reader.GetOrdinal("Area"));
                }
            }

        OpinionLabel: Messages.Opinion("name", "change");
            string? input = Console.ReadLine();
            bool isSucceded = char.TryParse(input, out char choice);

            if (string.IsNullOrWhiteSpace(input) || !isSucceded || !input.IsValidChoice())
            {
                Messages.InvalidInput();
                goto OpinionLabel;
            }

            string? newName = "";

            if (choice.Equals('y'))
            {
            NewNameLabel: Messages.InputMessages("new name");
                newName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Messages.InvalidInput();
                    goto NewNameLabel;
                }

                SqlCommand searchedCity = new("SELECT * FROM Cities WHERE Name=@name AND Id != @id", connection);
                searchedCity.Parameters.AddWithValue("@name", newName);
                searchedCity.Parameters.AddWithValue("@id", id);

                int existId = Convert.ToInt32(searchedCity.ExecuteScalar());

                if (existId > 0)
                {
                    Messages.Exist("City", newName);
                    goto NewNameLabel;
                }
            }

        OpinionAreaLabel: Messages.Opinion("area", "change");
            input = Console.ReadLine();
            isSucceded = char.TryParse(input, out choice);

            if (string.IsNullOrWhiteSpace(input) && !isSucceded && input!.IsValidChoice())
            {
                Messages.InvalidInput();
                goto OpinionAreaLabel;
            }

            decimal newArea = 0;
            decimal countryArea = Others.FindCountryArea(countryId);

            if (choice.Equals('y'))
            {
            NewAreaLabel: Messages.InputMessages("new area");
                bool isValid = decimal.TryParse(Console.ReadLine(), out newArea);

                if (!isValid || newArea <= 0)
                {
                    Messages.InvalidInput();
                    goto NewAreaLabel;
                }

                if (newArea > countryArea - Others.FindSumArea(countryId) + cityArea)
                {
                    Console.WriteLine("City area cannot be higher than country");
                    goto NewAreaLabel;
                }
            }

            List<string> clouseList = [];
            SqlCommand command = new("UPDATE Cities SET ", connection);


            if (newName != "")
            {
                clouseList.Add("Name=@name");
                command.Parameters.AddWithValue("@name", newName);
            }

            if (newArea != 0)
            {
                clouseList.Add("Area=@area");
                command.Parameters.AddWithValue("@area", newArea);
            }

            if (clouseList.Count > 0)
            {
                command.CommandText += string.Join(", ", clouseList) + " WHERE Id=@id";
                command.Parameters.AddWithValue("@id", id);

                int affected = command.ExecuteNonQuery();

                if (affected > 0)
                    Messages.SuccessMessage("City", "changed");
                else
                    Messages.ErrorOcured();
            }
        }
    }
}
