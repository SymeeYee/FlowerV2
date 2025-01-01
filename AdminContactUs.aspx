using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Assg1
{
    public partial class AdminContactUs : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ContactUsDB"].ConnectionString;
        private int totalResponded = 0;
        private int totalUnresponded = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFeedback();
            }
        }

        private void LoadFeedback()
        {
            string query = "SELECT FeedbackID, Message, Responded FROM Feedback";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string feedbackText = reader.GetString(1);
                        bool isResponded = reader.GetBoolean(2);

                        if (isResponded)
                        {
                            // Create responded feedback item
                            respondedList.InnerHtml += $"<div class='feedback-item'>ID: {id} - {feedbackText} <button onclick=\"undoMarkAsRespond('{id}', this)\">Undo Mark As Respond</button></div>";
                            totalResponded++;
                        }
                        else
                        {
                            // Create unresponded feedback item
                            unrespondedList.InnerHtml += $"<div class='feedback-item'>ID: {id} - {feedbackText} <button onclick=\"markAsResponded('{id}', this)\">Mark As Respond</button></div>";
                            totalUnresponded++;
                        }
                    }
                }
            }

            // Update displayed counts
            respondedCount.InnerText = totalResponded.ToString();
            unrespondedCount.InnerText = totalUnresponded.ToString();
            UpdateChart();
        }

        // Function to mark feedback as responded
        public void MarkAsResponded(int feedbackId)
        {
            string query = "UPDATE Feedback SET Responded = 1 WHERE FeedbackID = @Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", feedbackId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Function to undo marking feedback as responded
        public void UndoMarkAsRespond(int feedbackId)
        {
            string query = "UPDATE Feedback SET Responded = 0 WHERE FeedbackID = @Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", feedbackId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Function to update the chart
        private void UpdateChart()
        {
            // Logic to perform any updates needed for the chart (if applicable)
            // This can also be triggered after each feedback update
        }
    }
}
