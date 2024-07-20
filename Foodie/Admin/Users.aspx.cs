using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Users";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                    getUsers();
                }
            }
        }

        private void getUsers()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("User_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECT4ADMIN");
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            repeaterUsers.DataSource = dataTable;
            repeaterUsers.DataBind();
        }

        protected void repeaterUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                connection = new SqlConnection(Connection.GetConnectionString());
                command = new SqlCommand("User_Crud", connection);
                command.Parameters.AddWithValue("@Action", "DELETE");
                command.Parameters.AddWithValue("@UserId", e.CommandArgument);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "User deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getUsers();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}