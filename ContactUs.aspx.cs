using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assg1
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string recaptchaResponse = Request["g-recaptcha-response"];
            string secretKey = "6LdzyqgqAAAAAA-Y662v9S11MhLCjRfpA9knX3Ia"; // Replace with actual secret key

            if (string.IsNullOrEmpty(recaptchaResponse) || !ValidateRecaptcha(recaptchaResponse, secretKey))
            {
                ShowAlert("Please complete the reCAPTCHA correctly.");
                return;
            }

            if (!IsValidEmail(TxtEmail.Text))
            {
                ShowAlert("Invalid email address.");
                return;
            }

            if (!IsValidPhoneNumber(TxtPhoneNumber.Text))
            {
                ShowAlert("Invalid phone number.");
                return;
            }

            string[] filePaths = ProcessFileUploads();

            string connectionString = ConfigurationManager.ConnectionStrings["ContactUsDB"].ConnectionString;

            try
            {
                SaveFeedback(connectionString, filePaths);
                ShowAlert("Form submitted successfully!");
            }
            catch (Exception ex)
            {
                ShowAlert($"Error: {ex.Message}");
            }
        }

        private bool ValidateRecaptcha(string recaptchaResponse, string secretKey)
        {
            string apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";
            using (var client = new WebClient())
            {
                var jsonResponse = client.DownloadString(apiUrl);
                var serializer = new JavaScriptSerializer();
                dynamic jsonData = serializer.Deserialize<dynamic>(jsonResponse);
                return jsonData["success"] == true;
            }
        }

        private string[] ProcessFileUploads()
        {
            string[] filePaths = new string[3];
            for (int i = 1; i <= 3; i++)
            {
                var fileUploadControl = (FileUpload)FindControl("FileUpload" + i);
                if (fileUploadControl != null && fileUploadControl.HasFile)
                {
                    string fileName = Path.GetFileName(fileUploadControl.FileName);
                    string filePath = Server.MapPath("~/Uploads/") + fileName;
                    fileUploadControl.SaveAs(filePath);
                    filePaths[i - 1] = filePath;
                }
            }
            return filePaths;
        }

        private void SaveFeedback(string connectionString, string[] filePaths)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO ContactUs (Number, FirstName, LastName, Email, PhoneNumber, Message, FilePath1, FilePath2, FilePath3) " +
                               "VALUES (@Number, @FirstName, @LastName, @Email, @PhoneNumber, @Message, @FilePath1, @FilePath2, @FilePath3)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        // Generate a unique Number for each feedback
                        int feedbackNumber = GetNextFeedbackNumber(conn);

                        cmd.Parameters.AddWithValue("@Number", feedbackNumber);
                        cmd.Parameters.AddWithValue("@FirstName", TxtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastName", TxtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@PhoneNumber", TxtPhoneNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@Message", TxtMessage.Text.Trim());
                        cmd.Parameters.AddWithValue("@FilePath1", filePaths[0] ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FilePath2", filePaths[1] ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FilePath3", filePaths[2] ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (log it, show a message, etc.)
                        ShowAlert($"Error while saving feedback: {ex.Message}");
                    }
                }
            }
        }

        private int GetNextFeedbackNumber(SqlConnection conn)
        {
            string query = "SELECT ISNULL(MAX(Number), 0) + 1 FROM ContactUs"; // Generates the next available Number
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
        private void ShowAlert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 7; // Simple validation
        }
    }
}
