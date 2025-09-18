using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka.Model
{
    /**
     *  Model danych Historycznych
     **/
    public class HistoryModel
    {
        public int id { get; set; }
        public int city_id { get; set; }
        public long dt { get; set; }
        public string icon { get; set; }
        public string main_weather { get; set; }
        public string description { get; set; }
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int humidity { get; set; }
        public int pressure { get; set; }
        public int cloud_cover { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public long sun_rise { get; set; }
        public long sun_set { get; set; }
    }
}
