using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MixAndMeltCo
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class DatabaseConnection
    {
        private string connectionString = "Server=localhost;Port=3306;Database=mixandmelt;Uid=root;Pwd=DBpass12345$;";

        public void AddOrderToDatabase(string customerName, List<OrderItem> orderItems)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Insert order
                string orderQuery = "INSERT INTO Orders (CustomerName, OrderDate, TotalAmount, OrderStatus) VALUES (@CustomerName, @OrderDate, @TotalAmount, @OrderStatus)";
                MySqlCommand command = new MySqlCommand(orderQuery, connection);
                command.Parameters.AddWithValue("@CustomerName", customerName);
                command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                command.Parameters.AddWithValue("@TotalAmount", orderItems.Sum(item => item.TotalPrice)); // Calculate total amount
                command.Parameters.AddWithValue("@OrderStatus", "Pending");

                command.ExecuteNonQuery();
                int orderId = (int)command.LastInsertedId;

                // Insert order items
                foreach (var orderItem in orderItems)
                {
                    string orderItemQuery = "INSERT INTO OrderItems (OrderId, MenuItemId, Quantity, Price, TotalPrice) VALUES (@OrderId, @MenuItemId, @Quantity, @Price, @TotalPrice)";
                    MySqlCommand orderItemCommand = new MySqlCommand(orderItemQuery, connection);
                    orderItemCommand.Parameters.AddWithValue("@OrderId", orderId);
                    orderItemCommand.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItemId);
                    orderItemCommand.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                    orderItemCommand.Parameters.AddWithValue("@Price", orderItem.Price);
                    orderItemCommand.Parameters.AddWithValue("@TotalPrice", orderItem.TotalPrice);

                    orderItemCommand.ExecuteNonQuery();
                }
            }
        }
    }

}