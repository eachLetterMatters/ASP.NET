using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.Admin
{
    public partial class Category : System.Web.UI.Page
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
                Session["breadCrum"] = "Category";
                getCategories();
            }
            lblMsg.Visible = false;
        }

        protected void btnAddOrUpdateClick(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value);
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Category_Crud", connection);
            command.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
            command.Parameters.AddWithValue("@CategoryId", categoryId);
            command.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            command.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            command.CommandType = CommandType.StoredProcedure;

            if (fuCategoryImage.HasFile)
            {
                if (Utils.IsValidExtension(fuCategoryImage.FileName))
                {
                    Guid guid = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                    imagePath = "Images/Category/" + guid.ToString() + fileExtension;
                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + guid.ToString() + fileExtension);
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
                    actionName = categoryId == 0 ? "inserted" : "updated";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category " + actionName + " successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
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

        private void getCategories()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("Category_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECT");
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            repeaterCategory.DataSource = dataTable;
            repeaterCategory.DataBind();
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            imgCategory.ImageUrl = string.Empty;
        }

        protected void btnClearClick(object sender, EventArgs e)
        {
            clear();
        }

        protected void repeaterCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            connection = new SqlConnection(Connection.GetConnectionString());
            if (e.CommandName == "edit")
            {
                command = new SqlCommand("Category_Crud", connection);
                command.Parameters.AddWithValue("@Action", "GETBYID");
                command.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                command.CommandType = CommandType.StoredProcedure;
                dataAdapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                // set form content according to element we want to edit
                txtName.Text = dataTable.Rows[0]["name"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dataTable.Rows[0]["is_active"]);
                imgCategory.ImageUrl = string.IsNullOrEmpty(dataTable.Rows[0]["image_url"].ToString()) ? 
                    "../Images/no_image.jpg" : "../" + dataTable.Rows[0]["image_url"].ToString();
                imgCategory.Height = 200;
                imgCategory.Width = 200;
                hdnId.Value = dataTable.Rows[0]["category_id"].ToString();
                btnAddOrUpdate.Text = "Update";
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";
            }
            else if (e.CommandName == "delete")
            {
                command = new SqlCommand("Category_Crud", connection);
                command.Parameters.AddWithValue("@Action", "DELETE");
                command.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Category deleted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                    clear();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass= "alert alert-danger";
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        protected void repeaterCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl = e.Item.FindControl("lblIsActive") as Label;
                if(lbl.Text == "True")
                {
                    lbl.Text = "Active";
                    lbl.CssClass = "badge badge-success";
                }
                else
                {
                    lbl.Text = "Inactive";
                    lbl.CssClass = "badge badge-danger";
                }
            }
        }
    }
}