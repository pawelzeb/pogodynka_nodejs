using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka.Model
{
    public class ForecastModel
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List<WeatherEntry> list { get; set; }
    }

    public class WeatherEntry
    {
        public long dt { get; set; }
        public MainData main { get; set; }
        public List<WeatherInfo> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public int visibility { get; set; }
        public double pop { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
    }

    public class MainData
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }

    public class WeatherInfo
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }




}
