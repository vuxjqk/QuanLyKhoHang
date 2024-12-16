using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyKhoHang
{
    class DatabaseConnection
    {
        string conStr;
        SqlConnection con;

        public DatabaseConnection()
        {
            conStr = "Data Source=DELL\\SQLEXPRESS;Initial Catalog=QuanLyKhoHang;Integrated Security=True";
            con = new SqlConnection(conStr);
        }

        public void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
        }

        public void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
        }

        public int GetNonQuery(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                OpenConnection();
                int rowsAffected = cmd.ExecuteNonQuery();
                CloseConnection();
                return rowsAffected;
            }
            catch
            {
                return 0;
            }
        }

        public object GetScalar(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                OpenConnection();
                object result = cmd.ExecuteScalar();
                CloseConnection();
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public DataTable GetDataTable(string sql)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public int UpdateDataTable(string sql, DataTable dt)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                return adapter.Update(dt);
            }
            catch
            {
                return 0;
            }
        }
    }
}
