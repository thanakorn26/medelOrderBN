using System;
using Microsoft.Data.SqlClient;

public class SqlConnectionTester
{
    public static void TestConnection()
    {
        var connectionString = "Server=localhost,1433;Database=OrderManagement;User Id=sa;Password=test@Database;TrustServerCertificate=True;";

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }
}
