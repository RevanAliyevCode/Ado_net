using Country.Concrets;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Country.Services.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Country.Services.Command
{
    public static class Adding
    {
        public static void AddCountry()
        {
            using SqlConnection connection = new(Connections.Default);
            connection.Open();
        NameLabel: Messages.InputMessages("name");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInput();
                goto NameLabel;
            }

            SqlCommand query = new("SELECT * FROM Countries WHERE Name=@name", connection);
            query.Parameters.AddWithValue("@name", name);

            int isExist = Convert.ToInt32(query.ExecuteScalar());
            if (isExist > 0)
            {
                Messages.Exist("Country", name);
                goto NameLabel;
            }

        AreaLabel: Messages.InputMessages("area");
            bool isValidArea = decimal.TryParse(Console.ReadLine(), out decimal area);

            if (!isValidArea || area <= 0)
            {
                Messages.InvalidInput();
                goto AreaLabel;
            }

        PopulationLabel: Messages.InputMessages("population");
            bool isValidPopulation = int.TryParse(Console.ReadLine(), out int population);

            if (!isValidPopulation)
            {
                Messages.InvalidInput();
                goto PopulationLabel;
            }


            SqlCommand command = new("INSERT INTO Countries VALUES(@name, @area, @population)", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@area", area);
            command.Parameters.AddWithValue("@population", population);

            int affected = command.ExecuteNonQuery();

            if (affected > 0)
                Messages.SuccessMessage(name, "added");
            else
                Messages.ErrorOcured();
        }

        public static void AddCity()
        {
            using SqlConnection connection = new(Connections.Default);
            connection.Open();
        CountryNameLabel: Messages.InputMessages("country name");
            string? countryName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(countryName))
            {
                Messages.InvalidInput();
                goto CountryNameLabel;
            }

            SqlCommand query = new("SELECT * FROM Countries WHERE Name=@name", connection);
            query.Parameters.AddWithValue("@name", countryName);

            int countryId = Convert.ToInt32(query.ExecuteScalar());
            if (countryId <= 0)
            {
                Messages.NotFound(countryName);
                goto CountryNameLabel;
            }

            decimal countryArea = 0;
            using (var reader = query.ExecuteReader())
            {
                if (reader.Read())
                    countryArea = reader.GetDecimal(reader.GetOrdinal("Area"));
            }

            if (countryArea - Others.FindSumArea(countryId) == 0)
            {
                Console.WriteLine("There is no free space in the country");
                return;
            }

        CityNameLabel: Messages.InputMessages("city name");
            string? cityName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cityName))
            {
                Messages.InvalidInput();
                goto CityNameLabel;
            }

            query = new("SELECT * FROM Cities WHERE Name=@name", connection);
            query.Parameters.AddWithValue("@name", cityName);

            int cityId = Convert.ToInt32(query.ExecuteScalar());
            if (cityId > 0)
            {
                Messages.Exist("City", cityName);
                goto CityNameLabel;
            }

        AreaLabel: Messages.InputMessages("area");
            bool isValidArea = decimal.TryParse(Console.ReadLine(), out decimal area);

            if (!isValidArea || area <= 0)
            {
                Messages.InvalidInput();
                goto AreaLabel;
            }


            if (area > countryArea - Others.FindSumArea(countryId))
            {
                Console.WriteLine("City area cannot be higher than country");
                goto AreaLabel;
            }

            SqlCommand command = new("INSERT INTO Cities VALUES(@name, @area, @countryId)", connection);
            command.Parameters.AddWithValue("@name", cityName);
            command.Parameters.AddWithValue("@area", area);
            command.Parameters.AddWithValue("@countryId", countryId);

            int affected = command.ExecuteNonQuery();

            if (affected > 0)
                Messages.SuccessMessage(cityName, "added");
            else
                Messages.ErrorOcured();
        }
    }
}
