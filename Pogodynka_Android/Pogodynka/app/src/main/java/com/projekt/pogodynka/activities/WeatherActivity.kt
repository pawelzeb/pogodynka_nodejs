package com.projekt.pogodynka.activities

import android.Manifest
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.content.ServiceConnection
import android.content.pm.PackageManager
import android.content.res.ColorStateList
import android.os.Build
import android.os.Bundle
import android.os.IBinder
import android.preference.PreferenceManager.getDefaultSharedPreferences
import android.util.Log
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.google.android.material.snackbar.Snackbar
import com.projekt.pogodynka.Globalne
import com.projekt.pogodynka.R
import com.projekt.pogodynka.databinding.ActivityMainBinding
import com.projekt.pogodynka.model.ForecastPlaceholder
import com.projekt.pogodynka.model.WeatherData
import com.projekt.pogodynka.services.AlertService
import com.projekt.pogodynka.utils.Utils
import com.squareup.picasso.Callback
import com.squareup.picasso.OkHttp3Downloader
import com.squareup.picasso.Picasso
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import okhttp3.Call
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import java.io.IOException
import java.math.BigDecimal
import java.math.RoundingMode
import java.net.SocketTimeoutException
import java.util.concurrent.TimeUnit

class WeatherActivity : AppCompatActivity() {
    private lateinit var serviceIntent: Intent
    private lateinit var binding: ActivityMainBinding


    private var alertService: AlertService? = null

    /**
     * Pobiera w osobnym wątku dane i sprawdza czy istnieje połaczenie z internetem/serwerem
     */
    private fun getDataForLocation(context: Context, city: String, cc:String) {
        CoroutineScope(Dispatchers.IO).launch {
            if(!Utils.isInternetAvailable(context))
            {
                withContext(Dispatchers.Main) {
                    binding.mainLayout.visibility = View.INVISIBLE
                    binding.noConn.visibility = View.VISIBLE
                }
                return@launch
            }
        val pogoda: WeatherData? = try {
            WeatherData(
                "${Globalne.nodejs_url}/weather/${city}/${cc}")
//                "https://api.openweathermap.org/data/2.5/weather?q=${city},${cc}&appid=1f7cff01e11e11b2533f9497da9ee055")
        } catch (e: SocketTimeoutException) {
            withContext(Dispatchers.Main) {
                binding.btnFollow.visibility = View.INVISIBLE
                //binding.mainLayout.visibility = View
                //Toast.makeText(context, "${e.message}", Toast.LENGTH_LONG ).show()
                val rootView: View = findViewById(android.R.id.content)
                // Wyświetl Snackbar z komunikatem błędu
                Snackbar.make(rootView, "Brak połączenia z serwerem.\nSprawdź czy serwer Azure jest podłączony.", Snackbar.LENGTH_LONG).show()
            }
            null
        }

        pogoda?.let{
            withContext(Dispatchers.Main) {
                binding.mainLayout.visibility = View.VISIBLE
                binding.noConn.visibility = View.INVISIBLE
            }
            ForecastPlaceholder.init(city, cc)
            Log.d("TAG", "Zapisuje do sharedPreferences")
            val sharedPreferences = getDefaultSharedPreferences(applicationContext)
            val editor = sharedPreferences.edit()
            editor.putString(Globalne.CITY, city)
            editor.putString(Globalne.CC, cc)
            editor.apply()

            with(pogoda) {
                withContext(Dispatchers.Main) {

                    val cityName = "${name}_${cc}"
                    if(isSubscribing(cityName)) {
                        binding.btnFollow.backgroundTintList = ColorStateList.valueOf(ContextCompat.getColor(applicationContext, R.color.red))
                        binding.btnFollow.setText("-Obserwuj")
                    }
                    else {
                        binding.btnFollow.setText("+Obserwuj")
                        binding.btnFollow.backgroundTintList = ColorStateList.valueOf(ContextCompat.getColor(applicationContext, R.color.green))
                    }


                    val editor = sharedPreferences.edit()
                    editor.putString(Globalne.CITY, city).apply()
                    editor.putString(Globalne.CC, cc).apply()

                    binding.btnFollow.visibility = View.VISIBLE
                    binding.mainLayout.visibility = View.VISIBLE
                    binding.description.text = description
                    binding.temp.text = "${round(temp,0)}°C"
                    binding.tempMax.text = "Max: ${round(temp_max,0)}°C"
                    binding.tempMin.text = "Min: ${round(temp_min,0)}°C"
                    binding.lat.text = "${round(lat)}°"
                    binding.lon.text = "${round(lon)}°"
                    binding.name.text = name
                    binding.name.text = name
                    binding.windSpeed.text = "${round(windSpeed)} m/s"
                    binding.pressure.text = "${pressure}mBar"
                    binding.sunRise.text = "${getSunRiseTime()}"
                    binding.sunSet.text = "${getSunSetTime()}"
                    binding.localTime.text = "(${getTime()})"
                    binding.windBearing.rotation = windDeg.toFloat()

                    val url = "https://openweathermap.org/img/w/${icon}.png"
                    val okHttpClient = OkHttpClient.Builder()
                        .connectTimeout(10, TimeUnit.SECONDS) // Czas oczekiwania na połączenie
                        .readTimeout(60, TimeUnit.SECONDS) // Czas oczekiwania na odpowiedź
                        .writeTimeout(60, TimeUnit.SECONDS) // Czas oczekiwania na zapis
                        .build()

                    val picasso = Picasso.Builder(applicationContext)
                        .downloader(OkHttp3Downloader(okHttpClient))
                        .build()

                    picasso.load(url)
                        .into(binding.weatherImage, object : Callback {
                            override fun onSuccess() {
                                Log.d("TAG", "Wczytał plik graficzny")
                            }
                            override fun onError(e: Exception?) {
                                Log.e("TAG", "Nie wczytał pliku graficznego ${e?.message}")
                            }
                        })
                    val dayLengthMillis = sunSet.time - sunRise.time
                    val dayLengthSeconds = dayLengthMillis / 1000 // Konwersja na sekundy
                    val dayLengthMinutes = dayLengthSeconds / 60 // Konwersja na minuty
                    val dayLengthHours = dayLengthMinutes / 60 // Konwersja na godziny
//                    val h = dayLengthHours
//                    val m =  (dayLengthSeconds - h * 3600)/60
//                    val s = (dayLengthSeconds - h * 3600 - m * 60)

                    println("Długość dnia: $dayLengthHours godzin i ${dayLengthMinutes % 60} minut")


                    if(dt.time < sunRise.time || dt.time > sunSet.time) {
                        binding.sunBearing.visibility = View.INVISIBLE
                        return@withContext
                    }
                    val now = dt.time - sunRise.time
                    binding.sunBearing.visibility = View.VISIBLE
                    val angle = -90f + now.toFloat()/dayLengthMillis.toFloat() * 180f
                    binding.sunBearing.rotation = angle
                }
            }
        }
    }

}
    fun followRequest(city: String, cc: String) {
        val client = OkHttpClient()

        val request = Request.Builder()
            .url("${Globalne.nodejs_url}/follow/${city}/${cc}")
            .put(ByteArray(0).toRequestBody(null, 0, 0))
            .build()
        client.newCall(request).enqueue(object: okhttp3.Callback{
            override fun onResponse(call: Call, response: Response) {
                follow(city, cc, true)
            }
            override fun onFailure(call: Call, e: IOException) {
                // obsługa błędu
            }
        })
    }
    fun unfollowRequest(city: String, cc: String) {
        val client = OkHttpClient()

        val request = Request.Builder()
            .url("${Globalne.nodejs_url}/follow/${city}/${cc}")
            .delete()
            .build()
        client.newCall(request).enqueue(object: okhttp3.Callback{
            override fun onResponse(call: Call, response: Response) {
                follow(city, cc, false)
            }
            override fun onFailure(call: Call, e: IOException) {
                // obsługa błędu
            }
        })
    }

    fun round(number: Float, scale:Int = 2) = BigDecimal(number.toDouble()).setScale(scale, RoundingMode.HALF_UP).toFloat()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, 0)
            insets
        }

        val sharedPreferences = getDefaultSharedPreferences(applicationContext)
        val city:String = sharedPreferences.getString(Globalne.CITY, "Warszawa")?:"Warszawa"
        val cc:String = sharedPreferences.getString(Globalne.CC, "pl")?:"pl"
        binding.location.setText(city)
        binding.cc.setText(cc)

        binding.btnFollow.setOnClickListener {
            val cc:String = binding.cc.text.trim().toString();
            //val city1:String = binding.location.text.trim().toString();
            val city:String = binding.name.text.trim().toString()   //do listenera bierzemy nazwę z danych a nie wpisaną przez użytkownika!

//            if(city != city1) return@setOnClickListener
            if(cc.length != 2) return@setOnClickListener
            if(city.isEmpty()) return@setOnClickListener

            val sharedPreferences = getDefaultSharedPreferences(applicationContext)
            var followCities:MutableList<String> =
                sharedPreferences.getString(Globalne.FOLLOW_CITIES, "")?.split("|")?.toMutableList()?: mutableListOf()

            val cityCC = "${city}_${binding.cc.text}"

            if(followCities.contains(cityCC)) {
                unfollowRequest(city, cc)
                followCities.removeIf { it == cityCC }
                binding.btnFollow.text = "+Obserwuj"
                binding.btnFollow.backgroundTintList = ColorStateList.valueOf(ContextCompat.getColor(applicationContext, R.color.green))

            }
            else {
                followCities.add(cityCC)
                followRequest(city, cc)
                binding.btnFollow.backgroundTintList = ColorStateList.valueOf(ContextCompat.getColor(applicationContext, R.color.red))
                binding.btnFollow.setText("-Obserwuj")
            }


            val editor = sharedPreferences.edit()
            editor.putString(Globalne.FOLLOW_CITIES, followCities.joinToString("|")).apply()



        }
        binding.btnGo.setOnClickListener {
            val city = binding.location.text.toString().trim()
            val cc = binding.cc.text.toString().trim()
            if(city.isEmpty() || cc.isEmpty()) return@setOnClickListener
            getDataForLocation(this, city,cc)
        }
        menuState(binding)
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU)
        {
            requestPermissions(arrayOf(Manifest.permission.POST_NOTIFICATIONS), 100)
        }
        else {
            startService()
        }
//        binding.bottomNavigation.selectedItemId = R.id.weather

    }

    private fun isSubscribing(city:String): Boolean {
        val sharedPreferences = getDefaultSharedPreferences(applicationContext)
        var followCities: MutableList<String> =
            sharedPreferences.getString(Globalne.FOLLOW_CITIES, "")?.split("|")?.toMutableList()
                ?: mutableListOf()


        return followCities.contains(city)
    }

    private fun startService() {
        serviceIntent = Intent(this, AlertService::class.java)
        ContextCompat.startForegroundService(this, serviceIntent)
    }

    private fun follow(city: String, cc:String, bFollow:Boolean = true) {
        if(!::serviceIntent.isInitialized) return
        bindService(serviceIntent, object : ServiceConnection {
            override fun onServiceConnected(name: ComponentName?, service: IBinder?) {
                val binder = service as AlertService.LocalBinder
                val alertService = binder.getService()
                when(bFollow) {
                    true -> alertService.follow(applicationContext, city,cc)
                    false -> alertService.unfollow(city,cc)
                }

                unbindService(this)
            }

            override fun onServiceDisconnected(name: ComponentName?) {
                alertService = null
            }
        }, Context.BIND_AUTO_CREATE)
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == 100 && grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
            startService() // ← Twoja funkcja do startu serwisu
        } else {
            Toast.makeText(this, "Brak zgody na powiadomienia", Toast.LENGTH_SHORT).show()
        }
    }

    private fun menuState(binding: ActivityMainBinding) {
        binding.bottomNavigation.selectedItemId = R.id.weather
        binding.bottomNavigation.setOnItemSelectedListener {
            val intent = when (it.itemId) {
                R.id.forecast -> Intent(applicationContext, ForecastActivity::class.java)
                R.id.history -> Intent(applicationContext, HistoryDetailHostActivity::class.java)
                else -> null
            }
            if (intent != null) {
                startActivity(intent)
                overridePendingTransition(0, 0)
            }
            true
        }
    }
    override fun onResume() {
        super.onResume()
        //binding.btnGo.performClick()
    }
}