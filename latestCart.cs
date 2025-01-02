using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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
                SELECT C.CartID, C.ProductID, C.Quantity, P.DiscountPrice, P.ImageURL, P.ProductName
                FROM dbo.Cart C, dbo.Products  P WHERE C.CustomerID = {custId} AND P.ProductID = C.ProductID;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Add parameters if provided
                //if (parameters != null)
                //{
                //    cmd.Parameters.AddRange(parameters.ToArray());
                //}

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
