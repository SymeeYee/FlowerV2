using System;
using System.Collections.Generic;
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

            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please complete the reCAPTCHA.');", true);
                return;
            }

            try
            {
                string apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";
                using (var client = new WebClient())
                {
                    var jsonResponse = client.DownloadString(apiUrl);
                    var serializer = new JavaScriptSerializer();
                    dynamic jsonData = serializer.Deserialize<dynamic>(jsonResponse);

                    if (jsonData["success"] != true)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('reCAPTCHA validation failed. Please try again.');", true);
                        return;
                    }
                }

                if (!IsValidEmail(TxtEmail.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid email address.');", true);
                    return;
                }

                if (!IsValidPhoneNumber(TxtPhoneNumber.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid phone number.');", true);
                    return;
                }

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Form submitted successfully!');", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
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
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 7; // Example validation
        }
    }
}
