using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Foodie
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        }
    }

    public class Utils
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;

        public static bool IsValidExtension(string fileName)
        {
            bool isValid = false;
            string[] fileExtension = { ".jpg", ".png", ".jpeg" };
            for (int i = 0;  i < fileExtension.Length; i++) 
            {
                if (fileName.Contains(fileExtension[i]))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

        public static string GetImageUrl(Object url)
        {
            string returnedUrl = "";
            if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                returnedUrl = "../Images/no_image.jpg";
            }
            else
            {
                returnedUrl = string.Format("../{0}", url);
            }
            return returnedUrl;
        }
        
        public bool updateCartQuantity(int quantity, int productId, int userId)
        {
            bool isUpdated = false;
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Cart_Crud", connection);
            command.Parameters.AddWithValue("@Action", "UPDATE");
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@UserId", userId);
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                isUpdated = true;
            }
            catch (Exception ex)
            {
                isUpdated = false;
                System.Web.HttpContext.Current.Response.Write("<script>alert('Error - " + ex.Message + "');</script>");
            } 
            finally
            {
                connection.Close();
            }
            return isUpdated;
        }
    }
}