﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ManagementApp;

namespace WarehouseManagement
{   
    public class DBAccess
    {

        public void UpdateProductData(Product product)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = GetConnectionString();
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("uspUpdateProductData"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Available", SqlDbType.Int).Value = product.Availability;
                    cmd.Parameters.Add("@DPCI", SqlDbType.NVarChar).Value = product.Dpci;

                    cmd.ExecuteNonQuery();

                }
            }
        }

        public DataTable GetProductList()

        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = GetConnectionString();

                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("uspGetProductList"))
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        dt = GetData(cmd);
                    }
                }

            }
            catch (Exception)
            {

                return null;
            }
            return dt;
        }


        private DataTable GetData(SqlCommand cmd)
        {
            DataTable dataTable = new DataTable();

            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dataTable);
            }
            catch (Exception)
            {
                return null;

            }
            return dataTable;
        }

        private string GetConnectionString()
        {
            string conString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            return conString;

            //A connection string is an expression that contains the parameters required 
            // for the applications to connect a database server
            //Connection string typically includes the server instance, database name, 
            //authentication details, and some other settings to communicate with the database
            //They are normally stored inside a configuration file somewhere in the application
        }
    }
}
