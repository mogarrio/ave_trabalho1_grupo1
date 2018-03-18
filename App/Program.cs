using System;
using System.Data;
using System.Data.SqlClient;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = @"
                    Server=(LocalDB)\MSSQLLocalDB;
                    Integrated Security=true;
                    AttachDbFileName=" +
                        Environment.CurrentDirectory +
                        "\\data\\NORTHWND.MDF";

            SqlConnection con = new SqlConnection(connStr);
            try
            {
                /*
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT ProductID, ProductName FROM Products";
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    Console.WriteLine(dr["ProductName"]);*/

                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandText = "SELECT RegionID, RegionDescription FROM Region";
                con.Open();
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                    Console.WriteLine(dr2["RegionID"] + ", " + dr2["RegionDescription"] + "-----");
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

        }
    }
}
