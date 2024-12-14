using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace AsgV3
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string recaptchaResponse = Request["g-recaptcha-response"];
            string secretKey = "YOUR_SECRET_KEY"; // Replace with your secret key

            string apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string jsonResponse = reader.ReadToEnd();
                dynamic json = JsonConvert.DeserializeObject(jsonResponse);

                if (json.success == true)
                {
                    // Success: Process the form submission
                    Response.Write("<script>alert('Form submitted successfully.');</script>");
                }
                else
                {
                    // Failure: Show error message
                    Response.Write("<script>alert('Failed reCAPTCHA verification. Please try again.');</script>");
                }
            }
        }
    }

    }
