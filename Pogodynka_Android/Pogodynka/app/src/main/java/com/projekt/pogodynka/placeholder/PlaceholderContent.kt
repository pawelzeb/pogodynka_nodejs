package com.projekt.pogodynka.placeholder

import com.projekt.pogodynka.Globalne
import com.projekt.pogodynka.model.WeatherData
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import org.json.simple.JSONObject
import java.util.ArrayList
import java.util.HashMap
import java.util.concurrent.CountDownLatch

/**
 * Helper class for providing sample content for user interfaces created by
 * Android template wizards.
 *
 * TODO: Replace all uses of this class before publishing your app.
 */
object PlaceholderContent {

    /**
     * An array of sample (placeholder) items.
     */
    val ITEMS: MutableList<WeatherData> = ArrayList()

    /**
     * A map of sample (placeholder) items, by ID.
     */
    val ITEM_MAP: MutableMap<String, WeatherData> = HashMap()

    fun init(city: String, cc: String) {
        ITEMS.clear()
        ITEM_MAP.clear()
        val latch = CountDownLatch(1)
        CoroutineScope(Dispatchers.IO).launch {
            val jsonArray = WeatherData.readJsonHistoryFromUrl("${Globalne.nodejs_url}/history/${city}/${cc}")
            jsonArray?.forEach { wd->
                if(wd is JSONObject) {
                    println(wd)
                    val mainWeather = wd["main_weather"].toString()
                    val icon = wd["icon"].toString()
                    val description = wd["description"].toString()
                    val temp = wd["temp"].toString().toFloat()
                    val temp_min = wd["temp_min"].toString().toFloat()
                    val temp_max =  wd["temp_max"].toString().toFloat()
                    val humidity = wd["humidity"].toString().toInt()
                    val pressure = wd["pressure"].toString().toInt()
                    val windDeg = wd["wind_deg"].toString().toInt()
                    val windSpeed = wd["wind_speed"].toString().toFloat()
                    val dt = wd["dt"].toString().toLong()
                    val sunRise = wd["sun_rise"].toString().toLong()
                    val sunSet = wd["sun_set"].toString().toLong()
                    val data = WeatherData(city,mainWeather,description,temp,temp_max,temp_min,icon,
                        pressure,dt,sunSet,sunRise,windDeg,humidity,windSpeed)
                    addItem(data)
                }
            }
            println(jsonArray)
            latch.countDown()
        }
        latch.await()
        println("Done!")
    }

    private fun addItem(item: WeatherData) {
        synchronized(PlaceholderContent) {
            ITEMS.add(item)
            ITEM_MAP.put(item.dt.toString(), item)
        }
    }
}