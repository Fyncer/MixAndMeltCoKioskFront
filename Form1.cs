using System.Drawing.Text;

namespace MixAndMeltCo {
    public partial class Form1 : Form {
        private Panel catalogueViewer;
        public Form1() {
            InitializeComponent();
            //Full Screen & No Border Display
            this.WindowState = FormWindowState.Maximized; // Maximizes the form to full screen
            this.FormBorderStyle = FormBorderStyle.None; // Removes the border and title bar
            this.Size = Screen.PrimaryScreen.Bounds.Size;

            //Embedding Custom Fonts
            PrivateFontCollection font = new PrivateFontCollection();
            string fontPath1 = Path.Combine(Application.StartupPath, "CustomFonts/RethinkSans-ExtraBold.ttf");
            string fontPath2 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Light.ttf");
            string fontPath3 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Regular.ttf");

            try {
                font.AddFontFile(fontPath1);
                font.AddFontFile(fontPath2);
                font.AddFontFile(fontPath3);
            }
            catch (Exception ex) {
                MessageBox.Show($"Failed to load custom fonts: {ex.Message}");
            }


            SplitContainer splitKiosk = new SplitContainer() {
                Size = new Size(this.Width, this.Height),
                IsSplitterFixed = true
            };
            splitKiosk.SplitterDistance = (int)(splitKiosk.Width * 0.7);
            splitKiosk.SplitterWidth = 1;
            AddCustomBorderToPanel(splitKiosk.Panel1, 0, 0, 0, 2, System.Drawing.Color.Black);
            this.Controls.Add(splitKiosk);

            Panel menuHeader = new Panel() {
                Size = new Size(splitKiosk.Panel1.Width, splitKiosk.Panel1.Height / 9),
            };
            AddCustomBorderToPanel(menuHeader, 0, 2, 0, 0, System.Drawing.Color.Black);
            splitKiosk.Panel1.Controls.Add(menuHeader);

            Button iceCream = new Button() {
                AutoSize = true,
                Text = "Ice Cream",
                FlatStyle = FlatStyle.Flat
            };
            iceCream.FlatAppearance.BorderSize = 0;
            Button snacks = new Button() {
                AutoSize = true,
                Text = "Snacks",
                FlatStyle = FlatStyle.Flat
            };
            snacks.FlatAppearance.BorderSize = 0;
            Button beverage = new Button() {
                AutoSize = true,
                Text = "Beverages",
                FlatStyle = FlatStyle.Flat
            };
            beverage.FlatAppearance.BorderSize = 0;
            snacks.Font = new Font(font.Families[0], 12, FontStyle.Regular);
            iceCream.Font = new Font(font.Families[0], 12, FontStyle.Regular);
            beverage.Font = new Font(font.Families[0], 12, FontStyle.Regular);
            snacks.Location = new Point((menuHeader.Width - snacks.Width) / 2, (menuHeader.Height - snacks.Height) / 2);
            iceCream.Location = new Point(snacks.Location.X - (snacks.Width + 45), snacks.Location.Y);
            beverage.Location = new Point(snacks.Location.X + (iceCream.Width + 20), snacks.Location.Y);
            menuHeader.Controls.Add(iceCream);
            menuHeader.Controls.Add(snacks);
            menuHeader.Controls.Add(beverage);

            catalogueViewer = new Panel() {
                Size = new Size(splitKiosk.Panel1.Width, splitKiosk.Panel1.Height - menuHeader.Height),
                Location = new Point(0, menuHeader.Height),
                AutoSize = true,
            };
            AddCustomBorderToPanel(catalogueViewer, 0, 0, 0, 2, Color.Black);
            this.Load += new EventHandler(iceCreamCatalogue_Click);
            splitKiosk.Panel1.Controls.Add(catalogueViewer);


            iceCream.Click += iceCreamCatalogue_Click;
            snacks.Click += snacksCatalogue_Click;
            beverage.Click += beverageCatalogue_Click;

            AddCustomBorderToPanel(splitKiosk.Panel2, 0, 0, 2, 0, Color.Black);
            Label orderList_header = new Label() {
                Location = new Point(0, 0),
                Text = "Your Order",
                Font = new Font(font.Families[2], 25, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            orderList_header.Padding = Padding = new Padding(20, ((splitKiosk.Panel2.Height / 9) - 50) / 2, 20, ((splitKiosk.Panel2.Height / 9) - 50) / 2);
            splitKiosk.Panel2.Controls.Add(orderList_header);

            Panel Orderlist_bottom = new Panel() {
                Dock = DockStyle.Bottom
            };
            AddCustomBorderToPanel(Orderlist_bottom, 2, 0, 2, 2, Color.Black);
            splitKiosk.Panel2.Controls.Add(Orderlist_bottom);

            Button ToOrderButton = new Button() {
                Size = new Size(Orderlist_bottom.Width / 2, Orderlist_bottom.Height),
                Location = new Point(Orderlist_bottom.Width / 2, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Black,
                Text = "ORDER",
                ForeColor = Color.White,
                Font = new Font(font.Families[2], 20, FontStyle.Regular),
            };
            ToOrderButton.FlatAppearance.BorderColor = Color.Black; // Border color
            ToOrderButton.FlatAppearance.BorderSize = 2;

            decimal totalPrice = 0.00m;

            Label OrderTotalPriceLabel = new Label() {
                Size = new Size(Orderlist_bottom.Width / 2, Orderlist_bottom.Height),
                Location = new Point(0, 0),
                Text = $"Total Price:\n{totalPrice}",
                Font = new Font(font.Families[0], 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };
            AddCustomBorderToLabel(OrderTotalPriceLabel, 2, 0, 2, 2, Color.Black);

            Orderlist_bottom.Controls.Add(ToOrderButton);
            Orderlist_bottom.Controls.Add(OrderTotalPriceLabel);

            Panel orderListContainer = new Panel() {
                AutoScroll = true,
                Size = new Size(splitKiosk.Panel2.Width, splitKiosk.Panel2.Height - (orderList_header.Height + Orderlist_bottom.Height)),
                Location = new Point(0, orderList_header.Height),
                BackColor = Color.Transparent
            };
            AddCustomBorderToPanel(orderListContainer, 0, 0, 2, 0, Color.Black);
            splitKiosk.Panel2.Controls.Add(orderListContainer);

        }

        private void DisplayFormInPanel(Form formToDisplay, Panel targetPanel) {
            formToDisplay.TopLevel = false;                // Allows the form to be embedded
            formToDisplay.Dock = DockStyle.Fill;        // Makes the form fill the 
            targetPanel.Controls.Clear();                 // Clears existing controls in the panel
            targetPanel.Controls.Add(formToDisplay);      // Adds the form to the panel
            AddCustomBorderToPanel(targetPanel, 0, 0, 0, 2, Color.Black);
            formToDisplay.Show();
        }

        private void iceCreamCatalogue_Click(object sender, EventArgs e) {
            IceCream cs = new IceCream(catalogueViewer.Width, catalogueViewer.Height);
            DisplayFormInPanel(cs, catalogueViewer);
            AddCustomBorderToPanel(catalogueViewer, 0, 0, 0, 2, Color.Black);
        }
        private void snacksCatalogue_Click(object sender, EventArgs e) {
            Snack sk = new Snack(catalogueViewer.Width, catalogueViewer.Height);
            DisplayFormInPanel(sk, catalogueViewer);
            AddCustomBorderToPanel(catalogueViewer, 0, 0, 0, 2, Color.Black);
        }
        private void beverageCatalogue_Click(object sender, EventArgs e) {
            Beverages bv = new Beverages(catalogueViewer.Width, catalogueViewer.Height);
            DisplayFormInPanel(bv, catalogueViewer);
            AddCustomBorderToPanel(catalogueViewer, 0, 0, 0, 2, Color.Black);
        }

        private void AddCustomBorderToPanel(Panel panel, int top, int bottom, int left, int right, Color borderColor) {
            panel.Paint += (s, e) => {
                Graphics g = e.Graphics;

                // Use a solid brush to fill rectangles for each side of the border
                using (Brush brush = new SolidBrush(borderColor)) {
                    // Top border
                    if (top > 0)
                        g.FillRectangle(brush, 0, 0, panel.Width, top);

                    // Bottom border
                    if (bottom > 0)
                        g.FillRectangle(brush, 0, panel.Height - bottom, panel.Width, bottom);

                    // Left border
                    if (left > 0)
                        g.FillRectangle(brush, 0, 0, left, panel.Height);

                    // Right border
                    if (right > 0)
                        g.FillRectangle(brush, panel.Width - right, 0, right, panel.Height);
                }
            };

            // Trigger the Paint event to apply the border immediately
            panel.Invalidate();
        }

        private void AddCustomBorderToLabel(Label label, int top, int bottom, int left, int right, Color borderColor) {
            label.Paint += (s, e) => {
                Graphics g = e.Graphics;

                // Use a solid brush to fill rectangles for each side of the border
                using (Brush brush = new SolidBrush(borderColor)) {
                    // Top border
                    if (top > 0)
                        g.FillRectangle(brush, 0, 0, label.Width, top);

                    // Bottom border
                    if (bottom > 0)
                        g.FillRectangle(brush, 0, label.Height - bottom, label.Width, bottom);

                    // Left border
                    if (left > 0)
                        g.FillRectangle(brush, 0, 0, left, label.Height);

                    // Right border
                    if (right > 0)
                        g.FillRectangle(brush, label.Width - right, 0, right, label.Height);
                }
            };

            // Redraw the label to apply the custom border
            label.Invalidate();
        }
    }
}