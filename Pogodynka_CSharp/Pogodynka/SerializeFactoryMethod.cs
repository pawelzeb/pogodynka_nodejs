using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pogodynka.Model;
using static System.Windows.Forms.Design.AxImporter;

namespace Pogodynka
{
    public enum FileFormat
    {
        CSV,
        JSON,
        XML
    }

    /**
     * Metoda wytwórcza słóżąca do zapisu danych do określonego formatu
     **/
    internal static class SerializeFactoryMethod
    {
        public static bool Save(string fileName, FileFormat ff, List<HistoryModel>? data)
        {
            switch (ff)
            {
                case FileFormat.CSV:
                    {
                        saveCSV(fileName, data);
                        return true;
                    }
                case FileFormat.JSON:
                    {
                        saveJSON(fileName, data);
                        return true;
                    }
                default:
                    break;
            }
            return false;
        }

        private static void saveJSON(string fileName, List<HistoryModel>? histData)
        {
            if (histData != null)
            {
                // Opcjonalne ustawienia (np. formatowanie JSON)
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // czytelny format z wcięciami
                };

                string jsonString = JsonSerializer.Serialize(histData, options);
                File.WriteAllText(fileName, jsonString);
            }
        }

        private static void saveCSV(string fileName, List<HistoryModel>? histData)
        {
            if (histData != null)
            {
                var cnt = 0;
                string prevDay = "";
                StringBuilder sb = new StringBuilder();
                var weekDays = new string[] { "Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota" };
                DateTime date = new DateTime();
                sb.AppendLine("dzień_tygodnia;data;czas;opis1;opis2;temperatura;temp_max;temp_min;wilgotność;");
                foreach (var data in histData)
                {
                    var dat = DateTimeOffset.FromUnixTimeSeconds(data.dt).DateTime;
                    if (dat.Date == date.Date) continue;
                    var sDay = weekDays[(uint)dat.DayOfWeek];
                    sb.AppendLine($"{sDay};{dat.ToString("dd/MM/yyyy")};{dat.TimeOfDay};{data.main_weather};{data.description};{Globalne.KelvinZero + data.temp};{Globalne.KelvinZero + data.temp_max};{Globalne.KelvinZero + data.temp_min};{data.humidity};");
                    cnt++;
                }
                if (cnt == 0)
                {
                    sb = new StringBuilder();
                    sb.AppendLine("Brak wpisów");
                }
                File.WriteAllText(fileName, sb.ToString());
            }
        }
    }
}
