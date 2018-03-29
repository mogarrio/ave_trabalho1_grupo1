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

               /* 
                SqlCommand cmd2 = con.CreateCommand();
                cmd2.CommandText = "INSERT INTO Region(" + "RegionDescription" + ") OUTPUT INSERTED.RegionID VALUES " + "('Central')";
                con.Open();
                int i = cmd2.ExecuteNonQuery();
                Console.WriteLine(i);*/


                SqlCommand cmd3 = con.CreateCommand();
                cmd3.CommandText = @"SELECT CategoryID, CategoryName, Description FROM Categories";
                con.Open();
                SqlDataReader dr3 = cmd3.ExecuteReader();
                while (dr3.Read())
                    Console.WriteLine(dr3["CategoryID"] + ", " + dr3["CategoryName"] + ", " + dr3["Description"] + "-----");
                dr3.Close();

            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

        }
    }
}
