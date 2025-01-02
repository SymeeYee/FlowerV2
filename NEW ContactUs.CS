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
            //using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ContactUsDB"].ConnectionString))
            //{
            //    conn.Open();
            //    string query = "INSERT INTO Feedback (FirstName, LastName, Email, PhoneNumber, Message, FilePath1, FilePath2, FilePath3, TimeFeedback, Responded, TimeResponded) " +
            //       "VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Message, @FilePath1, @FilePath2, @FilePath3, @TimeFeedback, @Responded, @TimeResponded)";

            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {

            //        cmd.Parameters.AddWithValue("@FirstName", "John");
            //        cmd.Parameters.AddWithValue("@LastName", "Doe");
            //        cmd.Parameters.AddWithValue("@Email", "johndoe@example.com");
            //        cmd.Parameters.AddWithValue("@PhoneNumber", "1234567890");
            //        cmd.Parameters.AddWithValue("@Message", "This is a test message.");
            //        cmd.Parameters.AddWithValue("@FilePath1", DBNull.Value);
            //        cmd.Parameters.AddWithValue("@FilePath2", DBNull.Value);
            //        cmd.Parameters.AddWithValue("@FilePath3", DBNull.Value);
            //        cmd.Parameters.AddWithValue("@TimeFeedback", DateTime.Now); // Adds current timestamp
            //        cmd.Parameters.AddWithValue("@Responded", false); // Assuming not responded yet
            //        cmd.Parameters.AddWithValue("@TimeResponded", DBNull.Value); // NULL since no response yet

            //        cmd.ExecuteNonQuery();
            //        // This method is called when the page loads
            //    }
            //}
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Step 1: Validate reCAPTCHA
            string recaptchaResponse = Request["g-recaptcha-response"];
            string secretKey = "6LdzyqgqAAAAAA-Y662v9S11MhLCjRfpA9knX3Ia"; // Replace with your actual secret key

            if (!ValidateRecaptcha(recaptchaResponse, secretKey))
            {
                ShowAlert("Please complete the reCAPTCHA correctly.");
                return;
            }

            // Step 2: Validate inputs
            if (!ValidateInputs())
            {
                return; // Validation errors already displayed
            }

            // Step 3: Process file uploads
            string[] filePaths = ProcessFileUploads();

            // Step 4: Get the database connection string
            string connectionString = ConfigurationManager.ConnectionStrings["ContactUsDB"].ConnectionString;

            try
            {
                // Step 5: Save feedback to the database
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
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                return false;
            }

            string apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";
            using (var client = new WebClient())
            {
                var jsonResponse = client.DownloadString(apiUrl);
                var serializer = new JavaScriptSerializer();
                dynamic jsonData = serializer.Deserialize<dynamic>(jsonResponse);
                return jsonData["success"] == true;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) || string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                ShowAlert("First name and last name are required.");
                return false;
            }

            if (!IsValidEmail(TxtEmail.Text))
            {
                ShowAlert("Invalid email address.");
                return false;
            }

            if (!IsValidPhoneNumber(TxtPhoneNumber.Text))
            {
                ShowAlert("Phone number must not be empty and must be between 10 and 12 digits.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtMessage.Text))
            {
                ShowAlert("Message cannot be empty.");
                return false;
            }

            return true;
        }

        private string[] ProcessFileUploads()
        {
            string[] filePaths = new string[3];
            string uploadDir = Server.MapPath("~/Uploads/");

            for (int i = 1; i <= 3; i++)
            {
                var fileUploadControl = (FileUpload)FindControl("FileUpload" + i);
                if (fileUploadControl != null && fileUploadControl.HasFile)
                {
                    string fileName = Path.GetFileName(fileUploadControl.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    try
                    {
                        fileUploadControl.SaveAs(filePath);
                        filePaths[i - 1] = filePath;
                    }
                    catch (Exception ex)
                    {
                        ShowAlert($"Error uploading file {fileName}: {ex.Message}");
                    }
                }
            }

            return filePaths;
        }

        private void SaveFeedback(string connectionString, string[] filePaths)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ContactUsDB"].ConnectionString))
            {
                conn.Open();

                bool turnOnIdentity = TurnOnIdentity(conn);

                if (!turnOnIdentity) return;

                string query = "INSERT INTO Feedback (FeedbackID, FirstName, LastName, Email, PhoneNumber, Message, FilePath1, FilePath2, FilePath3, TimeFeedback, Responded) " +
                                "VALUES (@FeedbackID, @FirstName, @LastName, @Email, @PhoneNumber, @Message, @FilePath1, @FilePath2, @FilePath3, @TimeFeedback, @Responded)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int feedbackNumber = GetNextFeedbackNumber(conn);

                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackNumber);
                    cmd.Parameters.AddWithValue("@FirstName", TxtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", TxtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@PhoneNumber", TxtPhoneNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Message", TxtMessage.Text.Trim());
                    cmd.Parameters.AddWithValue("@FilePath1", filePaths[0] ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FilePath2", filePaths[1] ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FilePath3", filePaths[2] ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TimeFeedback", DateTime.Now); // Adds current timestamp
                    cmd.Parameters.AddWithValue("@Responded", false); // Assuming not responded yet
                    cmd.Parameters.AddWithValue("@TimeResponded", DBNull.Value); // NULL since no response yet

                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException ex)
                    {
                        ShowAlert($"Database error: {ex.Message}");
                    }
                }

                bool turnOffIdentity = TurnOffIdentity(conn);

                if (!turnOffIdentity) return;
            }
        }

        private bool TurnOnIdentity(SqlConnection conn)
        {
            string openIdInsQuery = "SET IDENTITY_INSERT Feedback ON";

            using (SqlCommand cmd = new SqlCommand(openIdInsQuery, conn))
            {

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    ShowAlert($"Database error: {ex.Message}");
                    return false;
                }
            }
        }

        private bool TurnOffIdentity(SqlConnection conn)
        {
            string offIdInsQuery = "SET IDENTITY_INSERT Feedback OFF";

            using (SqlCommand cmd = new SqlCommand(offIdInsQuery, conn))
            {

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    ShowAlert($"Database error: {ex.Message}");
                    return false;
                }
            }
        }

        private int GetNextFeedbackNumber(SqlConnection conn)
        {
            string query = "SELECT ISNULL(MAX(FeedbackID), 0) + 1 FROM Feedback";
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
            // Check if the phone number is not empty, contains only digits, and has a length between 10 and 12
            return !string.IsNullOrWhiteSpace(phoneNumber) &&
                   phoneNumber.All(char.IsDigit) &&
                   phoneNumber.Length >= 10 &&
                   phoneNumber.Length <= 12;
        }

        protected void DdlCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle country code selection change if necessary
        }
    }
}
