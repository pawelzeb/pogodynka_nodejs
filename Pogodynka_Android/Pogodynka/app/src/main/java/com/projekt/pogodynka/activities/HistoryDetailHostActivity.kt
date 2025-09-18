package com.projekt.pogodynka.activities

import android.content.Intent
import android.os.Bundle
import android.preference.PreferenceManager.getDefaultSharedPreferences
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import com.projekt.pogodynka.Globalne
import com.projekt.pogodynka.R
import com.projekt.pogodynka.databinding.ActivityForecastBinding
import com.projekt.pogodynka.databinding.ActivityHistoryDetailBinding


class HistoryDetailHostActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val binding = ActivityHistoryDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)
        val sharedPreferences = getDefaultSharedPreferences(applicationContext)
        val city = sharedPreferences.getString(Globalne.CITY, "")
        supportActionBar?.title = city
        binding.cityName.text = city
        menuState(binding)
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_history_detail)
        return navController.navigateUp(appBarConfiguration)
                || super.onSupportNavigateUp()
    }

    private fun menuState(binding: ActivityHistoryDetailBinding) {
        binding.bottomNavigation.selectedItemId = R.id.history
        binding.bottomNavigation.setOnItemSelectedListener {
            val intent = when (it.itemId) {
                R.id.weather -> Intent(applicationContext, WeatherActivity::class.java)
                R.id.forecast -> Intent(applicationContext, ForecastActivity::class.java)
                else -> null
            }
            if (intent != null) {
                startActivity(intent)
                overridePendingTransition(0, 0)
            }
            true
        }

    }
}