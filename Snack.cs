using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MixAndMeltCo {
    public partial class Snack : Form {
        Label catalogueHeader = new Label();
        Label counterLabel = new Label();
        Label productPriceDisplay = new Label();

        Label productDescription = new Label();

        string[] snacks = {
            "Cheesy Nachos",
            "Garlic Parmesan Fries",
            "Spicy Chicken Wings",
            "Classic Mozzarella Sticks",
            "Loaded Potato Skins",
            "Crunchy Onion Rings"
        };

        decimal[] prices = {
            100.00m,
            90.00m,
            120.00m,
            95.00m,
            110.00m,
            85.00m,
        };

        string[] descriptions = {
            "Crispy nachos topped with gooey cheese and served with a side of zesty salsa.",
            "Golden fries seasoned with savory garlic and Parmesan for a delightful twist.",
            "Juicy chicken wings with a fiery kick, perfect for spice lovers.",
            "Classic breaded mozzarella sticks with a molten cheesy center, served with marinara sauce.",
            "Crispy potato skins loaded with cheese, bacon bits, and sour cream.",
            "Perfectly battered onion rings with a satisfying crunch in every bite."
        };

        int amountToOrder = 1;

        PrivateFontCollection font = new PrivateFontCollection();
        string fontPath1 = Path.Combine(Application.StartupPath, "CustomFonts/RethinkSans-ExtraBold.ttf");
        string fontPath2 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Light.ttf");
        string fontPath3 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Regular.ttf");
        public Snack(int width, int height) {
            InitializeComponent();
            this.Width = width;   // Set the width to match the catalogueViewer
            this.Height = height; // Set the height to match the catalogueViewer
            this.HorizontalScroll.Maximum = 0;   // Disable horizontal scrolling
            this.HorizontalScroll.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;

            try {
                font.AddFontFile(fontPath1);
                font.AddFontFile(fontPath2);
                font.AddFontFile(fontPath3);
            }
            catch (Exception ex) {
                MessageBox.Show($"Failed to load custom fonts: {ex.Message}");
            }

            catalogueHeader = new Label() {
                AutoSize = true,
                Text = "Snack",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                Location = new Point(0, 20),
                Dock = DockStyle.Top,
                Padding = new Padding(90, 50, 0, 10)
            };
            this.Controls.Add(catalogueHeader);

            TableLayoutPanel snackGrid = new TableLayoutPanel() {
                RowCount = 3,
                ColumnCount = 3,
                Padding = new Padding(90, 0, 90, 50)
            };

            for (int i = 0; i < snackGrid.RowCount; i++) {
                snackGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            }
            for (int i = 0; i < snackGrid.ColumnCount; i++) {
                snackGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            }

            int snackIndex = 0;

            snackGrid.Size = new Size(this.Width, this.Height);
            snackGrid.Location = new Point(0, catalogueHeader.Height);
            snackGrid.HorizontalScroll.Maximum = 0;   // Disable horizontal scrolling
            snackGrid.HorizontalScroll.Visible = false; // Hide the scrollbar
            this.Controls.Add(snackGrid);

            for (int rowIndex = 0; rowIndex < snackGrid.RowCount-1; rowIndex++) {
                for (int columnIndex = 0; columnIndex < snackGrid.ColumnCount; columnIndex++) {
                    string uniqueName = $"productBox{snackIndex + 1}";
                    Panel productBox = new Panel() {
                        Name = uniqueName,
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(10),
                    };

                    Label snackLabel = new Label() {
                        Text = $"{snacks[snackIndex]}\n{prices[snackIndex]:F2}",
                        Font = new Font(font.Families[0], 11, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(0),
                        Dock = DockStyle.Bottom,
                    };
                    snackLabel.Size = new Size(productBox.Width, productBox.Height);
                    snackLabel.Location = new Point((productBox.Width / 2), productBox.Height);
                    productBox.Controls.Add(snackLabel);
                    snackGrid.Controls.Add(productBox, columnIndex, rowIndex);

                    int currentFlavorIndex = snackIndex; // Capture the index
                    productBox.Click += (sender, e) => productBox_selected(sender, e, currentFlavorIndex);

                    snackIndex++;
                }
            }

            Panel productBox1 = (Panel)snackGrid.Controls.Find("productBox1", true).FirstOrDefault();
            if (productBox1 != null) {
                productBox1.Click += (sender, e) => productBox_selected(sender, e, 0);
            }
            Panel productBox2 = (Panel)snackGrid.Controls.Find("productBox2", true).FirstOrDefault();
            if (productBox2 != null) {
                productBox2.Click += (sender, e) => productBox_selected(sender, e, 1);
            }
            Panel productBox3 = (Panel)snackGrid.Controls.Find("productBox3", true).FirstOrDefault();
            if (productBox3 != null) {
                productBox3.Click += (sender, e) => productBox_selected(sender, e, 2);
            }
            Panel productBox4 = (Panel)snackGrid.Controls.Find("productBox4", true).FirstOrDefault();
            if (productBox4 != null) {
                productBox4.Click += (sender, e) => productBox_selected(sender, e, 3);
            }
            Panel productBox5 = (Panel)snackGrid.Controls.Find("productBox5", true).FirstOrDefault();
            if (productBox5 != null) {
                productBox5.Click += (sender, e) => productBox_selected(sender, e, 4);
            }
            Panel productBox6 = (Panel)snackGrid.Controls.Find("productBox6", true).FirstOrDefault();
            if (productBox6 != null) {
                productBox6.Click += (sender, e) => productBox_selected(sender, e, 5);
            }
        }

        Panel activeProductBoxScreen = null;

        private void productBox_selected(object sender, EventArgs e, int snackIndex) {

            AutoScroll = false;
            // Remove the current active panel, if any
            if (activeProductBoxScreen != null) {
                this.Controls.Remove(activeProductBoxScreen);
                activeProductBoxScreen.Dispose();
            }

            Panel selectedProductBoxScreen = new Panel() {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            activeProductBoxScreen = selectedProductBoxScreen; // Track the active panel

            this.Controls.Remove(catalogueHeader);
            this.Controls.Add(selectedProductBoxScreen);
            selectedProductBoxScreen.BringToFront();

            Button closeSelectedProduct = new Button() {
                AutoSize = true,
                Text = "✖",
                Font = new Font("Arial", 20, FontStyle.Regular),
                BackColor = Color.Transparent,
                Location = new Point(selectedProductBoxScreen.Width - 120, 20),
                Padding = new Padding(20)
            };
            closeSelectedProduct.FlatStyle = FlatStyle.Flat;
            closeSelectedProduct.FlatAppearance.BorderSize = 0;

            closeSelectedProduct.Click += (s, args) => {
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
            Panel productDetailContainer = new Panel() {
                AutoSize = true,
            };
            productDetailContainer.Width = selectedProductBoxScreen.Width / 2;
            productDetailContainer.Location = new Point((selectedProductBoxScreen.Width - productDetailContainer.Width) / 2, ((selectedProductBoxScreen.Height - productDetailContainer.Height) / 2) - 50);

            Label productName = new Label() {
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                AutoSize = true,
                Text = $"{snacks[snackIndex]}",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            productDescription = new Label() {
                Font = new Font(font.Families[0], 12, FontStyle.Regular),
                AutoSize = true, // Disable AutoSize to control dimensions manually
                Text = $"{descriptions[snackIndex]}",
                BorderStyle = BorderStyle.None,
                Location = new Point(0, (productName.Height + 3) * 2), // Position below productName
                Size = new Size(productDetailContainer.Width, 60), // Set width to container's width, and height to fit text
                TextAlign = ContentAlignment.TopLeft, // Align text within the label
                Padding = new Padding(0, 0, 0, 10)
            };
            productDescription.MaximumSize = new Size(productDetailContainer.Width, 0);
            productDescription.AutoEllipsis = true;

            // Enable word wrapping
            productDetailContainer.Controls.Add(productName);
            productDetailContainer.Controls.Add(productDescription);

            Panel amountCounterContainer = new Panel() {
                Size = new Size(productDetailContainer.Width / 2, 50),
                Location = new Point(0, (productDescription.Location.Y + productDescription.Height) + 20),
            };

            productPriceDisplay = new Label() {
                Text = $"{prices[snackIndex]}",
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                TextAlign = ContentAlignment.TopRight,
            };
            productPriceDisplay.Size = new Size(productDetailContainer.Width / 2, amountCounterContainer.Height);
            productPriceDisplay.Location = new Point(amountCounterContainer.Width, (productDescription.Location.Y + productDescription.Height) + 20);

            Label localCounterLabel = new Label() {
                Text = amountToOrder.ToString(),
                Font = new Font(font.Families[0], 15, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(80, 42),
                BorderStyle = BorderStyle.FixedSingle,
            };
            // Attach buttons and update the correct counter
            Button decrementButton = new Button() {
                AutoSize = true,
                Text = "-",
                Font = new Font(font.Families[2], 15, FontStyle.Regular),
                BackColor = Color.Black,
                ForeColor = Color.White,
            };
            decrementButton.FlatStyle = FlatStyle.Flat;
            decrementButton.FlatAppearance.BorderColor = Color.Black;
            decrementButton.Click += (s, e) => {
                amountToOrder = Math.Max(1, amountToOrder - 1);
                localCounterLabel.Text = amountToOrder.ToString();
                productPriceDisplay.Text = $"{prices[snackIndex] * amountToOrder:F2}";
            };
            Button incrementButton = new Button() {
                AutoSize = true,
                Text = "+",
                Font = new Font(font.Families[2], 15, FontStyle.Regular),
                BackColor = Color.Black,
                ForeColor = Color.White
            };
            incrementButton.FlatStyle = FlatStyle.Flat;
            incrementButton.FlatAppearance.BorderColor = Color.Black;
            incrementButton.Click += (s, e) => {
                amountToOrder++;
                localCounterLabel.Text = amountToOrder.ToString();
                productPriceDisplay.Text = $"{prices[snackIndex] * amountToOrder:F2}";
            };


            decrementButton.Location = new Point(0, (amountCounterContainer.Height - incrementButton.Height) / 4);
            localCounterLabel.Location = new Point(decrementButton.Location.X + decrementButton.Width, (amountCounterContainer.Height - incrementButton.Height) / 4);
            incrementButton.Location = new Point(localCounterLabel.Location.X + localCounterLabel.Width, (amountCounterContainer.Height - incrementButton.Height) / 4);

            amountCounterContainer.Controls.Add(localCounterLabel);
            amountCounterContainer.Controls.Add(decrementButton);
            amountCounterContainer.Controls.Add(incrementButton);

            Button addToOrder = new Button() {
                Size = new Size(productDetailContainer.Width, 80),
                Font = new Font(font.Families[2], 20, FontStyle.Regular),
                Location = new Point(0, (amountCounterContainer.Location.Y + amountCounterContainer.Height) + 20),
                BackColor = Color.Black,
                ForeColor = Color.White,
            };
            addToOrder.FlatStyle = FlatStyle.Flat;
            addToOrder.FlatAppearance.BorderColor = Color.Black;
            addToOrder.Text = "Add To Order";

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
