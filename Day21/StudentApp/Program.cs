 using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string conString =
            "Server=localhost;Database=CollegeDB;Integrated Security=True;TrustServerCertificate=True";

        using SqlConnection con = new SqlConnection(conString);
        con.Open();

        string query =
            "INSERT INTO Students(Name, Age) VALUES(@name, @age)";

        using SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@name", "Yogesh");
        cmd.Parameters.AddWithValue("@age", 23);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Record Inserted Successfully");
    }
}
