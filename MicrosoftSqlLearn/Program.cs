using System;
using System.Data.SqlClient;

namespace MicrosoftSqlLearn
{
    class Program
    {
        private static void CreateCommand(
            string queryString,
            string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(string.Format("{0}, {1}", reader[0], reader[1]));
                    }
                }
            }
        }

        private static void GetCount(string tableName, string connectionString)
        {
            var queryString = $"select count(1) from [{tableName}]";
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var x = command.ExecuteScalar() as int?;
                Console.WriteLine(x);
            }
        }

        static void Main(string[] args)
        {
            CreateCommand("select Foo, Bar from Spam", "Server=.;Database=Test;Trusted_Connection=True;");
            GetCount("Spam", "Server=.;Database=Test;Trusted_Connection=True;");

            Console.WriteLine("Press key...");
            Console.ReadKey();
        }
    }
}
