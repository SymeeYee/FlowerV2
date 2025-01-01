using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Assg1
{
    public partial class AdminContactUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFeedbackData();
            }
        }

        private void LoadFeedbackData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FeedbackID, Responded FROM dbo.Feedback"; // Adjust query as needed
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int totalResponded = 0;
                    int totalUnresponded = 0;

                    while (reader.Read())
                    {
                        int feedbackID = reader.GetInt32(0);
                        bool responded = reader.GetBoolean(1);

                        // Update counts
                        if (responded)
                        {
                            totalResponded++;
                        }
                        else
                        {
                            totalUnresponded++;
                        }

                        // Create a new row in the feedback table
                        feedbackTable.InnerHtml += $"<tr><td>{feedbackID}</td></tr>";
                    }

                    // Display totals
                    totalRespondedLabel.InnerText = $"Total Responded: {totalResponded}";
                    // Update percentage display
                    UpdateFeedbackSummary(totalResponded, totalUnresponded);
                }
            }
        }

        private void UpdateFeedbackSummary(int totalResponded, int totalUnresponded)
        {
            int totalFeedback = totalResponded + totalUnresponded;
            if (totalFeedback > 0)
            {
                double respondedPercentage = (double)totalResponded / totalFeedback * 100;
                double unrespondedPercentage = (double)totalUnresponded / totalFeedback * 100;

                // Update UI elements
                feedbackSummary.InnerText = $"{respondedPercentage:F2}%"; // Responded
                unrespondedSummary.InnerText = $"{unrespondedPercentage:F2}%"; // Unresponded
            }
        }
    }
}
