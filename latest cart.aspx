<%@ Page Title="Bloom & Grace | Cart" Language="C#" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="Assg1.Cart" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Flower Shop</title>
    <link href="~/css/StyleSheet2.css" rel="stylesheet" />
    <script>
        // Set the increment value (default to 1, can be changed to 2 or more)
        const incrementValue = 1;

        function updateCart() {
            let totalItems = 0;
            let totalAmountToPay = 0;

            // Get all cart items
            const cartItems = document.querySelectorAll('.cart-item');

            cartItems.forEach(item => {
                const quantityInput = item.querySelector('.quantity');
                const quantity = parseInt(quantityInput.value) || 0;
                const price = parseFloat(item.querySelector('.price-value').innerText) || 0;
                const itemTotal = quantity * price;

                // Update total price for this item
                item.querySelector('.total-price').innerText = itemTotal.toFixed(2);

                // Update total items and total amount to pay if the item is selected
                if (item.querySelector('.select-item').checked) {
                    totalAmountToPay += itemTotal;
                    totalItems += quantity;
                }
            });

            // Update item count
            document.getElementById('item-count').innerText = totalItems;

            // Calculate total amount to pay (assuming no additional fees)
            document.getElementById('total-amount').innerText = totalAmountToPay.toFixed(2);
        }

        function removeItem(item) {
            item.remove(); // Removes the item from the cart
            updateCart(); // Update cart after removal
        }

        function increaseQuantity(button) {
            const quantityInput = button.previousElementSibling;
            let quantity = parseInt(quantityInput.value) || 0;
            quantity += incrementValue; // Increase by the increment value (default is 2)
            quantityInput.value = quantity;
            updateCart();
        }

        function decreaseQuantity(button) {
            const quantityInput = button.nextElementSibling;
            let quantity = parseInt(quantityInput.value) || 0;
            if (quantity > 1) { // Ensure the quantity doesn't go below 1
                quantity -= incrementValue; // Decrease by the increment value (default is 2)
                quantityInput.value = quantity;
                updateCart();
            }
        }

        // Function to validate the delivery date
        document.addEventListener('DOMContentLoaded', function () {
            const today = new Date();
            const dd = String(today.getDate()).padStart(2, '0');
            const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
            const yyyy = today.getFullYear();

            // Format the date as YYYY-MM-DD
            const minDate = yyyy + '-' + mm + '-' + dd;

            // Select the input element by its ID
            const dateInput = document.getElementById('delivery-date');
            if (dateInput) {
                // Set the 'min' attribute to the current date
                dateInput.setAttribute('min', minDate);
            } else {
                console.error('Input element not found');
            }
        });


    </script>



    <style>
        body {
            font-family: sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }

        .container {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 20px;
            margin: 20px auto;
            max-width: 1200px;
        }

        .cart-section {
            flex: 2;
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
        }

        .delivery-section {
            flex: 1;
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
        }

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

        .price {
            font-weight: bold;
            font-size: 18px;
        }

        .quantity-roller {
            display: flex;
            align-items: center;
            border: 1px solid #ccc;
            border-radius: 5px;
            background-color: #fff;
            padding: 5px;
        }

        .adjust-button {
            background-color: transparent;
            border: none;
            color: #007BFF;
            font-size: 20px;
            cursor: pointer;
            padding: 0 10px;
        }

        .adjust-button:hover {
            color: #0056b3;
        }

        .quantity {
            width: 40px;
            text-align: center;
            border: none;
            outline: none;
            font-size: 16px;
            color: #333;
        }

        .summary {
            margin-top: 20px;
            font-weight: bold;
            font-size: 18px;
        }

        .delivery-summary {
            margin-top: 40px;
            font-weight: bold;
            font-size: 18px;
            text-align: right;
        }

        label {
            display: block;
            margin-top: 10px;
            font-weight: bold;
        }

        select, input[type="checkbox"], textarea, button, input[type="text"], input[type="date"] {
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
            border-bottom: 2px solid #55BC88;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="cart-section">
                <h1>Shopping Cart</h1>
                
                <table>
                    <thead>
                        <tr>
                            <th>Select</th>
                            <th>Product</th>
                            <th>Unit Price</th>
                            <th>Quantity</th>
                            <th>Total Price</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    
                    <tbody class="cart-items">
                        <asp:Repeater ID="CartRepeater" runat="server">
                            <ItemTemplate>
                                <tr class="cart-item">
                                    <td><input type="checkbox" class="select-item" onchange="updateCart()"></td>
                                    <td class="product-name">
                                        <img src="<%# Eval("ImageURL") %>" alt="<%# Eval("ProductName") %>" style="width: 100px; height: auto; display: block; margin-bottom: 10px;">
                                    </td>
                                    <td class="price">RM <span class="price-value"><%# Eval("DiscountPrice", "{0:0.00}") %></span></td>
                                    <td>
                                        <div class="quantity-roller">
                                            <button type="button" class="adjust-button" onclick="decreaseQuantity(this)">–</button>
                                            <input type="number" class="quantity" value="<%# Eval("Quantity") %>" min="1" oninput="updateCart()" />
                                            <button type="button" class="adjust-button" onclick="increaseQuantity(this)">+</button>
                                        </div>
                                    </td>

                                    <td>RM <span class="total-price"><%# Eval("DiscountPrice", "{0:0.00}") %></span></td>
                                    <td>
                                        <button type="button" onclick="removeItem(this.closest('.cart-item'))">Remove</button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

            <div class="delivery-section">
                <h3>Delivery Information</h3>

                <asp:Label ID="lblMessage" runat="server" Text="Select a Delivery Destination:"></asp:Label>
                <asp:DropDownList ID="ddlDestination" runat="server" CssClass="destination-dropdown">
                    <asp:ListItem Text="Please Pick A Destination" Value="" />
                    <asp:ListItem Text="Kuala Lumpur" Value="Kuala Lumpur" />
                    <asp:ListItem Text="Selangor" Value="Selangor" />
                    <asp:ListItem Text="Penang" Value="Penang" />
                    <asp:ListItem Text="Johor" Value="Johor" />
                    <asp:ListItem Text="Kedah" Value="Kedah" />
                    <asp:ListItem Text="Kelantan" Value="Kelantan" />
                    <asp:ListItem Text="Melaka" Value="Melaka" />
                    <asp:ListItem Text="Negeri Sembilan" Value="Negeri Sembilan" />
                    <asp:ListItem Text="Pahang" Value="Pahang" />
                    <asp:ListItem Text="Perak" Value="Perak" />
                    <asp:ListItem Text="Perlis" Value="Perlis" />
                    <asp:ListItem Text="Terengganu" Value="Terengganu" />
                </asp:DropDownList>

                <asp:Label ID="lblDeliveryAddress" runat="server" Text="Delivery Address:"></asp:Label>
                <asp:TextBox ID="txtDeliveryAddress" runat="server" TextMode="MultiLine" Rows="4" CssClass="textarea-delivery" placeholder="Enter delivery address here"></asp:TextBox>

                <label for="delivery-date">Select Delivery Date:</label>
                <input type="date" id="delivery-date" />

                <label for="time-slot">Select Time Slot:</label>
                <select id="time-slot">
                    <option value="8am-12pm">8:00 AM - 12:00 PM</option>
                    <option value="1pm-4pm">1:00 PM - 4:00 PM</option>
                    <option value="4pm-8pm">4:00 PM - 8:00 PM</option>
                </select>

                <asp:Label ID="LblPM" runat="server" Text="Type Your Personal Message:"></asp:Label>
                <asp:TextBox ID="TxtboxDA" runat="server" TextMode="MultiLine" Rows="4" CssClass="textarea-delivery textbox-sender" placeholder="Type your personal card message"></asp:TextBox>

                <asp:Label ID="LblSenderName" runat="server" Text="Type sender's name:"></asp:Label>
                <asp:TextBox ID="TxtboxSender" runat="server" placeholder="Enter your name here"></asp:TextBox>

                <div class="delivery-summary">
                    <p>Total Items: <span id="item-count">3</span></p>
                    <p>Total Amount to Pay: RM <span id="total-amount">337.00</span></p>
                </div>
                <button id="continue">Checkout</button>
            </div>
        </div>
    </form>
</body>
</html>
