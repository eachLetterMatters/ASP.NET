using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
    public partial class Default : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getCategories();
            }
        }

        private void getCategories()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Category_Crud", connection);
            //command = new SqlCommand("Product_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECTACTIVE");
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            repeaterCategory.DataSource = dataTable;
            repeaterCategory.DataBind();
        }
    }
}