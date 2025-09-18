using Pogodynka.Properties;
using System.Drawing.Drawing2D;
using System.Net;
using System.Text.Json;
using Microsoft.Toolkit.Uwp.Notifications;
using WebSocketSharp;
using Firebase.Database;
using Firebase.Database.Query;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.DirectoryServices.ActiveDirectory;
using Pogodynka.Model;
using Pogodynka.Utils;


namespace Pogodynka
{
    public partial class Form1 : Form, IAlertObserver
    {
        int rotSun = 0;
        int rotWind = 0;

        bool bHideSun = false;
        bool bInitForecast = true;
        bool bInitHistory = true;

        public int iPressure { get; private set; }

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tabHistory, true, null);

            var city = Properties.Settings.Default.mru_city;
            var cc = Properties.Settings.Default.mru_cc;
            txtCC.Text = cc;
            txtCity.Text = city;

            tabControl.TabPages.Clear();

            //uruchamiamy śledzenie alertów
            //ponieważ to singleton nie musimy przechowywać instancji jeśli jej nie potrzebujemy
            AlertService.getInstance(this);
        }

        

        private void setForecastData(WeatherEntry forecast)
        {
            var weekDays = new string[] { "Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota" };
            var data = DateTime.Parse(forecast.dt_txt);
            var sDay = weekDays[(uint)data.DayOfWeek];
            addForecastDay(sDay, data.ToString("HH:mm"), forecast.weather[0].icon, ((int)(Globalne.KelvinZero + forecast.main.temp)).ToString());
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void onClose(object sender, EventArgs e)
        {
            var alertService = AlertService.getInstance(this);
            alertService.Stop();

            Application.Exit();
        }

        private void groupBoxWind(object sender, EventArgs e)
        {

        }

        private void pbWindH_Paint(object sender, PaintEventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb == null) return;
            int rot = pb.Tag is int ? (int)pb.Tag : 0;

            Graphics g = e.Graphics;
            Image bearingImg = Properties.Resources.bearing;
            Bitmap rotated = new Bitmap(bearingImg.Width, bearingImg.Height);
            rotated.SetResolution(bearingImg.HorizontalResolution, bearingImg.VerticalResolution);
            using (Graphics g1 = Graphics.FromImage(rotated))
            {
                g1.TranslateTransform(bearingImg.Width / 2, bearingImg.Height / 2);
                g1.RotateTransform(rot);
                g1.TranslateTransform(-bearingImg.Width / 2, -bearingImg.Height / 2);
                g1.DrawImage(bearingImg, new Point(0, 0));
            }
            g.DrawImage(rotated, new Rectangle(0, 0, 50, 50));
        }
        private void pbWind_Paint(object sender, PaintEventArgs e)
        {
            int w = pbWind.Width;
            int h = pbWind.Height;
            Graphics g = e.Graphics;
            Image roseImg = Properties.Resources.rose;
            Image bearingImg = Properties.Resources.bearing;
            //bearingImg.RotateFlip(RotateFlipType.Rotate90FlipNone);
            g.DrawImage(roseImg, new Rectangle(10, 10, w - 20, h - 20));
            Bitmap rotated = new Bitmap(bearingImg.Width, bearingImg.Height);
            rotated.SetResolution(bearingImg.HorizontalResolution, bearingImg.VerticalResolution);
            using (Graphics g1 = Graphics.FromImage(rotated))
            {
                g1.TranslateTransform(bearingImg.Width / 2, bearingImg.Height / 2);
                g1.RotateTransform(rotWind);
                g1.TranslateTransform(-bearingImg.Width / 2, -bearingImg.Height / 2);
                g1.DrawImage(bearingImg, new Point(0, 0));
            }
            g.DrawImage(rotated, new Rectangle(60, 60, w - 120, h - 120));
            //g.DrawRectangle(pen, 10, 10, w - 20, h - 20);
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pbWind.Width - 1, pbWind.Height - 1);
            pbWind.Region = new Region(gp);
        }

        private void pbSun_Paint(object sender, PaintEventArgs e)
        {
            int w = pbSun.Width;
            int h = pbSun.Height;
            Graphics g = e.Graphics;
            Image sunLine = Properties.Resources.sun_line;
            Image sun = Properties.Resources.sun;
            g.DrawImage(sunLine, new Rectangle(10, 10, w - 20, h - 20));
            Bitmap rotated = new Bitmap(sun.Width, sun.Height);
            rotated.SetResolution(sun.HorizontalResolution, sun.VerticalResolution);
            using (Graphics g1 = Graphics.FromImage(rotated))
            {
                g1.TranslateTransform(sun.Width / 2, sun.Height / 2);
                g1.RotateTransform(rotSun);
                g1.TranslateTransform(-sun.Width / 2, -sun.Height / 2);
                g1.DrawImage(sun, new Point(0, 0));
            }
            g.DrawImage(rotated, new Rectangle(55, -14, 200, 200));
            //Thread.Sleep(100);
            //pbSun.Invalidate();
        }
        private void pressure_Paint(object sender, PaintEventArgs e)
        {
            int w = pbPressure.Width;
            int h = pbPressure.Height;
            Graphics g = e.Graphics;
            Pen penChk = new Pen(Brushes.GreenYellow);
            penChk.Width = 4;
            penChk.Width = 6;
            var min = 870;
            var max = 1086;
            var zakres = ((iPressure - min) / (float)(max - min));
            g.FillRectangle(Brushes.GreenYellow, new Rectangle(0, 0, w, h));
            g.FillRectangle(Brushes.OrangeRed, new Rectangle(0, h - (int)(zakres * h), w, h));
            g.DrawRectangle(penChk, new Rectangle(0, 0, w, h));

        }
        void followCity(string city, string cc)
        {
            String baseurl = $"{Globalne.nodejs_url}/follow/{city}/{cc}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            Console.WriteLine(baseurl);
            request.Method = "PUT";
            request.Accept = "application/json";
            //request.Credentials = new NetworkCredential(username, password);
            request.UserAgent = "curl/7.37.0";
            request.ContentType = "application/json";
            var response = request.GetResponse();
        }
        void unfollowCity(string city, string cc)
        {
            String baseurl = $"{Globalne.nodejs_url}/follow/{city}/{cc}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            Console.WriteLine(baseurl);
            request.Method = "DELETE";
            request.Accept = "application/json";
            //request.Credentials = new NetworkCredential(username, password);
            request.UserAgent = "curl/7.37.0";
            request.ContentType = "application/json";
            var response = request.GetResponse();
        }

        private void go_Click(object sender, EventArgs e)
        {
            bool isConnected = InternetCheck.IsConnected();
            if(!isConnected)
            {
                MessageBox.Show("Brak połączenia z internetem :(");
                return;
            }
            bInitForecast = true;
            bInitHistory = true;
            tabControl.TabPages.Clear();
            string city = txtCity.Text.Trim();
            string cc = txtCC.Text.Trim();

            if (city.Length < 1 || cc.Length != 2)
            {
                MessageBox.Show("Podaj najpierw lokację i 2 znakowy kod kraju do którego lokacja należy", "Tytuł okna");
                return;
            }
            Properties.Settings.Default.mru_city = city;
            Properties.Settings.Default.mru_cc = cc;
            Settings.Default.Save();
            String baseurl = $"{Globalne.nodejs_url}/weather/{city}/{cc}";
            Console.WriteLine(baseurl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            request.Timeout = 4000;
            Console.WriteLine(baseurl);
            request.Method = "GET";
            request.Accept = "application/json";
            //request.Credentials = new NetworkCredential(username, password);
            request.UserAgent = "curl/7.37.0";
            request.ContentType = "application/json";

            try
            {
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    Console.WriteLine("Odebrany JSON:");
                    Console.WriteLine(json);

                    var dataModel = JsonSerializer.Deserialize<WeatherModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (dataModel != null && dataModel.Name != null)
                    {
                        tz = dataModel.Timezone;
                        setData(dataModel);
                    }
                    else
                    {
                        btnFollow.Visible = false;
                        tabControl.TabPages.Clear();
                    }
                }
            }
            catch(Exception ex) {
                Console.Error.WriteLine("BŁĄD!:");
                Console.Error.WriteLine(ex.ToString());
                MessageBox.Show(this, "Brak połączenia z serwerem.\nSprawdź czy serwer Azure jest podłączony.");
            }
        }

        async private void setData(WeatherModel dm)
        {
            Image img = Resources.bearing;


            labelCity.Text = dm.Name;
            //WAŻNE po przypizaniu labelCity
            if (checkIfFollow())
            {
                btnFollow.BackColor = Color.LightCoral;
                btnFollow.Text = "-Obserwuj";
            }
            else
            {
                btnFollow.BackColor = Color.DarkSeaGreen;
                btnFollow.Text = "+Obserwuj";
            }
            btnFollow.Visible = true;
            tabControl.TabPages.AddRange(new TabPage[] { tabWeather, tabForecast, tabHistory });

            labelLat.Text = $"{dm.Coord.Lat}°";
            labelLon.Text = $"{dm.Coord.Lon}°";
            labelTemp.Text = $"{(int)(Globalne.KelvinZero + dm.Main.Temp)}°C";
            labelTempMax.Text = $"{(int)(Globalne.KelvinZero + dm.Main.Temp_Max)}°C";
            labelTempMin.Text = $"{(int)(Globalne.KelvinZero + dm.Main.Temp_Min)}°C";
            labelDescription.Text = $"{dm.Weather[0].Description}";
            rotWind = dm.Wind.Deg;

            DateTime sunRise = DateTimeOffset.FromUnixTimeSeconds(dm.Sys.Sunrise * 1).LocalDateTime;
            DateTime sunSet = DateTimeOffset.FromUnixTimeSeconds(dm.Sys.Sunset * 1).LocalDateTime;
            DateTime dt = DateTimeOffset.FromUnixTimeSeconds(dm.Dt * 1).LocalDateTime;

            labelDt.Text = $"czas odczytu: ({dt.ToString("HH:mm:ss")})";
            labelSunSet.Text = sunSet.ToString("HH:mm:ss");
            labelSunRise.Text = sunRise.ToString("HH:mm:ss");
            pbWind.Invalidate();

            labelWindSpeed.Text = $"{dm.Wind.Speed} m/s";
            iPressure = dm.Main.Pressure;
            labelPressure.Text = $"{dm.Main.Pressure}mBar";


            if (dt.Ticks < sunRise.Ticks || dt.Ticks > sunSet.Ticks)
                bHideSun = true;
            else
                bHideSun = false;
            TimeSpan diff = sunSet - sunRise;

            var date = DateTime.Now;
            TimeSpan now = date - sunRise;

            rotSun = (int)(-90f + (float)now.TotalMilliseconds / (float)diff.TotalMilliseconds * 180.0f);
            string formatted = diff.ToString(@"hh\:mm\:ss");
            pbSun.Invalidate();

            var url = $"http://openweathermap.org/img/w/{dm.Weather[0].Icon}.png";
            try
            {
                pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                using (HttpClient client = new HttpClient())
                {
                    var imageBytes = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pbIcon.Image = Image.FromStream(ms);
                        pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                pbIcon.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool checkIfFollow()
        {
            var followCities = Properties.Settings.Default.follow_cities;
            var city = labelCity.Text;
            var cc = txtCC.Text.Trim();
            var follow = $"{city}_{cc}";
            var cities = followCities.Split("|");

            return cities.Contains(follow);
        }
        private bool checkIfFollow(string follow)
        {
            var followCities = Settings.Default.follow_cities;
            var cities = followCities.Split("|");

            return cities.Contains(follow);
        }
        private void follow_Click(object sender, EventArgs e)
        {
            var followCities = Settings.Default.follow_cities;
            var city = labelCity.Text;
            var cc = txtCC.Text.Trim();
            var follow = $"{city}_{cc}";
            var cities = followCities.Split("|").ToList();
            if (cities.Contains(follow))
            {

                cities.Remove(follow);
                followCities = string.Join("|", cities);
                btnFollow.BackColor = Color.DarkSeaGreen;
                btnFollow.Text = "+Obserwuj";
                unfollowCity(city, cc);
            }
            else
            {
                if (followCities.Length > 2)
                    followCities += $"|{follow}";
                else
                    followCities += follow;
                btnFollow.BackColor = Color.LightCoral;
                btnFollow.Text = "-Obserwuj";
                followCity(city, cc);
            }
            Settings.Default.follow_cities = followCities;
            Settings.Default.Save();
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            var city = labelCity.Text;
            var cc = txtCC.Text.Trim();
            if (city.IsNullOrEmpty() || cc.IsNullOrEmpty() || cc.Length != 2) return;

            if (bInitHistory && tabControl.SelectedTab == tabHistory)
            {
                saveAs.Enabled = true;
                tabControl.SelectedTab.Controls.Clear();
                bInitHistory = false;
                String baseurl = $"{Globalne.nodejs_url}/history/{city}/{cc}";
                Console.WriteLine(baseurl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
                Console.WriteLine(baseurl);
                request.Method = "GET";
                request.Accept = "application/json";
                //request.Credentials = new NetworkCredential(username, password);
                request.UserAgent = "curl/7.37.0";
                request.ContentType = "application/json";


                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    jsonHistory = json.Clone();

                    Console.WriteLine(json);
                    if (json.Contains("Brak wpisów dla"))
                    {
                        tabControl.SelectedTab.Controls.Add(labelError);
                        labelError.Text = "Brak wpisów dla <Częstochowa>/<pl>.Dodaj Częstochowa do obserwowanych, aby zacząć rejestrować dane meteo";
                        labelError.Visible = true;
                        return;
                    }
                  
                    histData = JsonSerializer.Deserialize<List<HistoryModel>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    refreshHistoryView();
                }
            }
            else
                saveAs.Enabled = false;

            if (bInitForecast && tabControl.SelectedTab == tabForecast)
            {
                tabControl.SelectedTab.Controls.Clear();
                //var json = File.ReadAllText("weather.json");
                String baseurl = $"{Globalne.nodejs_url}/forecast/{city}/{cc}";
                Console.WriteLine(baseurl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
                Console.WriteLine(baseurl);
                request.Method = "GET";
                request.Accept = "application/json";
                //request.Credentials = new NetworkCredential(username, password);
                request.UserAgent = "curl/7.37.0";
                request.ContentType = "application/json";


                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    Console.WriteLine("Odebrany JSON:");
                    Console.WriteLine(json);
                    var dataModel = JsonSerializer.Deserialize<ForecastModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (dataModel != null && dataModel.cod == "200")
                    {

                        var tommorow = DateTime.Today.AddDays(1);

                        var filtered = dataModel.list
                            .Where(entry =>
                            DateTime.Parse(entry.dt_txt).Date >= tommorow && DateTime.Parse(entry.dt_txt).Hour > 4 && DateTime.Parse(entry.dt_txt).Hour < 23)
                            .ToList();
                        prevDay = "";
                        gb = null;
                        foreach (var forecast in filtered)
                            setForecastData(forecast);
                        //Console.WriteLine(f.dt);
                    }
                    //setDataForecast(dataModel);
                    else
                    {
                        btnFollow.Visible = false;
                        tabControl.TabPages.Clear();
                    }
                    bInitForecast = false;
                }
            }
        }

        private void refreshHistoryView()
        {
            if (histData != null)
            {
                tabHistory.SuspendLayout();
                var cnt = 0;
                prevDay = "";
                DateTime date = dateTimePicker.Value.Date;

                foreach (var data in histData)
                {
                    var weekDays = new string[] { "Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota" };
                    var dat = DateTimeOffset.FromUnixTimeSeconds(data.dt).DateTime;
                    if (dat.Date != date.Date) continue;
                    var sDay = weekDays[(uint)dat.DayOfWeek];
                    if (prevDay != sDay)
                    {
                        headerHistory($"{sDay} {dat.ToString("dd/MM/yyyy")}", cnt++);
                        initHistoryPanel(data, cnt++);
                        prevDay = sDay;
                    }
                    else
                        initHistoryPanel(data, cnt++);
                }
                if (cnt == 0)
                {
                    tabControl.SelectedTab.Controls.Add(labelError);
                    labelError.Text = "Brak wpisów";
                    labelError.Visible = true;

                }
                tabHistory.ResumeLayout();
            }
        }

        string prevDay = "";
        GroupBox gb = null;
        int counter = 0;
        int offsetY = 0;
        private List<HistoryModel>? histData;
        private object jsonHistory;
        private int tz = 0;

        void addForecastDay(string sDay, string sHour, string sIcon, string sTemp)
        {
            if (prevDay != sDay)
            {
                counter = 0;
                offsetY++;
                if (gb != null)
                {
                    tabForecast.Controls.Add(gb);
                }
                else
                    offsetY = 0;
                gb = new GroupBox();
                // 
                // gb
                // 
                gb.BackColor = Color.SteelBlue;
                gb.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
                gb.ForeColor = SystemColors.HighlightText;
                gb.Location = new Point(27, 20 + offsetY * 160);
                gb.Name = "gb";
                gb.Size = new Size(630, 150);
                gb.TabIndex = 0;
                gb.TabStop = false;
                gb.Text = sDay;
            }
            AddForecastHour(gb, sHour, sIcon, sTemp, counter++);

            if (prevDay != sDay)
            {
                prevDay = sDay;
                tabForecast.Controls.Add(gb);
            }


        }

        async private void AddForecastHour(GroupBox gb, string sHour, string sIcon, string sTemp, int offsetX)
        {
            var panel1 = new Panel();
            var pbImg = new PictureBox();
            var hour = new System.Windows.Forms.Label();
            var temp = new System.Windows.Forms.Label();

            gb.Controls.Add(panel1);

            pbImg.BackColor = Color.SteelBlue;
            pbImg.Location = new Point(12, 27);
            pbImg.Name = "pbImg";
            pbImg.Size = new Size(50, 50);
            pbImg.TabIndex = 0;
            pbImg.TabStop = false;
            var url = $"http://openweathermap.org/img/w/{sIcon}.png";
            try
            {
                pbIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                using (HttpClient client = new HttpClient())
                {
                    var imageBytes = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pbImg.Image = Image.FromStream(ms);
                        pbImg.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                pbImg.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // 
            // panel1
            // 
            panel1.BackColor = Color.DarkSlateGray;
            panel1.Controls.Add(temp);
            panel1.Controls.Add(hour);
            panel1.Controls.Add(pbImg);
            panel1.Location = new Point(30 + (offsetX * 100), 34);
            panel1.Name = "panel1";
            panel1.Size = new Size(74, 100);
            panel1.TabIndex = 1;
            // 
            // hour
            // 
            hour.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            hour.Location = new Point(12, 9);
            hour.Name = "hour";
            hour.Size = new Size(50, 15);
            hour.TabIndex = 2;
            hour.Text = sHour;
            hour.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // temp
            // 
            temp.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            temp.Location = new Point(12, 80);
            temp.Name = "temp";
            temp.Size = new Size(50, 15);
            temp.TabIndex = 3;
            temp.Text = $"{sTemp}°C";
            temp.TextAlign = ContentAlignment.MiddleCenter;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AlertService.getInstance(this).Stop();
        }

        private void headerHistory(string sDay, int id)
        {
            Panel mainPanel = new Panel();
            System.Windows.Forms.Label labelDayH = new System.Windows.Forms.Label();

            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.Black;
            mainPanel.Controls.Add(labelDayH);
            mainPanel.Location = new Point(26, 20 + (80 * id));
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(632, 75);
            mainPanel.TabIndex = 1;

            // 
            // labelDayH
            // 
            labelDayH.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDayH.ForeColor = SystemColors.HighlightText;
            labelDayH.Location = new Point(25, 7);
            labelDayH.Name = "labelDayH";
            labelDayH.Size = new Size(587, 60);
            labelDayH.TabIndex = 1;
            labelDayH.Text = sDay;
            labelDayH.TextAlign = ContentAlignment.MiddleCenter;
            tabHistory.Controls.Add(mainPanel);
        }
        async private void initHistoryPanel(HistoryModel data, int id)
        {
            Panel mainPanel = new Panel();
            System.Windows.Forms.Label labelMain = new System.Windows.Forms.Label();
            System.Windows.Forms.Label labelWindSpeedH = new System.Windows.Forms.Label();
            PictureBox pbWindH = new PictureBox();
            System.Windows.Forms.Label labelTimeH = new System.Windows.Forms.Label();
            System.Windows.Forms.Label labelTempH = new System.Windows.Forms.Label();
            PictureBox pbImgH = new PictureBox();

            // 
            // pbImgH
            // 
            pbImgH.BorderStyle = BorderStyle.FixedSingle;
            pbImgH.Location = new Point(17, 14);
            pbImgH.Name = "pbImgH";
            pbImgH.Size = new Size(50, 50);
            pbImgH.TabIndex = 0;
            pbImgH.TabStop = false;
            var url = $"http://openweathermap.org/img/w/{data.icon}.png";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var imageBytes = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pbImgH.Image = Image.FromStream(ms);
                        pbImgH.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                pbImgH.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // 
            // labelTempH
            // 
            labelTempH.AutoSize = true;
            labelTempH.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTempH.ForeColor = SystemColors.HighlightText;
            labelTempH.Location = new Point(73, 19);
            labelTempH.Name = "labelTempH";
            labelTempH.Size = new Size(80, 40);
            labelTempH.TabIndex = 1;
            labelTempH.Text = $"{(int)(Globalne.KelvinZero + data.temp)}°C";

            // 
            // labelTimeH
            // 
            labelTimeH.AutoSize = true;
            labelTimeH.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTimeH.ForeColor = SystemColors.HighlightText;
            labelTimeH.Location = new Point(563, 23);
            labelTimeH.Name = "labelTimeH";
            labelTimeH.Size = new Size(66, 32);
            labelTimeH.TabIndex = 3;
            labelTimeH.Text = $"{DateTimeOffset.FromUnixTimeSeconds(data.dt + tz).DateTime.ToString("HH:mm")}";
            labelTimeH.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pbWindH
            // 
            pbWindH.Location = new Point(168, 14);
            pbWindH.Name = "pbWindH";
            pbWindH.Size = new Size(50, 50);
            pbWindH.TabIndex = 4;
            pbWindH.TabStop = false;
            pbWindH.Tag = data.wind_deg;
            pbWindH.Paint += pbWindH_Paint;

            // 
            // labelWindSpeedH
            // 
            labelWindSpeedH.AutoSize = true;
            labelWindSpeedH.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelWindSpeedH.ForeColor = SystemColors.HighlightText;
            labelWindSpeedH.Location = new Point(234, 23);
            labelWindSpeedH.Name = "labelWindSpeedH";
            labelWindSpeedH.Size = new Size(75, 32);
            labelWindSpeedH.TabIndex = 5;
            labelWindSpeedH.Text = $"{(int)data.wind_speed} m/s";
            // 
            // labelMain
            // 
            labelMain.AutoSize = true;
            labelMain.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelMain.ForeColor = SystemColors.HighlightText;
            labelMain.Location = new Point(371, 23);
            labelMain.Name = "labelMain";
            labelMain.Size = new Size(70, 32);
            labelMain.TabIndex = 6;
            labelMain.Text = data.main_weather;
            labelMain.TextAlign = ContentAlignment.MiddleCenter;


            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.SteelBlue;
            mainPanel.Controls.Add(labelMain);
            mainPanel.Controls.Add(labelWindSpeedH);
            mainPanel.Controls.Add(pbWindH);
            mainPanel.Controls.Add(labelTimeH);
            mainPanel.Controls.Add(labelTempH);
            mainPanel.Controls.Add(pbImgH);
            mainPanel.Location = new Point(26, 20 + (80 * id));
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(632, 75);
            mainPanel.TabIndex = 1;
            tabHistory.Controls.Add(mainPanel);
        }

        private void pickedDate(object sender, EventArgs e)
        {
            tabHistory.Controls.Clear();
            refreshHistoryView();
        }

        private void saveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Ustawienie filtrów plików
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|JSON files (*.json)|*.json";
            saveFileDialog.Title = "Zapisz plik jako";

            // Pokazanie okna dialogowego
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string extension = Path.GetExtension(filePath).ToLower();

                // Przykładowe dane do zapisania
                string data = "";

                // Zapis danych w zależności od rozszerzenia
                switch (extension)
                {
                    case ".xml":
                        File.WriteAllText(filePath, "<dane>Nie zaimplementowane jeszcze</dane>");
                        break;
                    case ".json":
                        SerializeFactoryMethod.Save(filePath, FileFormat.JSON, histData);
                        break;
                    case ".csv":
                        SerializeFactoryMethod.Save(filePath, FileFormat.CSV, histData);
                        break;
                    default:
                        MessageBox.Show("Nieobsługiwany format pliku.");
                        break;
                }
            }
        }

        void IAlertObserver.Call(string sCity, string sMsg)
        {
            new ToastContentBuilder()
                            .AddText(sCity)
                            .AddText(sMsg)
                            .Show(); // Wyświetla powiadomienie
        }
    }
}
