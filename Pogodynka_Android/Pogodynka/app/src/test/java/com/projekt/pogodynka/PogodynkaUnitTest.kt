package com.projekt.pogodynka

import com.google.android.gms.common.internal.Asserts
import com.projekt.pogodynka.model.WeatherData
import org.json.simple.parser.JSONParser
import org.json.simple.parser.ParseException
import org.junit.Assert.*
import org.junit.Test
import java.io.IOException
import java.time.LocalDate
import java.time.ZoneId
import java.time.format.DateTimeFormatter


/**
 * Example local unit test, which will execute on the development machine (host).
 *
 * See [testing documentation](http://d.android.com/tools/testing).
 */
class PogodynkaUnitTest {

    @Test
    fun strings() {
        val list =  mutableListOf("Kraków", "Warszawa", "Rzeszów")
        val city = "Warszawa"
        list.removeIf { it == city }
        assertTrue(list.size == 2 && !list.contains(city))
        val miasta = list.joinToString ("|")
        val l = miasta.split("|")
    }
    @Test
    fun addition_isCorrect() {
        assertEquals(4, 2 + 2)
    }

    /**
     * Test prawidłowego odczytu wiadomości
     */
    @Test
    fun dateSplit() {
        val msg = "Uwaga 2025-08-08 przewidywane są bardzo wysokie temperatury"
        val dateString = msg.split(" ")
        if(dateString.size < 2) return
        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")
        val d = LocalDate.parse(dateString[1], formatter)

        val millis1 = d.atStartOfDay(ZoneId.systemDefault()).toInstant().toEpochMilli()
        val localDate = LocalDate.now()
        val millis = localDate.atStartOfDay(ZoneId.systemDefault()).toInstant().toEpochMilli()
        println(d)

    }

    /**
     * Test parsera wiadomości JSON dla odczytu danych pogodowych
     */
    @Test
    fun weatherAPIParserTest() {
        val jsonStringWeather = "{\"coord\":{\"lon\":21.0118,\"lat\":52.2298},\"weather\":[{\"id\":801,\"main\":\"Clouds\",\"description\":\"few clouds\",\"icon\":\"02d\"}],\"base\":\"stations\",\"main\":{\"temp\":294.32,\"feels_like\":294.11,\"temp_min\":293.11,\"temp_max\":295.83,\"pressure\":1017,\"humidity\":62,\"sea_level\":1017,\"grnd_level\":1007},\"visibility\":10000,\"wind\":{\"speed\":1.54,\"deg\":0},\"clouds\":{\"all\":20},\"dt\":1757668850,\"sys\":{\"type\":2,\"id\":2032856,\"country\":\"PL\",\"sunrise\":1757649931,\"sunset\":1757696356},\"timezone\":7200,\"id\":756135,\"name\":\"Warsaw\",\"cod\":200}"

        val parser = JSONParser()
        val jsonObject = parser.parse(jsonStringWeather) as org.json.simple.JSONObject
        val pogoda: WeatherData? = try {
             WeatherData(jsonObject)
        } catch (e: IOException) {
            null
        } catch (e: ParseException) {
            null
        }
        Asserts.checkNotNull(pogoda)
        pogoda?.let{
            Asserts.checkState(pogoda.pressure ==  1017)
            Asserts.checkNotNull(pogoda.name == "Warsaw")
            Asserts.checkState(pogoda.mainWeather == "Clouds")
            Asserts.checkState(pogoda.description == "few clouds")
            Asserts.checkState(pogoda.icon == "02d")
        }
    }
}