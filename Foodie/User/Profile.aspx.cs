using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Foodie.User
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable dataTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    getUserDetails();
                }
            }

        }

        void getUserDetails()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("User_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECT4PROFILE");
            command.Parameters.AddWithValue("@UserId", Session["userId"]);
            command.CommandType = CommandType.StoredProcedure;
            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            repeaterUserProfile.DataSource = dataTable;
            repeaterUserProfile.DataBind();

            if (dataTable.Rows.Count == 1)
            {
                Session["name"] = dataTable.Rows[0]["name"].ToString();
                Session["email"] = dataTable.Rows[0]["email"].ToString();
                Session["imgUrl"] = dataTable.Rows[0]["image_url"].ToString();
                Session["createdDate"] = dataTable.Rows[0]["created_date"].ToString();
            }
            
        }
    }
}