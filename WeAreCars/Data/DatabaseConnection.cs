using System;
using System.Data.SqlClient;

namespace WeAreCars.Data
{
    /*
     * Class for database connection
     * (First you need to make database, for do that please read README.md)
     */
    public static class DBConnection
    {


        // Using windows authentication for WeAreCars database
        private static string connString = @"Server=localhost\SQLEXPRESS;Database=WeAreCars;Integrated Security=True;";




        //Function for make sql connection to database.
        public static SqlConnection Connect()
        {
            return new SqlConnection(connString);
        }


    }
}
