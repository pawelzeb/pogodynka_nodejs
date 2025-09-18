using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pogodynka;
using Pogodynka.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pogodynka.Tests
{
    /**
     * Testy sprawdzające połączenie z serwerem nodejs
     **/
    [TestClass()]
    public class PogodynkaConnectionTests
    {
        [TestMethod()]
        public void parseWeatherData()
        {
            string city = "Warszawa";
            string cc = "pl";

            String baseurl = $"{Globalne.nodejs_url}/weather/{city}/{cc}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "curl/7.37.0";
            request.ContentType = "application/json";


            var response = request.GetResponse();
            Assert.IsNotNull(response);

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                var dataModel = JsonSerializer.Deserialize<WeatherModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Assert.IsNotNull(dataModel);
                Assert.AreEqual(dataModel.Name, "Warsaw");
                //dataModel.Coord.Lat;
                //dataModel.Coord.Lon;
                Assert.AreEqual(dataModel.Coord.Lat, 52.2298, 0.001);
                Assert.AreEqual(dataModel.Coord.Lon, 21.0118, 0.001);
                Assert.IsTrue(dataModel.Main.Temp_Max >= dataModel.Main.Temp_Min);
            }

        }
        [TestMethod()]
        public void parseForecastData()
        {
            string city = "Warszawa";
            string cc = "pl";

            String baseurl = $"{Globalne.nodejs_url}/forecast/{city}/{cc}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "curl/7.37.0";
            request.ContentType = "application/json";


            var response = request.GetResponse();
            Assert.IsNotNull(response);

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                var dataModel = JsonSerializer.Deserialize<ForecastModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Assert.IsNotNull(dataModel);
                Assert.AreEqual(dataModel.cod, "200");
                Assert.IsTrue(dataModel.list.Count > 0);
                foreach (var forecast in dataModel.list)
                {
                    Assert.IsTrue(forecast.main.temp_max >= forecast.main.temp_min);
                }
            }

        }

        [TestMethod()]
        public void parseHistoryData()
        {
            string city = "Warszawa";
            string cc = "pl";

            String baseurl = $"{Globalne.nodejs_url}/history/{city}/{cc}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseurl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "curl/7.37.0";
            request.ContentType = "application/json";


            var response = request.GetResponse();
            if (response == null)
            {
                //testujemy tylko gdy mamy dane historyczne na danej lokacji
                Assert.IsTrue(true);
                return;
            }
            Assert.IsNotNull(response);
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                var dataModel = JsonSerializer.Deserialize<List<HistoryModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Assert.IsNotNull(dataModel);
                Assert.IsTrue(dataModel.Count > 0);
                foreach (var hist in dataModel)
                {
                    Assert.IsTrue(hist.temp_max >= hist.temp_min);
                }

            }

        }
    }
}