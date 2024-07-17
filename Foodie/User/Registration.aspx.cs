using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
    public partial class Registration : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable dataTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // second if checks whether a user is already logged in // && Session["userId"] != null
                if (Request.QueryString["id"] != null )
                {
                    getUserDetails();
                }
                else if (Session["userId"] != null)
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtention = string.Empty;
            bool isValidToExecute = false;
            int userId = Convert.ToInt32(Request.QueryString["id"]);
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("User_Crud", connection);
            command.Parameters.AddWithValue("@Action", userId == 0 ? "INSERT" : "UPDATE" );
            command.Parameters.AddWithValue("@UserId", userId );
            command.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            command.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
            command.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
            command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            command.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            command.Parameters.AddWithValue("@PostCode", txtPostCode.Text.Trim());
            command.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
            if(fuUserImage.HasFile)
            {
                if (Utils.IsValidExtension(fuUserImage.FileName))
                {
                    Guid guid = Guid.NewGuid();
                    fileExtention = Path.GetExtension(fuUserImage.FileName);
                    imagePath = "Images/User/" + guid.ToString() + fileExtention;
                    fuUserImage.PostedFile.SaveAs(Server.MapPath("~/Images/User/") + guid.ToString() + fileExtention);
                    command.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select .jpg .jpeg or .png image";
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
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    actionName = userId == 0 ?
                        " registration is successful! <b><a href='Login.aspx'>Click here</a></b> to do login" :
                        " details updated successfully <b><a href='Profile.aspx'>Can check here</a></b>";
                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtUsername.Text.Trim() + "</b>" + actionName;
                    lblMsg.CssClass = "alert alert-success";
                    if(userId != 0)
                    {
                        Response.AddHeader("REFRESH", "1;URL=Profile.aspx");        // refresh after 1 second
                    }
                    clear();
                }
                catch(SqlException ex)
                {
                    if(ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "<b>" + txtUsername.Text.Trim() + "</b> username already exist, try new one..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                catch(Exception ex)
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

        void getUserDetails()
        {
            connection = new SqlConnection(Connection.GetConnectionString());
            command = new SqlCommand("User_Crud", connection);
            command.Parameters.AddWithValue("@Action", "SELECT4PROFILE");
            command.Parameters.AddWithValue("@UserId", Request.QueryString["id"]);
            command.CommandType = CommandType.StoredProcedure;
            adapter = new SqlDataAdapter(command);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            if (dataTable.Rows.Count == 1)
            {
                txtName.Text = dataTable.Rows[0]["name"].ToString();
                txtUsername.Text = dataTable.Rows[0]["username"].ToString();
                txtMobile.Text = dataTable.Rows[0]["mobile"].ToString();
                txtEmail.Text = dataTable.Rows[0]["email"].ToString();
                txtAddress.Text = dataTable.Rows[0]["address"].ToString();
                txtPostCode.Text = dataTable.Rows[0]["post_code"].ToString();
                imgUser.ImageUrl = string.IsNullOrEmpty(dataTable.Rows[0]["image_url"].ToString()) 
                    ? "../Images/no_image.jpg" : "../" + dataTable.Rows[0]["image_url"].ToString();
                imgUser.Height = 200;
                imgUser.Width = 200;
                txtPassword.TextMode = TextBoxMode.SingleLine;
                txtPassword.ReadOnly = true;
                txtPassword.Text = dataTable.Rows[0]["password"].ToString();
            }
            lblHeaderMsg.Text = "<h2>Edit Profile</h2>";
            btnRegister.Text = "Update";
            lblAlreadyUser.Text = "";
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPostCode.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
    }
}