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
    public class SnackOrderItem {
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public SnackOrderItem(decimal price, int amount)
        {
            Price = price;
            Amount = amount;
        }
    }   

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
        public decimal totalPrice = 0;
        public int yOffset = 0 ; 

        PrivateFontCollection font = new PrivateFontCollection();
        string fontPath1 = Path.Combine(Application.StartupPath, "CustomFonts/RethinkSans-ExtraBold.ttf");
        string fontPath2 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Light.ttf");
        string fontPath3 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Regular.ttf");

        Panel selectedProductBoxScreen = new Panel();
        public Snack(int width, int height) {
            InitializeComponent();
            this.Width = width;   
            this.Height = height;
            this.HorizontalScroll.Maximum = 0;
            this.HorizontalScroll.Visible = false;
            this.AutoScroll = true;
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
                Text = "Ice Cream",
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

            int flavorIndex = 0;
            yOffset = 0 ; 

            displayLists();

            snackGrid.Size = new Size(this.Width, this.Height  + 350);
            snackGrid.Location = new Point(0, catalogueHeader.Height);
            snackGrid.HorizontalScroll.Maximum = 0; 
            snackGrid.HorizontalScroll.Visible = false;
            this.Controls.Add(snackGrid);

            for (int rowIndex = 0; rowIndex < snackGrid.RowCount-1; rowIndex++) {
                for (int columnIndex = 0; columnIndex < snackGrid.ColumnCount; columnIndex++) {
                    string uniqueName = $"productBox{flavorIndex + 1}";
                    Panel productBox = new Panel() {
                        Name = uniqueName,
                        Dock = DockStyle.Fill,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(10),
                    };

                    Label flavorLabel = new Label() {
                        Text = $"{snacks[flavorIndex]}\n{prices[flavorIndex]:F2}",
                        Font = new Font(font.Families[0], 11, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Margin = new Padding(0),
                        Dock = DockStyle.Bottom,
                    };
                    flavorLabel.Size = new Size(productBox.Width, productBox.Height);
                    flavorLabel.Location = new Point((productBox.Width / 2), productBox.Height);
                    productBox.Controls.Add(flavorLabel);
                    snackGrid.Controls.Add(productBox, columnIndex, rowIndex);

                    int currentFlavorIndex = flavorIndex; // Capture the index
                    productBox.Click += (sender, e) => productBox_selected(sender, e, currentFlavorIndex);

                    flavorIndex++;
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

        private void productBox_selected(object sender, EventArgs e, int flavorIndex) {

            AutoScroll = false;
            if (activeProductBoxScreen != null) {
                this.Controls.Remove(activeProductBoxScreen);
                activeProductBoxScreen.Dispose();
            }

            selectedProductBoxScreen = new Panel() {
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            activeProductBoxScreen = selectedProductBoxScreen;

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
                activeProductBoxScreen = null;
                this.Controls.Add(catalogueHeader);
                catalogueHeader.BringToFront();
                AutoScroll = true;
                this.Refresh();
            };

            selectedProductBoxScreen.Controls.Add(closeSelectedProduct);

            Panel productDetailContainer = new Panel() {
                AutoSize = true,
            };
            productDetailContainer.Width = selectedProductBoxScreen.Width / 2;
            productDetailContainer.Location = new Point((selectedProductBoxScreen.Width - productDetailContainer.Width) / 2, ((selectedProductBoxScreen.Height - productDetailContainer.Height) / 2)-50);

            Label productName = new Label() {
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                AutoSize = true,
                Text = $"{snacks[flavorIndex]}",
                TextAlign = ContentAlignment.MiddleLeft,
            };
            productDescription = new Label() {
                Font = new Font(font.Families[0], 12, FontStyle.Regular),
                AutoSize = true,
                Text = $"{descriptions[flavorIndex]}",
                BorderStyle = BorderStyle.None,
                Location = new Point(0, (productName.Height + 3) * 2),
                Size = new Size(productDetailContainer.Width, 60),
                TextAlign = ContentAlignment.TopLeft,
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

            productPriceDisplay = new Label(){
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
                productPriceDisplay.Text = $"{prices[flavorIndex] * amountToOrder:F2}";
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
                productPriceDisplay.Text = $"{prices[flavorIndex] * amountToOrder:F2}";
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

            addToOrder.Click += (s, args) => {
                if (Form1.orderList.ContainsKey(snacks[flavorIndex]))
                {
                    Form1.orderList[snacks[flavorIndex]] = (prices[flavorIndex], amountToOrder);
                }
                else
                {
                    Form1.orderList.Add(snacks[flavorIndex], (prices[flavorIndex], amountToOrder));
                }

                Form1.orderListDisplay.Controls.Clear();
                orderUpdateIndicate();
                displayLists();
            };

            if (Form1.orderList.ContainsKey(snacks[flavorIndex])){
                productPriceDisplay.Text = $"{(Form1.orderList[snacks[flavorIndex]].Price)*(Form1.orderList[snacks[flavorIndex]].AmountToOrder)}";
                amountToOrder = Form1.orderList[snacks[flavorIndex]].AmountToOrder;
                localCounterLabel.Text = Convert.ToString(amountToOrder);
            }
            else
            {
                productPriceDisplay.Text = $"{prices[flavorIndex]}";
            }

            addToOrder.FlatStyle = FlatStyle.Flat;
            addToOrder.FlatAppearance.BorderColor = Color.Black;
            addToOrder.Text = "Add To Order";

            productDetailContainer.Controls.Add(addToOrder);

            productDetailContainer.Controls.Add(amountCounterContainer);
            productDetailContainer.Controls.Add(productPriceDisplay);

            productDetailContainer.Controls.Add(productName);
            productDetailContainer.Controls.Add(productDescription);

            selectedProductBoxScreen.Controls.Add(productDetailContainer);
        }

                private void displayLists() {
            yOffset = 0;
            Form1.totalPrice = 0; // Reset total price to 0 before recalculating

            foreach (var entry in Form1.orderList) {
                decimal itemTotalPrice = entry.Value.Price * entry.Value.AmountToOrder;
                Form1.totalPrice += itemTotalPrice; // Accumulate total price

                Panel selectedProductFrame = new Panel() {
                    Size = new Size(Form1.orderListDisplay.Width - 80, 110),
                    Padding = new Padding(5),
                };
                Form1.AddCustomBorderToPanel(selectedProductFrame, 2, 2, 2, 2, Color.Black);
                selectedProductFrame.Location = new Point((Form1.orderListDisplay.Width - selectedProductFrame.Width) / 2, yOffset);

                Label selectedProductName = new Label() {  
                    TextAlign = ContentAlignment.MiddleLeft,
                    Height = 40,
                    Text = $"{entry.Key}",
                    Font = new Font(font.Families[2], 20, FontStyle.Regular),
                };

                Label selectedProductPrice = new Label() {
                    Height = 40,
                    Width = 200,
                    TextAlign = ContentAlignment.MiddleRight,
                    Text = $"{itemTotalPrice:F2}", // Display as currency
                    Font = new Font(font.Families[2], 20, FontStyle.Regular),
                };

                Label selectedProductQuantity = new Label() {
                    Height = 40,
                    Width = 150,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Text = $"Quantity: {entry.Value.AmountToOrder}",
                    Font = new Font(font.Families[0], 10, FontStyle.Regular),
                };

                Button deleteSelectedProduct = new Button() {
                    Size = new Size(90, selectedProductName.Height),
                    Font = new Font(font.Families[0], 10, FontStyle.Regular),
                    Text = "Remove",
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                };
                deleteSelectedProduct.FlatStyle = FlatStyle.Flat;
                deleteSelectedProduct.FlatAppearance.BorderSize = 0;

                selectedProductName.Width = (selectedProductFrame.Width - deleteSelectedProduct.Width) - 20;
                selectedProductName.Location = new Point(10, 10);
                selectedProductQuantity.Location = new Point(10, (selectedProductName.Location.Y + selectedProductName.Height) + 10);
                deleteSelectedProduct.Location = new Point((selectedProductFrame.Width - deleteSelectedProduct.Width) - 10, selectedProductName.Location.Y);
                selectedProductPrice.Location = new Point((deleteSelectedProduct.Location.X + deleteSelectedProduct.Width) - selectedProductPrice.Width, (selectedProductName.Location.Y + selectedProductName.Height) + 10);

                selectedProductFrame.Controls.Add(selectedProductName);
                selectedProductFrame.Controls.Add(selectedProductPrice);
                selectedProductFrame.Controls.Add(selectedProductQuantity);
                selectedProductFrame.Controls.Add(deleteSelectedProduct);

                Form1.orderListDisplay.Controls.Add(selectedProductFrame);

                deleteSelectedProduct.Click += (sender, eventArgs) => {
                    Form1.orderList.Remove(entry.Key);
                    Form1.orderListDisplay.Controls.Clear(); // Clear display
                    displayLists(); // Rebuild display after deletion
                };

                yOffset += selectedProductFrame.Height + 5;
            }

            // Update total price display after the loop
            Form1.OrderTotalPriceLabel.Text = $"Total Price:\n{Form1.totalPrice:F2}";
        }

        public void orderUpdateIndicate() {

            Panel updateIndicateScreen = new Panel()
            {
                Size = new Size(selectedProductBoxScreen.Width, selectedProductBoxScreen.Height),
            };

            // Create the header label
            Label updateIndicateHeader = new Label
            {
                Size = new Size(updateIndicateScreen.Width, 50),
                Text = "Order Updated",
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label updateIndicateSubtext = new Label
            {
                Size = new Size(updateIndicateScreen.Width, 30),
                Text = "Your specified changes have been added to the Order List",
                Font = new Font(font.Families[0], 12, FontStyle.Regular),
                TextAlign = ContentAlignment.TopCenter
            };
            updateIndicateHeader.Location = new Point((updateIndicateScreen.Width - updateIndicateHeader.Width) / 2,(updateIndicateScreen.Height / 2) - updateIndicateHeader.Height);
            updateIndicateSubtext.Location = new Point((updateIndicateScreen.Width - updateIndicateSubtext.Width) / 2,(updateIndicateHeader.Location.Y + updateIndicateHeader.Height + 20));

            updateIndicateScreen.Controls.Add(updateIndicateHeader);
            updateIndicateScreen.Controls.Add(updateIndicateSubtext);

            this.Controls.Add(updateIndicateScreen);
            updateIndicateScreen.BringToFront();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
            {
                Interval = 3000
            };

            timer.Tick += (sender, e) =>
            {
                timer.Stop();
                timer.Dispose();
                amountToOrder = 1;
                this.Controls.Remove(updateIndicateScreen);
                updateIndicateScreen.Dispose();
                this.Controls.Remove(selectedProductBoxScreen);
                selectedProductBoxScreen.Dispose();
                activeProductBoxScreen = null;
                this.Controls.Add(catalogueHeader);
                catalogueHeader.BringToFront();
                AutoScroll = true;
                this.Refresh();
            };

            timer.Start();
        }
    }
}
