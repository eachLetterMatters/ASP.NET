using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Foodie.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter dataAdapter;
        DataTable dataTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)    // check if page is loaded for the first time
            {
                // displayed in Admin.Master breadCrum
                Session["breadCrum"] = "Product";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                    getProducts();
                }
            }
            lblMsg.Visible = false;
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int productId = Convert.ToInt32(hdnId.Value);
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Product_Crud", connection);
            command.Parameters.AddWithValue("@Action", productId == 0 ? "INSERT" : "UPDATE");
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            command.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
            command.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
            command.Parameters.AddWithValue("@Quantity", txtQuantity.Text.Trim());
            command.Parameters.AddWithValue("@CategoryId", ddlCategories.SelectedValue);
            command.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            command.CommandType = CommandType.StoredProcedure;

            if (fuProductImage.HasFile)
            {
                if (Utils.IsValidExtension(fuProductImage.FileName))
                {
                    Guid guid = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuProductImage.FileName);
                    imagePath = "Images/Product/" + guid.ToString() + fileExtension;
                    fuProductImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + guid.ToString() + fileExtension);
                    command.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select .jpg, .jpeg or .png image";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else
            {
                isValidToExecute = true;
            }

            if (isValidToExecute)
            {
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    actionName = productId == 0 ? "inserted" : "updated";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Product " + actionName + " successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getProducts();
                    clear();
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

        private void getProducts()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Product_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECT");
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            repeaterProduct.DataSource = dataTable;
            repeaterProduct.DataBind();
        }
        private void clear()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            ddlCategories.ClearSelection();
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            imgProduct.ImageUrl = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void repeaterProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            connection = new SqlConnection(Connection.GetConnectionString());
            if (e.CommandName == "edit")
            {
                command = new SqlCommand("Product_Crud", connection);
                command.Parameters.AddWithValue("@Action", "GETBYID");
                command.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                command.CommandType = CommandType.StoredProcedure;
                dataAdapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                // set form content according to element we want to edit
                txtName.Text = dataTable.Rows[0]["name"].ToString();
                txtDescription.Text = dataTable.Rows[0]["description"].ToString();
                txtPrice.Text = dataTable.Rows[0]["price"].ToString();
                txtQuantity.Text = dataTable.Rows[0]["quantity"].ToString();
                ddlCategories.SelectedValue = dataTable.Rows[0]["category_id"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dataTable.Rows[0]["is_active"]);
                imgProduct.ImageUrl = string.IsNullOrEmpty(dataTable.Rows[0]["image_url"].ToString()) ?
                    "../Images/no_image.jpg" : "../" + dataTable.Rows[0]["image_url"].ToString();
                imgProduct.Height = 200;
                imgProduct.Width = 200;
                hdnId.Value = dataTable.Rows[0]["product_id"].ToString();
                btnAddOrUpdate.Text = "Update";
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";
            }
            else if (e.CommandName == "delete")
            {
                command = new SqlCommand("Product_Crud", connection);
                command.Parameters.AddWithValue("@Action", "DELETE");
                command.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Product deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getProducts();
                    clear();
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

        protected void repeaterProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblIsActive = e.Item.FindControl("lblIsActive") as Label;
                Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;
                if (lblIsActive.Text == "True")
                {
                    lblIsActive.Text = "Active";
                    lblIsActive.CssClass = "badge badge-success";
                }
                else
                {
                    lblIsActive.Text = "Inactive";
                    lblIsActive.CssClass = "badge badge-danger";
                }
                if (Convert.ToInt32(lblQuantity.Text) < 5)
                {
                    lblQuantity.CssClass = "badge badge-danger";
                    lblQuantity.ToolTip = "Item about to be 'Out of stock'!";
                }
            }
        }
    }
}