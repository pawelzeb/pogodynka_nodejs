package com.projekt.pogodynka

import com.google.android.gms.common.internal.Asserts
import com.projekt.pogodynka.model.ForecastData
import com.projekt.pogodynka.model.ForecastFactoryMethod
import com.projekt.pogodynka.model.ForecastPlaceholder
import com.projekt.pogodynka.model.WeatherData
import org.json.simple.parser.ParseException
import org.junit.Test
import java.io.IOException
import java.util.Date


class ConnectionTest {

    /**
     * Test parsera wiadomości JSON dla prognozy
     */
    @Test
    fun forecastAPIParserTest() {
        val city="Warszawa"
        val cc = "pl"
        val map = ForecastFactoryMethod.getForecastData(city, cc)
        val now = Date().time
        for(forecast in map) {
            ForecastPlaceholder.addData(ForecastData(forecast.value[0].dt, forecast.value))
            Asserts.checkNotNull(forecast)
            for(pogoda in forecast.value) {
                Asserts.checkState(pogoda.pressure > 800)
                Asserts.checkNotNull(pogoda.name)
                Asserts.checkState(pogoda.name.isNotEmpty())
                Asserts.checkState(pogoda.dt.time > 0 && pogoda.dt.time > now)
                Asserts.checkState(pogoda.icon.isNotEmpty())
                Asserts.checkState(pogoda.temp_max >= pogoda.temp_min)
            }
        }
    }

    /**
     * Test parsera wiadomości JSON dla odczytu danych pogodowych
     */
    @Test
    fun weatherAPIParserTest() {
        val city="Warszawa"
        val cc = "pl"
        val pogoda: WeatherData? = try {
            WeatherData(
                "${Globalne.nodejs_url}/weather/${city}/${cc}")
        } catch (e: IOException) {
            null
        } catch (e: ParseException) {
            null
        }
        Asserts.checkNotNull(pogoda)
        pogoda?.let{
            Asserts.checkState(pogoda.pressure > 800)
            Asserts.checkNotNull(pogoda.name)
            Asserts.checkState(pogoda.name.isNotEmpty())
            Asserts.checkState(pogoda.dt.time > 0 && pogoda.dt.time < Date().time)
            Asserts.checkState(pogoda.icon.isNotEmpty())
            Asserts.checkState(pogoda.temp_max >= pogoda.temp_min)
        }
    }
}