using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka
{
    /**
     * Klasa singletona obsługująca alerty,
     * klasa dodatkowo używa wzorca obserwera do wysyłania do niego powiadomień
     * obserwer to implementacja interfejsu funkcyjnego zaimplementowana w klasie Form
     * która obsługuje nadchodzące z Firebase powiadomienia
     **/
    internal class AlertService
    {
        System.Timers.Timer timer = new System.Timers.Timer(10000);
        static AlertService instance = null;
        private IAlertObserver observer;

        private AlertService(IAlertObserver observer) {
            this.observer = observer;
        }
        /**
         * Pobiertamy jednainstancję Alertów i
         * Uruchamiamy JEDNORAZOWO wątek śledzący powiadomienia z Firebase
        **/

        public static AlertService getInstance(IAlertObserver observer)
        {
            if (instance == null)
            {
                instance = new AlertService(observer);
                instance.initFB();
            }
            else
                instance.observer = observer;  
            return instance;
        }

        /**
         * Zatrzymujemy wątek śledzący powiadomieniaz Firebase
         **/
        public void Stop()
        {
            timer.Stop();
            timer.Close();
        }

        async private void initFB()
        {
            Dictionary<string, string> map = new Dictionary<string, string>();

            timer.Elapsed += async (s, e) =>
            {
                var firebase = new FirebaseClient("https://pogodynka-e398e-default-rtdb.europe-west1.firebasedatabase.app/");

                //var msgGloabal = await firebase.Child("msg").OnceAsJsonAsync();
                //msgGloabal = msgGloabal.Trim('"');
                //if (msgGloabal != null || msgGloabal != "null")
                //{
                //    observer.Call("Globalny Alert", msgGloabal);
                //};


                var followCities = Properties.Settings.Default.follow_cities;
                var cities = followCities.Split("|");
                foreach (var citycc in cities)
                {
                    //if (citycc.IsNullOrEmpty()) continue;
                    var city = citycc.Split("_");
                    var msg = await firebase.Child(city[0]).OnceAsJsonAsync();
                    msg = msg.Trim('"');
                    if (msg == null || msg == "null") continue;
                    if (map.ContainsKey(city[0]) && map[city[0]] == msg)
                        continue;
                    else
                    {
                        try
                        {
                            var date = DateTime.Parse(msg.Split(" ")[1]);
                            DateTime now = DateTime.Today;
                            map[city[0]] = msg;
                            if (date < now) continue;
                            observer.Call(city[0], msg);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.ToString());
                        }

                    }

                }

                //MessageBox.Show("Aktualna wartość: " + msg.Trim('"'));
            };
            timer.Start();

            //var dinos = firebase
            //  .Child("tmp")
            //  .AsObservable<Dane>()
            //  .Subscribe(d => {
            //        MessageBox.Show("Zmieniono dane: ");
            //        Console.WriteLine(d.Key);
            //     }
            //  );
            //.OnceAsync<String>();


            //Console.WriteLine($"{dinos}");
        }
    }
}
