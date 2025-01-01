<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Us Feedback Dashboard</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
        }
        .container {
            display: flex;
            justify-content: space-between;
            padding: 20px;
        }
        .card {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            padding: 20px;
            width: 48%;
        }
        .summary {
            text-align: center;
        }
        .summary h2 {
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card">
                <div class="summary">
                    <h2>Feedback Summary</h2>
                    <div>
                        <strong id="feedbackSummary" runat="server">100.00%</strong><br />
                        Responded Feedback
                    </div>
                </div>
                <table>
                    <thead>
                        <tr>
                            <th>Feedback ID</th>
                        </tr>
                    </thead>
                    <tbody id="feedbackTable" runat="server">
                        <!-- Rows will be populated here -->
                    </tbody>
                </table>
                <div id="totalRespondedLabel" runat="server">Total Responded: 0</div>
            </div>
            <div class="card">
                <div class="summary">
                    <h2>Unresponded Feedback</h2>
                    <strong id="unrespondedSummary" runat="server">0.00%</strong><br />
                    Unresponded Feedback
                </div>
            </div>
        </div>
    </form>
</body>
</html>
