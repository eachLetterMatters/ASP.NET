using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Foodie.Admin;

namespace Foodie.User
{
    public partial class Menu : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getProducts();
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

        private void getProducts()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Product_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECTACTIVE");
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            repeaterProducts.DataSource = dataTable;
            repeaterProducts.DataBind();
        }

        protected void repeaterProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["userId"] != null)
            {
                bool isCartItemUpdated = false;
                int i = isItemInCart(Convert.ToInt32(e.CommandArgument));
                if(i == 0)
                {
                    // Adding new item in cart
                    connection = new SqlConnection(Connection.GetConnectionString());
                    command = new SqlCommand("Cart_Crud", connection);
                    command.Parameters.AddWithValue("@Action", "INSERT");
                    command.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                    command.Parameters.AddWithValue("@Quantity", 1);
                    command.Parameters.AddWithValue("@UserId", Session["userId"]);
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        Response.Write("<script>alert('Error - " + ex.Message + "');</script>");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                else
                {
                    // Updating number of items in cart
                    Utils utils = new Utils();
                    isCartItemUpdated = utils.updateCartQuantity(++i, Convert.ToInt32(e.CommandArgument), Convert.ToInt32(Session["userId"]));
                }
                lblMsg.Visible = true;
                lblMsg.Text = "Item added successfully in your cart";
                lblMsg.CssClass = "alert alert-success";
                Response.AddHeader("REFRESH", "1;URL=Cart.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        int isItemInCart(int productId)
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Cart_Crud", connection);
            command.Parameters.AddWithValue("@Action", "GETBYID");
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@UserId", Session["userId"]);
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            int quantity = 0;
            if (dataTable.Rows.Count > 0)
            {
                quantity = Convert.ToInt32(dataTable.Rows[0]["quantity"]);
            }
            return quantity;
        }


    }
}