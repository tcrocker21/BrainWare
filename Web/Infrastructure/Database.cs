using System;

namespace Web.Infrastructure
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    public static class Database
    {
        /// <summary>
        /// Returns a dataset of query results. Multiple queries will return as multiple tables in the resulting dataset.
        /// </summary>
        /// <param name="query">SQL Query as a string.</param>
        /// <param name="cmdType">CommandType of the query.</param>
        /// <param name="parameters">Array of SqlParameters.</param>
        /// <returns>A Dataset containing the results of the query.</returns>
        public static DataSet ExecuteReader(string query, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalHost"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                DataSet ds = new DataSet();

                cmd.CommandType = cmdType;
                foreach(SqlParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                try
                {
                    con.Open();
                    new SqlDataAdapter(cmd).Fill(ds);
                }
                catch (Exception ex)
                {
                    //TODO: Set up logging for errors.
                }
                return ds;                                

            }

        }

        /// <summary>
        /// Executes the provided query and returns the number of affected rows.
        /// </summary>
        /// <param name="query">SQL Query as a string.</param>
        /// <param name="cmdType">CommandType of the query.</param>
        /// <param name="parameters">Array of SqlParameters.</param>q
        /// <returns>The number of rows affected by the query.</returns>
        public static int ExecuteNonQuery(string query, CommandType cmdType, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalHost"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                int rowsAffected = 0;

                cmd.CommandType = cmdType;
                foreach (SqlParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                try
                {
                    con.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //TODO: Set up logging for errors.
                }

                return rowsAffected;

            }                        
        }

    }
}