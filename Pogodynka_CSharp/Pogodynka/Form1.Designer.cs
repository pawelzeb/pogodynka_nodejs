namespace Pogodynka
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl = new TabControl();
            tabWeather = new TabPage();
            label3 = new Label();
            groupBox4 = new GroupBox();
            labelSunSet = new Label();
            labelSunRise = new Label();
            pbSun = new PictureBox();
            groupBox3 = new GroupBox();
            labelPressure = new Label();
            pbPressure = new PictureBox();
            labelDt = new Label();
            labelLon = new Label();
            labelLat = new Label();
            labelCity = new Label();
            groupBox2 = new GroupBox();
            labelWindSpeed = new Label();
            pbWind = new PictureBox();
            groupBox1 = new GroupBox();
            labelDescription = new Label();
            labelTempMin = new Label();
            labelTempMax = new Label();
            labelTemp = new Label();
            pbIcon = new PictureBox();
            tabForecast = new TabPage();
            tabHistory = new TabPage();
            labelError = new Label();
            notifyIcon1 = new NotifyIcon(components);
            menuStrip1 = new MenuStrip();
            plikToolStripMenuItem = new ToolStripMenuItem();
            saveAs = new ToolStripMenuItem();
            zamknijAplikacjęToolStripMenuItem = new ToolStripMenuItem();
            txtCity = new TextBox();
            txtCC = new TextBox();
            label1 = new Label();
            label2 = new Label();
            btnGo = new Button();
            btnFollow = new Button();
            dateTimePicker = new DateTimePicker();
            tabControl.SuspendLayout();
            tabWeather.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbSun).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbPressure).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbWind).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
            tabHistory.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabWeather);
            tabControl.Controls.Add(tabForecast);
            tabControl.Controls.Add(tabHistory);
            tabControl.Location = new Point(12, 73);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(695, 717);
            tabControl.TabIndex = 0;
            tabControl.Selecting += tabControl_Selecting;
            // 
            // tabWeather
            // 
            tabWeather.Controls.Add(label3);
            tabWeather.Controls.Add(groupBox4);
            tabWeather.Controls.Add(groupBox3);
            tabWeather.Controls.Add(labelDt);
            tabWeather.Controls.Add(labelLon);
            tabWeather.Controls.Add(labelLat);
            tabWeather.Controls.Add(labelCity);
            tabWeather.Controls.Add(groupBox2);
            tabWeather.Controls.Add(groupBox1);
            tabWeather.Location = new Point(4, 24);
            tabWeather.Name = "tabWeather";
            tabWeather.Padding = new Padding(3);
            tabWeather.Size = new Size(687, 689);
            tabWeather.TabIndex = 0;
            tabWeather.Text = "Pogoda";
            tabWeather.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Fuchsia;
            label3.Location = new Point(194, 15);
            label3.Name = "label3";
            label3.Size = new Size(0, 37);
            label3.TabIndex = 10;
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox4.BackColor = Color.SteelBlue;
            groupBox4.Controls.Add(labelSunSet);
            groupBox4.Controls.Add(labelSunRise);
            groupBox4.Controls.Add(pbSun);
            groupBox4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox4.ForeColor = SystemColors.HighlightText;
            groupBox4.Location = new Point(32, 478);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(619, 194);
            groupBox4.TabIndex = 9;
            groupBox4.TabStop = false;
            groupBox4.Text = "Wschód-Zachód Słońca";
            // 
            // labelSunSet
            // 
            labelSunSet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelSunSet.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelSunSet.ForeColor = SystemColors.HighlightText;
            labelSunSet.Location = new Point(472, 120);
            labelSunSet.Name = "labelSunSet";
            labelSunSet.Size = new Size(114, 25);
            labelSunSet.TabIndex = 9;
            labelSunSet.Text = "20:34:28";
            labelSunSet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelSunRise
            // 
            labelSunRise.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelSunRise.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelSunRise.ForeColor = SystemColors.HighlightText;
            labelSunRise.Location = new Point(38, 120);
            labelSunRise.Name = "labelSunRise";
            labelSunRise.Size = new Size(114, 25);
            labelSunRise.TabIndex = 8;
            labelSunRise.Text = "06:34:28";
            labelSunRise.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pbSun
            // 
            pbSun.BackColor = Color.SteelBlue;
            pbSun.BackgroundImageLayout = ImageLayout.Stretch;
            pbSun.Location = new Point(158, 32);
            pbSun.Name = "pbSun";
            pbSun.Size = new Size(308, 147);
            pbSun.TabIndex = 0;
            pbSun.TabStop = false;
            pbSun.Paint += pbSun_Paint;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox3.BackColor = Color.SteelBlue;
            groupBox3.Controls.Add(labelPressure);
            groupBox3.Controls.Add(pbPressure);
            groupBox3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox3.ForeColor = SystemColors.HighlightText;
            groupBox3.Location = new Point(378, 210);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(273, 248);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "Ciśnienie";
            // 
            // labelPressure
            // 
            labelPressure.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelPressure.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelPressure.ForeColor = SystemColors.HighlightText;
            labelPressure.Location = new Point(6, 113);
            labelPressure.Name = "labelPressure";
            labelPressure.Size = new Size(114, 25);
            labelPressure.TabIndex = 8;
            labelPressure.Text = "1024";
            labelPressure.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pbPressure
            // 
            pbPressure.BackColor = Color.SteelBlue;
            pbPressure.BackgroundImageLayout = ImageLayout.Stretch;
            pbPressure.Location = new Point(149, 43);
            pbPressure.Name = "pbPressure";
            pbPressure.Size = new Size(108, 190);
            pbPressure.TabIndex = 0;
            pbPressure.TabStop = false;
            pbPressure.Paint += pressure_Paint;
            // 
            // labelDt
            // 
            labelDt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelDt.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDt.ForeColor = SystemColors.ActiveCaptionText;
            labelDt.Location = new Point(399, 21);
            labelDt.Name = "labelDt";
            labelDt.RightToLeft = RightToLeft.No;
            labelDt.Size = new Size(252, 31);
            labelDt.TabIndex = 4;
            labelDt.Text = "czas odczytu: (12:31:20)";
            labelDt.TextAlign = ContentAlignment.MiddleRight;
            labelDt.UseCompatibleTextRendering = true;
            // 
            // labelLon
            // 
            labelLon.Location = new Point(108, 47);
            labelLon.Name = "labelLon";
            labelLon.Size = new Size(98, 15);
            labelLon.TabIndex = 3;
            labelLon.Text = "lon: 21.0118°";
            // 
            // labelLat
            // 
            labelLat.Location = new Point(32, 47);
            labelLat.Name = "labelLat";
            labelLat.Size = new Size(75, 15);
            labelLat.TabIndex = 2;
            labelLat.Text = "lat: 52.2298°";
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCity.ForeColor = SystemColors.ActiveCaptionText;
            labelCity.Location = new Point(32, 3);
            labelCity.Name = "labelCity";
            labelCity.Size = new Size(155, 40);
            labelCity.TabIndex = 1;
            labelCity.Text = "Warszawa";
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.SteelBlue;
            groupBox2.Controls.Add(labelWindSpeed);
            groupBox2.Controls.Add(pbWind);
            groupBox2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox2.ForeColor = SystemColors.HighlightText;
            groupBox2.Location = new Point(32, 210);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(273, 248);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Wiatr";
            groupBox2.Enter += groupBoxWind;
            // 
            // labelWindSpeed
            // 
            labelWindSpeed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelWindSpeed.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelWindSpeed.ForeColor = SystemColors.HighlightText;
            labelWindSpeed.Location = new Point(76, 15);
            labelWindSpeed.Name = "labelWindSpeed";
            labelWindSpeed.Size = new Size(114, 25);
            labelWindSpeed.TabIndex = 8;
            labelWindSpeed.Text = "8 m/s";
            labelWindSpeed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pbWind
            // 
            pbWind.BackColor = Color.SteelBlue;
            pbWind.BackgroundImageLayout = ImageLayout.Stretch;
            pbWind.Location = new Point(44, 43);
            pbWind.Name = "pbWind";
            pbWind.Size = new Size(190, 190);
            pbWind.TabIndex = 0;
            pbWind.TabStop = false;
            pbWind.Paint += pbWind_Paint;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.BackColor = Color.SteelBlue;
            groupBox1.Controls.Add(labelDescription);
            groupBox1.Controls.Add(labelTempMin);
            groupBox1.Controls.Add(labelTempMax);
            groupBox1.Controls.Add(labelTemp);
            groupBox1.Controls.Add(pbIcon);
            groupBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.ForeColor = SystemColors.HighlightText;
            groupBox1.Location = new Point(32, 65);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(619, 114);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Pogoda";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // labelDescription
            // 
            labelDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelDescription.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDescription.ForeColor = SystemColors.HighlightText;
            labelDescription.Location = new Point(367, 25);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(252, 25);
            labelDescription.TabIndex = 7;
            labelDescription.Text = "Clear sky";
            labelDescription.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelTempMin
            // 
            labelTempMin.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTempMin.ForeColor = SystemColors.HighlightText;
            labelTempMin.Location = new Point(90, 87);
            labelTempMin.Name = "labelTempMin";
            labelTempMin.Size = new Size(84, 15);
            labelTempMin.TabIndex = 6;
            labelTempMin.Text = "Min: 23°C";
            // 
            // labelTempMax
            // 
            labelTempMax.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTempMax.ForeColor = SystemColors.HighlightText;
            labelTempMax.Location = new Point(14, 87);
            labelTempMax.Name = "labelTempMax";
            labelTempMax.Size = new Size(78, 15);
            labelTempMax.TabIndex = 5;
            labelTempMax.Text = "Max: 23°C";
            // 
            // labelTemp
            // 
            labelTemp.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTemp.ForeColor = SystemColors.HighlightText;
            labelTemp.Location = new Point(6, 19);
            labelTemp.Name = "labelTemp";
            labelTemp.Size = new Size(146, 65);
            labelTemp.TabIndex = 5;
            labelTemp.Text = "23°C";
            // 
            // pbIcon
            // 
            pbIcon.BackColor = Color.SteelBlue;
            pbIcon.BorderStyle = BorderStyle.FixedSingle;
            pbIcon.Location = new Point(158, 28);
            pbIcon.Name = "pbIcon";
            pbIcon.Size = new Size(50, 50);
            pbIcon.TabIndex = 0;
            pbIcon.TabStop = false;
            // 
            // tabForecast
            // 
            tabForecast.AutoScroll = true;
            tabForecast.Location = new Point(4, 24);
            tabForecast.Name = "tabForecast";
            tabForecast.Padding = new Padding(3);
            tabForecast.Size = new Size(687, 689);
            tabForecast.TabIndex = 1;
            tabForecast.Text = "Prognoza";
            tabForecast.UseVisualStyleBackColor = true;
            // 
            // tabHistory
            // 
            tabHistory.AutoScroll = true;
            tabHistory.Controls.Add(labelError);
            tabHistory.Location = new Point(4, 24);
            tabHistory.Name = "tabHistory";
            tabHistory.Size = new Size(687, 689);
            tabHistory.TabIndex = 2;
            tabHistory.Text = "Historia";
            tabHistory.UseVisualStyleBackColor = true;
            // 
            // labelError
            // 
            labelError.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            labelError.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelError.ForeColor = SystemColors.ControlDarkDark;
            labelError.Location = new Point(22, 215);
            labelError.Name = "labelError";
            labelError.Size = new Size(636, 265);
            labelError.TabIndex = 0;
            labelError.Text = "Brak wpisów dla <Częstochowa>/<pl>.Dodaj Częstochowa do obserwowanych, aby zacząć rejestrować dane meteo";
            labelError.TextAlign = ContentAlignment.MiddleCenter;
            labelError.Visible = false;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { plikToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(719, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            plikToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAs, zamknijAplikacjęToolStripMenuItem });
            plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            plikToolStripMenuItem.Size = new Size(38, 20);
            plikToolStripMenuItem.Text = "&Plik";
            // 
            // saveAs
            // 
            saveAs.Enabled = false;
            saveAs.Name = "saveAs";
            saveAs.Size = new Size(168, 22);
            saveAs.Text = "Zapisz jako...";
            saveAs.Click += saveAs_Click;
            // 
            // zamknijAplikacjęToolStripMenuItem
            // 
            zamknijAplikacjęToolStripMenuItem.Name = "zamknijAplikacjęToolStripMenuItem";
            zamknijAplikacjęToolStripMenuItem.Size = new Size(168, 22);
            zamknijAplikacjęToolStripMenuItem.Text = "&Zamknij Aplikację";
            zamknijAplikacjęToolStripMenuItem.Click += onClose;
            // 
            // txtCity
            // 
            txtCity.AllowDrop = true;
            txtCity.Location = new Point(74, 31);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(107, 23);
            txtCity.TabIndex = 2;
            txtCity.Text = "Kraków";
            // 
            // txtCC
            // 
            txtCC.AllowDrop = true;
            txtCC.Location = new Point(306, 31);
            txtCC.Name = "txtCC";
            txtCC.Size = new Size(48, 23);
            txtCC.TabIndex = 3;
            txtCC.Text = "pl";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 35);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 4;
            label1.Text = "Lokacja:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(187, 35);
            label2.Name = "label2";
            label2.Size = new Size(113, 15);
            label2.TabIndex = 5;
            label2.Text = "2 literowy kod kraju:";
            // 
            // btnGo
            // 
            btnGo.Location = new Point(360, 31);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(47, 23);
            btnGo.TabIndex = 6;
            btnGo.Text = "GO";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += go_Click;
            // 
            // btnFollow
            // 
            btnFollow.BackColor = Color.LightCoral;
            btnFollow.Location = new Point(415, 30);
            btnFollow.Name = "btnFollow";
            btnFollow.Size = new Size(77, 23);
            btnFollow.TabIndex = 7;
            btnFollow.Text = "+Obserwuj";
            btnFollow.UseVisualStyleBackColor = false;
            btnFollow.Visible = false;
            btnFollow.Click += follow_Click;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(498, 31);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(200, 23);
            dateTimePicker.TabIndex = 1;
            dateTimePicker.ValueChanged += pickedDate;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(719, 802);
            Controls.Add(dateTimePicker);
            Controls.Add(btnFollow);
            Controls.Add(btnGo);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtCC);
            Controls.Add(txtCity);
            Controls.Add(tabControl);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Pogodynka";
            FormClosing += Form1_FormClosing;
            tabControl.ResumeLayout(false);
            tabWeather.ResumeLayout(false);
            tabWeather.PerformLayout();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbSun).EndInit();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbPressure).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbWind).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
            tabHistory.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private TabControl tabControl;
        private TabPage tabWeather;
        private TabPage tabForecast;
        private TabPage tabHistory;
        private GroupBox groupBox1;
        private NotifyIcon notifyIcon1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem plikToolStripMenuItem;
        private ToolStripMenuItem zamknijAplikacjęToolStripMenuItem;
        private PictureBox pbIcon;
        private GroupBox groupBox2;
        private PictureBox pbWind;
        private Label labelCity;
        private Label labelDt;
        private Label labelLon;
        private Label labelLat;
        private Label labelTemp;
        private Label labelTempMin;
        private Label labelTempMax;
        private Label labelDescription;
        private Label labelWindSpeed;
        private GroupBox groupBox3;
        private Label labelPressure;
        private PictureBox pbPressure;
        private GroupBox groupBox4;
        private PictureBox pbSun;
        private Label labelSunSet;
        private Label labelSunRise;
        private TextBox txtCity;
        private TextBox txtCC;
        private Label label1;
        private Label label2;
        private Button btnGo;
        private Button btnFollow;
        private Label labelError;
        private Label label3;
        private DateTimePicker dateTimePicker;
        private ToolStripMenuItem saveAs;
    }
}
