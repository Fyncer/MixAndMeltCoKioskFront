using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MixAndMeltCo
{
    public partial class IceCream : Form
    {
        Label catalogueHeader = new Label();
        Label counterLabel = new Label();
        Label productPriceDisplay = new Label();
        Label productDescription = new Label();

        // Arrays removed, these will be populated from the database
        List<string> iceCreams = new List<string>();
        List<decimal> prices = new List<decimal>();
        List<string> descriptions = new List<string>();

        int amountToOrder = 1;

        PrivateFontCollection font = new PrivateFontCollection();
        string fontPath1 = Path.Combine(Application.StartupPath, "CustomFonts/RethinkSans-ExtraBold.ttf");
        string fontPath2 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Light.ttf");
        string fontPath3 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Regular.ttf");

        // Database connection string
        string connectionString = "Server=localhost;Port=3306;Database=mixandmelt;Uid=root;Pwd=DBpass12345$;";

        public IceCream(int width, int height)
        {
            InitializeComponent();
            this.Width = width;
            this.Height = height;
            this.HorizontalScroll.Maximum = 0;   // Disable horizontal scrolling
            this.HorizontalScroll.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;

            try
            {
                font.AddFontFile(fontPath1);
                font.AddFontFile(fontPath2);
                font.AddFontFile(fontPath3);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load custom fonts: {ex.Message}");
            }

            catalogueHeader = new Label()
            {
                AutoSize = true,
                Text = "Ice Cream",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                Location = new Point(0, 20),
                Dock = DockStyle.Top,
                Padding = new Padding(90, 50, 0, 10)
            };
            this.Controls.Add(catalogueHeader);

            // Fetch ice cream data from the database
            FetchIceCreamData();

            // Set up the grid with the fetched data
            SetUpIceCreamGrid();
        }


        // Method to fetch ice cream data from the database
        List<string> imagePaths = new List<string>();
        List<int> menuItemIds = new List<int>(); // Add this list to store the MenuItemIds

        private void FetchIceCreamData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MenuItemId, ItemName, Price, Description, ImagePath FROM MenuItems WHERE CategoryId = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Ice Cream')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        menuItemIds.Add(reader.GetInt32("MenuItemId")); // Store the MenuItemId
                        iceCreams.Add(reader.GetString("ItemName"));
                        prices.Add(reader.GetDecimal("Price"));
                        descriptions.Add(reader.GetString("Description"));
                        imagePaths.Add(reader.GetString("ImagePath"));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data from database: {ex.Message}");
                }
            }
        }

        // Set up the ice cream grid using data from the database
        private void SetUpIceCreamGrid()
        {
            int iceCreamIndex = 0;

            TableLayoutPanel iceCreamGrid = new TableLayoutPanel()
            {
                RowCount = 3,
                ColumnCount = 3,
                Padding = new Padding(90, 0, 90, 50)
            };

            for (int i = 0; i < iceCreamGrid.RowCount; i++)
            {
                iceCreamGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            }
            for (int i = 0; i < iceCreamGrid.ColumnCount; i++)
            {
                iceCreamGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            }

            iceCreamGrid.Size = new Size(this.Width, this.Height);
            iceCreamGrid.Location = new Point(0, catalogueHeader.Height);
            iceCreamGrid.HorizontalScroll.Maximum = 0;
            iceCreamGrid.HorizontalScroll.Visible = false;
            this.Controls.Add(iceCreamGrid);

            // Use the ice cream data loaded from the database
            for (int rowIndex = 0; rowIndex < iceCreamGrid.RowCount - 1; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < iceCreamGrid.ColumnCount; columnIndex++)
                {
                    string uniqueName = $"productBox{iceCreamIndex + 1}";
                    Panel productBox = new Panel()
                    {
                        Name = uniqueName,
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(10),
                    };

                    PictureBox productImage = new PictureBox()
                    {
                        ImageLocation = imagePaths[iceCreamIndex], // Use the image path from the database
                        SizeMode = PictureBoxSizeMode.StretchImage, // Stretch the image to fill the panel
                        Dock = DockStyle.Top,
                        Height = 100, // Adjust height of the image
                    };
                    productBox.Controls.Add(productImage);


                    Label iceCreamLabel = new Label()
                    {
                        Text = $"{iceCreams[iceCreamIndex]}\n{prices[iceCreamIndex]:F2}",
                        Font = new Font(font.Families[0], 11, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(0),
                        Dock = DockStyle.Bottom,
                    };
                    iceCreamLabel.Size = new Size(productBox.Width, productBox.Height);
                    iceCreamLabel.Location = new Point((productBox.Width / 2), productBox.Height);
                    productBox.Controls.Add(iceCreamLabel);
                    iceCreamGrid.Controls.Add(productBox, columnIndex, rowIndex);

                    int currentIceCreamIndex = iceCreamIndex; // Capture the index
                    productBox.Click += (sender, e) => productBox_selected(sender, e, currentIceCreamIndex);

                    iceCreamIndex++;
                }
            }
        }

        Panel activeProductBoxScreen = null;

        private void productBox_selected(object sender, EventArgs e, int iceCreamIndex)
        {
            AutoScroll = false;
            // Remove the current active panel, if any
            if (activeProductBoxScreen != null)
            {
                this.Controls.Remove(activeProductBoxScreen);
                activeProductBoxScreen.Dispose();
            }

            Panel selectedProductBoxScreen = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            activeProductBoxScreen = selectedProductBoxScreen; // Track the active panel

            this.Controls.Remove(catalogueHeader);
            this.Controls.Add(selectedProductBoxScreen);
            selectedProductBoxScreen.BringToFront();

            Button closeSelectedProduct = new Button()
            {
                AutoSize = true,
                Text = "✖",
                Font = new Font("Arial", 20, FontStyle.Regular),
                BackColor = Color.Transparent,
                Location = new Point(selectedProductBoxScreen.Width - 120, 20),
                Padding = new Padding(20)
            };
            closeSelectedProduct.FlatStyle = FlatStyle.Flat;
            closeSelectedProduct.FlatAppearance.BorderSize = 0;

            closeSelectedProduct.Click += (s, args) =>
            {
                amountToOrder = 1;
                this.Controls.Remove(selectedProductBoxScreen);
                selectedProductBoxScreen.Dispose();
                activeProductBoxScreen = null; // Clear the reference
                this.Controls.Add(catalogueHeader);
                catalogueHeader.BringToFront();
                this.Refresh();
            };

            selectedProductBoxScreen.Controls.Add(closeSelectedProduct);

            // Add product details
            Panel productDetailContainer = new Panel()
            {
                AutoSize = true,
            };
            productDetailContainer.Width = selectedProductBoxScreen.Width / 2;
            productDetailContainer.Location = new Point((selectedProductBoxScreen.Width - productDetailContainer.Width) / 2, ((selectedProductBoxScreen.Height - productDetailContainer.Height) / 2) - 50);

            Label productName = new Label()
            {
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                AutoSize = true,
                Text = $"{iceCreams[iceCreamIndex]}",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            productDescription = new Label()
            {
                Font = new Font(font.Families[0], 12, FontStyle.Regular),
                AutoSize = true, // Disable AutoSize to control dimensions manually
                Text = $"{descriptions[iceCreamIndex]}",
                BorderStyle = BorderStyle.None,
                Location = new Point(0, (productName.Height + 3) * 2), // Position below productName
                Size = new Size(productDetailContainer.Width, 60), // Set width to container's width, and height to fit text
                TextAlign = ContentAlignment.TopLeft, // Align text within the label
                Padding = new Padding(0, 0, 0, 10)
            };
            productDescription.MaximumSize = new Size(productDetailContainer.Width, 0);
            productDescription.AutoEllipsis = true;

            // Add the product name and description labels
            productDetailContainer.Controls.Add(productName);
            productDetailContainer.Controls.Add(productDescription);

            // Add the image to the selected product screen
            PictureBox selectedProductImage = new PictureBox()
            {
                ImageLocation = imagePaths[iceCreamIndex],  // Set the image based on the selected index
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Top,
                Height = 200, // Adjust height for the image
            };
            selectedProductBoxScreen.Controls.Add(selectedProductImage); // Add to the selected product box


            Panel amountCounterContainer = new Panel()
            {
                Size = new Size(productDetailContainer.Width / 2, 50),
                Location = new Point(0, (productDescription.Location.Y + productDescription.Height) + 20),
            };

            productPriceDisplay = new Label()
            {
                Text = $"{prices[iceCreamIndex]}",
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                TextAlign = ContentAlignment.TopRight,
            };
            productPriceDisplay.Size = new Size(productDetailContainer.Width / 2, amountCounterContainer.Height);
            productPriceDisplay.Location = new Point(amountCounterContainer.Width, (productDescription.Location.Y + productDescription.Height) + 20);

            Label localCounterLabel = new Label()
            {
                Text = amountToOrder.ToString(),
                Font = new Font(font.Families[0], 15, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(80, 42),
                BorderStyle = BorderStyle.FixedSingle,
            };

            // Attach buttons and update the correct counter
            Button decrementButton = new Button()
            {
                AutoSize = true,
                Text = "-",
                Font = new Font(font.Families[2], 15, FontStyle.Regular),
                BackColor = Color.Black,
                ForeColor = Color.White,
            };
            decrementButton.FlatStyle = FlatStyle.Flat;
            decrementButton.FlatAppearance.BorderColor = Color.Black;
            decrementButton.Click += (s, e) =>
            {
                amountToOrder = Math.Max(1, amountToOrder - 1);
                localCounterLabel.Text = amountToOrder.ToString();
                productPriceDisplay.Text = $"{prices[iceCreamIndex] * amountToOrder:F2}";
            };
            Button incrementButton = new Button()
            {
                AutoSize = true,
                Text = "+",
                Font = new Font(font.Families[2], 15, FontStyle.Regular),
                BackColor = Color.Black,
                ForeColor = Color.White
            };
            incrementButton.FlatStyle = FlatStyle.Flat;
            incrementButton.FlatAppearance.BorderColor = Color.Black;
            incrementButton.Click += (s, e) =>
            {
                amountToOrder++;
                localCounterLabel.Text = amountToOrder.ToString();
                productPriceDisplay.Text = $"{prices[iceCreamIndex] * amountToOrder:F2}";
            };

            decrementButton.Location = new Point(0, (amountCounterContainer.Height - incrementButton.Height) / 4);
            localCounterLabel.Location = new Point(decrementButton.Location.X + decrementButton.Width, (amountCounterContainer.Height - incrementButton.Height) / 4);
            incrementButton.Location = new Point(localCounterLabel.Location.X + localCounterLabel.Width, (amountCounterContainer.Height - incrementButton.Height) / 4);

            amountCounterContainer.Controls.Add(localCounterLabel);
            amountCounterContainer.Controls.Add(decrementButton);
            amountCounterContainer.Controls.Add(incrementButton);

            Button addToOrder = new Button()
            {
                Size = new Size(productDetailContainer.Width, 80),
                Font = new Font(font.Families[2], 20, FontStyle.Regular),
                Location = new Point(0, (amountCounterContainer.Location.Y + amountCounterContainer.Height) + 20),
                BackColor = Color.Black,
                ForeColor = Color.White,
            };
            addToOrder.FlatStyle = FlatStyle.Flat;
            addToOrder.FlatAppearance.BorderColor = Color.Black;
            addToOrder.Text = "Add To Order";

            addToOrder.Click += (s, e) =>
            {
                // Get the selected product details
                string customerName = "Test Customer"; // You might want to use a dynamic name, e.g., from a text box.
                int selectedMenuItemId = menuItemIds[iceCreamIndex]; // Assuming you have an array of ice creams and their IDs.
                decimal selectedPrice = prices[iceCreamIndex];
                int quantity = amountToOrder;

                // Create the order item object
                List<OrderItem> orderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        MenuItemId = selectedMenuItemId,
                        Quantity = quantity,
                        Price = selectedPrice,
                        TotalPrice = selectedPrice * quantity
                    }
                };

                // Use DatabaseConnection to add the order
                DatabaseConnection dbConnection = new DatabaseConnection();
                dbConnection.AddOrderToDatabase(customerName, orderItems);

                // Optionally, update UI to confirm order has been added
                MessageBox.Show("Order added successfully!", "Order Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            };

            productDetailContainer.Controls.Add(addToOrder);

            productDetailContainer.Controls.Add(amountCounterContainer);
            productDetailContainer.Controls.Add(productPriceDisplay);

            // Add all components to the product detail container
            productDetailContainer.Controls.Add(productName);
            productDetailContainer.Controls.Add(productDescription);

            selectedProductBoxScreen.Controls.Add(productDetailContainer);
        }
    }
}