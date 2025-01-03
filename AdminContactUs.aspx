<%@ Page Title="Admin | Contact Responses" Language="C#" AutoEventWireup="true" CodeBehind="AdminContactUs.aspx.cs" Inherits="Assg1.AdminContactUs" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Contact Us Feedback Dashboard</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f4f4f4;
        }
        .title {
            text-align: center;
            margin-bottom: 20px;
            font-size: 24px;
            font-weight: bold;
        }
        .container {
            width: 100%;
            max-width: 800px;
            margin: auto;
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            padding: 20px;
        }
        .summary {
            text-align: center;
            margin-bottom: 20px;
        }
        .circle {
            position: relative;
            width: 120px;
            height: 120px;
            margin: 0 auto;
        }
        .feedbacks {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
        }
        .feedback-container {
            flex: 1;
            background: #f9f9f9;
            padding: 10px;
            border-radius: 5px;
            box-shadow: 0 1px 5px rgba(0,0,0,0.1);
            margin: 0 10px;
        }
        .feedback-container h2 {
            margin: 0 0 10px;
            text-decoration: underline;
        }
        .feedback-item {
            margin: 5px 0;
            cursor: pointer;
        }
        .responded {
            color: #28a745;
        }
        .unresponded {
            color: #dc3545;
        }
        button {
            background-color: #5DB996;
            color: white;
            border: none;
            border-radius: 5px;
            padding: 5px 10px;
            cursor: pointer;
            margin-left: 10px;
        }
        button:hover {
            background-color: #4A9A7E;
        }
        #feedbackDetails {
            display: none;
            background: white;
            border-radius: 8px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-top: 20px;
        }
    </style>
</head>
<body>

<div class="title">Contact Us Feedback Dashboard</div>

<div class="container">
    <div class="summary">
        <h2>Feedback Summary</h2>
        <div class="circle">
            <svg width="120" height="120">
                <circle r="54" cx="60" cy="60" fill="none" stroke="#e0e0e0" stroke-width="12" />
                <circle id="progressCircle" r="54" cx="60" cy="60" fill="none" stroke="#28a745" stroke-width="12" stroke-dasharray="339.292" stroke-dashoffset="339.292"/>
                <text id="percentageText" x="60" y="60" text-anchor="middle" alignment-baseline="middle">0%</text>
            </svg>
        </div>
        <div>
            <p class="responded" id="totalResponded" runat="server">Total Responded Feedback: 0</p>
            <p class="unresponded" id="totalUnresponded" runat="server">Total Unresponded Feedback: 0</p>
        </div>
    </div>
</div>

<div class="container">
    <div class="feedbacks">
        <div class="feedback-container">
            <h2>Responded Feedback</h2>
            <div id="respondedList" runat="server">
                <!-- Responded feedback items will be added here -->
            </div>
            <div class="feedback-item">Total Responded: <span id="respondedCount" runat="server">0</span></div>
        </div>
        <div class="feedback-container">
            <h2>Unresponded Feedback</h2>
            <div id="unrespondedList" runat="server">
                <!-- Unresponded feedback items will be added here -->
            </div>
            <div class="feedback-item">Total Unresponded: <span id="unrespondedCount" runat="server">0</span></div>
        </div>
    </div>
</div>

<div id="feedbackDetails">
    <!-- Feedback details will be displayed here -->
</div>

<script>
    let totalResponded = 0;
    const unrespondedList = document.getElementById("unrespondedList");
    let totalUnresponded = unrespondedList.querySelectorAll('div').length;

    function markAsResponded(feedbackId, button) {
        totalResponded++;
        totalUnresponded--;

        const feedbackItem = button.parentNode;
        const respondedList = document.getElementById("respondedList");
        respondedList.innerHTML += '<div class="feedback-item" onclick="showDetails(' + feedbackId + ')">ID: ' + feedbackId + ' <button onclick="undoMarkAsRespond(\'' + feedbackId + '\', this)">Undo Mark As Respond</button></div>';
        feedbackItem.remove();

        document.getElementById("respondedCount").innerText = totalResponded;
        document.getElementById("unrespondedCount").innerText = totalUnresponded;

        updateChart();
    }

    function undoMarkAsRespond(feedbackId, button) {
        totalResponded--;
        totalUnresponded++;

        const feedbackItem = button.parentNode;
        const unrespondedList = document.getElementById("unrespondedList");
        unrespondedList.innerHTML += '<div class="feedback-item" onclick="showDetails(' + feedbackId + ')">ID: ' + feedbackId + ' <button onclick="markAsResponded(\'' + feedbackId + '\', this)">Mark As Respond</button></div>';
        feedbackItem.remove();

        document.getElementById("respondedCount").innerText = totalResponded;
        document.getElementById("unrespondedCount").innerText = totalUnresponded;

        updateChart();
    }

    function showDetails(id, firstName, lastName, email, phoneNumber, message, filePath1, filePath2, filePath3, timeFeedback) {
        const detailsHtml = `
        <h3>Feedback Details</h3>
        <p><strong>ID:</strong> ${id}</p>
        <p><strong>First Name:</strong> ${firstName}</p>
        <p><strong>Last Name:</strong> ${lastName}</p>
        <p><strong>Email:</strong> <a href='mailto:${email}' style='color: blue; text-decoration: underline;'>${email}</a></p>
        <p><strong>Phone Number:</strong> <a href='tel:${phoneNumber}' style='color: blue; text-decoration: underline;'>${phoneNumber}</a></p></p>
        <p><strong>Message:</strong> ${message}</p>
        <p><strong>File Path 1:</strong> ${filePath1 ? `<a href="${filePath1}" target="_blank">View File</a>` : 'N/A'}</p>
        <p><strong>File Path 2:</strong> ${filePath2 ? `<a href="${filePath2}" target="_blank">View File</a>` : 'N/A'}</p>
        <p><strong>File Path 3:</strong> ${filePath3 ? `<a href="${filePath3}" target="_blank">View File</a>` : 'N/A'}</p>
        <p><strong>Time Feedback:</strong> ${new Date(timeFeedback).toLocaleString()}</p>
    `;

        const detailsContainer = document.getElementById("feedbackDetails");
        detailsContainer.innerHTML = detailsHtml;
        detailsContainer.style.display = 'block'; // Make sure to show the details
    }

    function updateChart() {
        const totalFeedback = totalResponded + totalUnresponded;
        const respondedPercentage = totalFeedback ? (totalResponded / totalFeedback) * 100 : 0;

        document.getElementById("percentageText").innerText = Math.round(respondedPercentage) + "%";

        const progressCircle = document.getElementById("progressCircle");
        const radius = 54;
        const circumference = 2 * Math.PI * radius;
        const offset = circumference - (respondedPercentage / 100 * circumference);

        progressCircle.style.strokeDashoffset = offset;

        document.getElementById("totalResponded").innerText = 'Total Responded Feedback: ' + totalResponded;
        document.getElementById("totalUnresponded").innerText = 'Total Unresponded Feedback: ' + totalUnresponded;
    }

    updateChart();
</script>

</body>
</html>
