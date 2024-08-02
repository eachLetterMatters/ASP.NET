using Foodie.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
    public partial class Cart : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;
        decimal grandTotal = 0;

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
                    getCartItems();
                }
            }
        }

        void getCartItems()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Cart_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECT");
            command.Parameters.AddWithValue("@UserId", Session["userId"]);
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            repeaterCart.DataSource = dataTable;
            if (dataTable.Rows.Count == 0)
            {
                repeaterCart.FooterTemplate = null;
                repeaterCart.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            repeaterCart.DataBind();
        }

        protected void repeaterCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Utils utils = new Utils();
            if (e.CommandName == "remove")
            {
                connection = new SqlConnection(Connection.GetConnectionString());
                command = new SqlCommand("Cart_Crud", connection);
                command.Parameters.AddWithValue("@Action", "DELETE");
                command.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                command.Parameters.AddWithValue("@UserId", Session["userId"]);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    getCartItems();
                    Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["userId"]));
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error - " + ex.Message + "');</script>");
                }
                finally
                {
                    connection.Close();
                }
            }
            else if (e.CommandName == "updateCart")
            {
                bool isCartUpdated = false;
                for (int i = 0; i < repeaterCart.Items.Count; i++)
                {
                    if (repeaterCart.Items[i].ItemType == ListItemType.Item || repeaterCart.Items[i].ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox quantity = repeaterCart.Items[i].FindControl("txtQuantity") as TextBox;
                        HiddenField _productId = repeaterCart.Items[i].FindControl("hdnProductId") as HiddenField;
                        HiddenField _quantity = repeaterCart.Items[i].FindControl("hdnQuantity") as HiddenField;
                        int quantityFromCart = Convert.ToInt32(quantity.Text);
                        int productId = Convert.ToInt32(_productId.Value);
                        int quantityFromDB = Convert.ToInt32(_quantity.Value);
                        if (quantityFromCart != quantityFromDB)
                        {
                            isCartUpdated = utils.updateCartQuantity(quantityFromCart, productId, Convert.ToInt32(Session["userId"]));
                        }
                    }
                }
                getCartItems();
            }
            else if (e.CommandName == "checkout")
            {
                bool runQuery = false;
                string pName = string.Empty;
                // check item quantity
                for (int i = 0; i < repeaterCart.Items.Count; i++)
                {
                    if (repeaterCart.Items[i].ItemType == ListItemType.Item || repeaterCart.Items[i].ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField _productId = repeaterCart.Items[i].FindControl("hdnProductId") as HiddenField;
                        HiddenField _cartQuantity = repeaterCart.Items[i].FindControl("hdnQuantity") as HiddenField;
                        HiddenField _productQuantity = repeaterCart.Items[i].FindControl("hdnPrdQuantity") as HiddenField;
                        Label productName = repeaterCart.Items[i].FindControl("lblName") as Label;
                        int cartQuantity = Convert.ToInt32(_cartQuantity.Value);
                        int productQuantity = Convert.ToInt32(_productQuantity.Value);

                        if (productQuantity > cartQuantity && productQuantity > 2)
                        {
                            runQuery = true;
                        }
                        else
                        {
                            runQuery = false;
                            pName = productName.Text.ToString();
                            break;
                        }
                    }
                }
                if (runQuery)
                {
                    Response.Redirect("Payment.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Item <b>'" + pName + "'</b> is out of stock :(";
                    lblMsg.CssClass = "alert alert-warning";
                }

            }
        }

        protected void repeaterCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label totalPrice = e.Item.FindControl("lblTotalPrice") as Label;
                Label productPrice = e.Item.FindControl("lblPrice") as Label;
                TextBox quantity = e.Item.FindControl("txtQuantity") as TextBox;
                decimal calTotalPrice = Convert.ToDecimal(productPrice.Text) * Convert.ToDecimal(quantity.Text);
                totalPrice.Text = calTotalPrice.ToString();
                grandTotal += calTotalPrice;
            }
            Session["grandTotalPrice"] = grandTotal;
        }

        // custom template class to add controls to the repeater's header, item and footer sections
        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType {  get; set; }

            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {
                if(ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Your Cart is empty.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Continue Shopping</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }

        }
    }
}