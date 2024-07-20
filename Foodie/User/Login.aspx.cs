using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] != null)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // TODO: handle Admin login better
            if(txtUsername.Text.Trim() == "Admin" && txtPassword.Text.Trim() == "123")
            {
                Session["admin"] = txtUsername.Text.Trim();
                Response.Redirect("../Admin/Dashboard.aspx");
            }
            else
            {
                connection = new SqlConnection(Connection.GetConnectionString());
                command = new SqlCommand("User_Crud", connection);
                command.Parameters.AddWithValue("@Action", "SELECT4LOGIN");
                command.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                command.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                command.CommandType = CommandType.StoredProcedure;
                dataAdapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count == 1)
                {
                    Session["username"] = txtUsername.Text.Trim();
                    Session["userId"] = dataTable.Rows[0]["user_id"];
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Invalid Credentials!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
        }
    }
}