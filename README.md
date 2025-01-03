<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="AsgV3.WebForm1" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Flower Shop</title>
    <script>
        function updateSubtotal() {
            var subtotal = 0;

            // Get all cart items
            var cartItems = document.querySelectorAll('.cart-item');

            cartItems.forEach(function (item) {
                var quantity = item.querySelector('.quantity').value;
                var price = item.querySelector('.price-value').innerText;

                // Calculate the total price for this item
                var itemTotal = quantity * price;

                // Add to the subtotal
                subtotal += itemTotal;
                item.querySelector('.total-price').innerText = itemTotal.toFixed(2); // Update total price for each item
            });

            // Update the subtotal element
            document.getElementById('subtotal').innerText = subtotal.toFixed(2);
        }

        // Attach event listeners to quantity inputs
        document.addEventListener('DOMContentLoaded', function () {
            var quantityInputs = document.querySelectorAll('.quantity');
            quantityInputs.forEach(function (input) {
                input.addEventListener('input', updateSubtotal);
            });
        });
    </script>

    <style>
        body {
            font-family: sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }

        /* Container layout */
        .container {
            display: flex;
            justify-content: space-between; /* Ensures sections are aligned left and right */
            align-items: flex-start; /* Aligns sections at the top */
            gap: 20px;
            margin: 20px auto;
            max-width: 1200px; /* Center container with a maximum width */
        }

        /* Shopping Cart Section */
        .cart-section {
            flex: 2; /* Takes 2/3 of the space */
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
        }

        /* Delivery Information Section */
        .delivery-section {
            flex: 1; /* Takes 1/3 of the space */
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
        }

        /* Product and Summary Styles */
        .cart-item {
            margin-bottom: 20px;
        }

        .product-name {
            font-weight: bold;
        }

        .tag {
            color: #888;
            font-size: 14px;
            margin-left: 10px;
        }

        .item-details {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .price {
            font-weight: bold;
            font-size: 18px;
        }

        .quantity {
            width: 50px;
            padding: 5px;
        }

        .summary {
            margin-top: 20px;
            font-weight: bold;
            font-size: 18px;
        }

        /* General Styling for Delivery Section */
        label {
            display: block;
            margin-top: 10px;
            font-weight: bold;
        }

        select, input[type="checkbox"], textarea, button {
            width: 100%;
            margin-top: 5px;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }

        textarea {
            height: 80px;
        }

        button {
            background-color: #55BC88;
            color: white;
            border: none;
            margin-top: 20px;
            cursor: pointer;
            padding: 10px 20px;
            border-radius: 5px;
            text-align: center;
        }

        button:hover {
            background-color: #007BFF;
        }

        /* Additional Table Styles */
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        th, td {
            text-align: left;
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        th {
            background-color: #f2f2f2;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Shopping Cart Section -->
            <div class="cart-section">
                <h1>Shopping Cart</h1>

                <table>
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Unit Price</th>
                            <th>Quantity</th>
                            <th>Total Price</th>
                        </tr>
                    </thead>
                    <tbody class="cart-items">
                        <!-- Product 1 -->
                        <tr class="cart-item">
                            <td>
                                Black Forest <span class="tag">Klang Valley Only</span>
                            </td>
                            <td class="price">RM <span class="price-value">189</span></td>
                            <td>
                                <input type="number" class="quantity" value="1" min="1" data-price="189" oninput="updateSubtotal()">
                            </td>
                            <td>RM <span class="total-price">189</span></td>
                        </tr>

                        <!-- Product 2 -->
                        <tr class="cart-item">
                            <td>
                                Peach Perfect <span class="tag">Klang Valley Only</span>
                            </td>
                            <td class="price">RM <span class="price-value">139</span></td>
                            <td>
                                <input type="number" class="quantity" value="1" min="1" data-price="139" oninput="updateSubtotal()">
                            </td>
                            <td>RM <span class="total-price">139</span></td>
                        </tr>
                    </tbody>
                </table>

                <div class="summary">
                    <p>Subtotal: RM <span id="subtotal">328</span></p>
                </div>
            </div>

            <!-- Delivery Information Section -->
            <div class="delivery-section">
                <h3>&nbsp;</h3>
                <h3>Delivery Information</h3>

                <!-- Delivery Destination -->
                <label for="destination">Select a Delivery Destination:</label>
                <select id="destination">
                    <option value="">Please Pick A Destination</option>
                    <option value="Kuala Lumpur">Kuala Lumpur</option>
                    <option value="Selangor">Selangor</option>
                    <option value="Penang">Penang</option>
                    <option value="Johor">Johor</option>
                    <option value="Kedah">Kedah</option>
                    <option value="Kelantan">Kelantan</option>
                    <option value="Melaka">Melaka</option>
                    <option value="Negeri Sembilan">Negeri Sembilan</option>
                    <option value="Pahang">Pahang</option>
                    <option value="Perak">Perak</option>
                    <option value="Perlis">Perlis</option>
                    <option value="Terengganu">Terengganu</option>
                </select>

                <!-- Personal Message -->
                <label for="message">Choose or type your personal card message:</label>
                <asp:TextBox ID="TextBox1" runat="server" Height="82px" Width="441px" placeholder="Tell the recipient who you are or just leave blank" CssClass="textbox-sender"></asp:TextBox>
                <br />
                <br />

                <label for="sender-name">Type sender's name:</label>
                <asp:TextBox ID="TxtboxSender" runat="server" Height="35px" Width="100%" placeholder="Tell the recipient who you are or just leave blank" CssClass="textbox-sender"></asp:TextBox>
                <br />
                <br />

                <button id="continue">Continue To Delivery Address</button>
            </div>
        </div>
    </form>
</body>
</html>
