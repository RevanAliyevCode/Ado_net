using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Country.Concrets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country.Services.Query
{
    public static class Others
    {
        public static decimal FindCountryArea(int id)
        {
            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand command = new("SELECT Area FROM Countries WHERE Id=@id", connection);
            command.Parameters.AddWithValue("@id", id);

            decimal findedArea = Convert.ToDecimal(command.ExecuteScalar());

            if (findedArea < 0)
            {
                return 0;
            }
            return findedArea;
        } 
        public static decimal FindSumArea(int id)
        {
            using SqlConnection connection = new(Connections.Default);
            connection.Open();
            SqlCommand command = new("SELECT c.Id, SUM(ct.Area) AS [UsedArea] FROM Countries c JOIN Cities ct ON c.Id = ct.CountryId GROUP BY c.Id HAVING c.Id=@id;", connection);
            command.Parameters.AddWithValue("@id", id);

            int findedId = Convert.ToInt32(command.ExecuteScalar());

            if (findedId < 0)
            {
                return 0;
            }

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetDecimal(reader.GetOrdinal("UsedArea"));
            }

            return 0;
        }
    }
}
