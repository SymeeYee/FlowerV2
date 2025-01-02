using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;


namespace Assg1
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Remove soon
                Session["custID"] = "1001";

                // Get customer ID from session
                string strCustId = Session["custID"].ToString();

                // Add try catch to check if able to parse customer ID
                int custId = Int32.Parse(strCustId);

                string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;

                // Use DISTINCT to avoid duplicates
                string query = $@"
            SELECT C.CartID, C.ProductID, C.Quantity, C.Size , P.DiscountPrice, P.ImageURL, P.ProductName, P.Color
            FROM dbo.Cart C
            JOIN dbo.Products P ON P.ProductID = C.ProductID
            WHERE C.CustomerID = {custId};
        ";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            CartRepeater.DataSource = reader;
                            CartRepeater.DataBind();
                        }
                        else
                        {
                            CartRepeater.DataSource = null;
                            CartRepeater.DataBind();
                            Response.Write("<script>alert('No cart item found.');</script>");
                        }
                    }
                }
            }

            if (Request.HttpMethod == "POST")
            {
                string cartId = Request.Form["cartId"];
                if (!string.IsNullOrEmpty(cartId))
                {
                    // Remove the item from the cart based on CartID
                    string connectionString = ConfigurationManager.ConnectionStrings["ProductsDB"].ConnectionString;
                    string query = "DELETE FROM dbo.Cart WHERE CartID = @CartID";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartID", cartId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // Return a success response (optional, can be customized)
                    Response.StatusCode = 200;
                    Response.End();
                }
            }
        }

        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //Response.Write($"<script>alert('Error loading products: {ex.Message}');</script>");
        //    }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string selectedDestination = ddlDestination.SelectedValue;
            if (!string.IsNullOrEmpty(selectedDestination))
            {
                lblMessage.Text = "You have selected: " + selectedDestination;
            }
            else
            {
                lblMessage.Text = "Please select a destination.";
            }

        }
    }
}
