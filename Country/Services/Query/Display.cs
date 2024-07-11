using Country.Concrets;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country.Services.Query
{
    public static class Display
    {
        public static void DisplayCountries()
        {
            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand command = new("SELECT * FROM Countries", connection);
            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Messages.DisplayDetails(reader);
                }
            }
            catch (Exception ex)
            {
                Messages.ErrorOcured();
            }
        }

        public static void DisplayCities()
        {
            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand cmd = new("SELECT ct.Id, ct.Name, ct.Area, c.Name AS ConuntryName FROM Cities ct JOIN Countries c ON ct.CountryId=c.Id", connection);

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Messages.DisplayDetails(reader);
                }
            }
            catch (Exception ex)
            {
                Messages.ErrorOcured();
            }
        }
    }
}
