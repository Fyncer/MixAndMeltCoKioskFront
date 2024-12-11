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

namespace MixAndMeltCo
{
    public partial class Opener : Form
    {
        public Opener()
        {
            PrivateFontCollection font = new PrivateFontCollection();
            string fontPath1 = Path.Combine(Application.StartupPath, "CustomFonts/RethinkSans-ExtraBold.ttf");
            string fontPath2 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Light.ttf");
            string fontPath3 = Path.Combine(Application.StartupPath, "CustomFonts/Fustat-Regular.ttf");
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

            InitializeComponent();
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            Label subtext = new Label() {
                Width = this.Width,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(font.Families[0], 15, FontStyle.Regular),
                Text = "Welcome To",
            };
            Label name = new Label()
            {
                Width = this.Width,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(font.Families[2], 40, FontStyle.Bold),
                Text = "Mix And Melt Co.",
            };
            Button instruction = new Button() {
                Width = 200,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(font.Families[0], 15, FontStyle.Regular),
                Text = "Start Order",
            };
            instruction.FlatStyle = FlatStyle.Flat;
            instruction.FlatAppearance.BorderColor = Color.Black;
            name.Location = new Point(10, (this.Height - name.Height) / 2);
            subtext.Location = new Point(10, (name.Location.Y - subtext.Height));
            instruction.Location = new Point(((this.Width - instruction.Width) / 2) + 10, ((this.Height - instruction.Height) / 2) + 250);

            instruction.Click += (s, args) => {
                Form1 asshat = new Form1();
                asshat.Show();
                this.Hide();
            };

            this.Controls.Add(name);
            this.Controls.Add(subtext);
            this.Controls.Add(instruction);
        }
    }
}
