package com.projekt.pogodynka.adapters

import android.content.ClipData
import android.content.ClipDescription
import android.content.Context
import android.os.Build
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.navigation.findNavController
import androidx.recyclerview.widget.RecyclerView
import com.projekt.pogodynka.R
import com.projekt.pogodynka.activities.HistoryDetailFragment
import com.projekt.pogodynka.databinding.HistoryListContentBinding
import com.projekt.pogodynka.model.ForecastData
import com.projekt.pogodynka.model.WeatherData
import com.squareup.picasso.Callback
import com.squareup.picasso.OkHttp3Downloader
import com.squareup.picasso.Picasso
import okhttp3.OkHttpClient
import java.text.DateFormat
import java.text.SimpleDateFormat
import java.util.Date
import java.util.concurrent.TimeUnit

class HistoryAdapter(
    private val context: Context,
    private val values: List<WeatherData>,
    private val itemDetailFragmentContainer: View?
) :
    RecyclerView.Adapter<HistoryAdapter.ViewHolder>() {


    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {

        val binding = HistoryListContentBinding.inflate(
            LayoutInflater.from(parent.context),
            parent,
            false
        )
        return ViewHolder(binding)

    }
    fun getTime(dt: Date):String {
        val df: DateFormat = SimpleDateFormat("HH:mm")
        return df.format(dt)
    }
    fun getDate(dt:Date):String {
        val df: DateFormat = SimpleDateFormat("dd.MM.yyyy")
        return df.format(dt)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val item = values[position]
        val prevItem = if(position == 0 ) item else values[position - 1]
        holder.time.text = getTime(item.dt)
        holder.temp.text = "${item.temp.toInt()}°C"
        //holder.temp.text = item.temp.toString()
        var bDayChannged = false;
        val fd = ForecastData(item.dt, listOf())
        val day = fd.getDayOfWeek()
        val prevDay = ForecastData(prevItem.dt, listOf()).getDayOfWeek()
        if(position == 0 || day != prevDay) {
            bDayChannged = true
            holder.day.text = "$day ${getDate(item.dt)}"
            holder.layout.visibility = View.INVISIBLE
            holder.day.visibility = View.VISIBLE
        }
        else {
            bDayChannged = false
            holder.layout.visibility = View.VISIBLE
            holder.day.visibility = View.INVISIBLE
            val okHttpClient = OkHttpClient.Builder()
                .connectTimeout(10, TimeUnit.SECONDS) // Czas oczekiwania na połączenie
                .readTimeout(60, TimeUnit.SECONDS) // Czas oczekiwania na odpowiedź
                .writeTimeout(60, TimeUnit.SECONDS) // Czas oczekiwania na zapis
                .build()

            val url = "https://openweathermap.org/img/w/${item.icon}.png"
            val picasso = Picasso.Builder(context)
                .downloader(OkHttp3Downloader(okHttpClient))
                .build()

            picasso.load(url)
                .into(holder.img, object : Callback {
                    override fun onSuccess() {
                        Log.d("TAG", "Wczytał plik graficzny")
                    }
                    override fun onError(e: Exception?) {
                        Log.e("TAG", "Nie wczytał pliku graficznego ${e?.message}")
                    }
                })
        }

        if(!bDayChannged)
            with(holder.itemView) {
                tag = item
                setOnClickListener { itemView ->
                    val item = itemView.tag as WeatherData
                    val bundle = Bundle()
                    bundle.putString(
                        HistoryDetailFragment.ARG_ITEM_ID,
                        item.dt.toString()
                    )
                    if (itemDetailFragmentContainer != null) {
                        itemDetailFragmentContainer.findNavController()
                            .navigate(R.id.fragment_history_detail, bundle)
                    } else {
                        itemView.findNavController().navigate(R.id.show_history_detail, bundle)
                    }
                }
                /**
                 * Context click listener to handle Right click events
                 * from mice and trackpad input to provide a more native
                 * experience on larger screen devices
                 */
                setOnContextClickListener { v ->
                    val item = v.tag as WeatherData
                    Toast.makeText(
                        v.context,
                        "Context click of item " + item.dt.toString(),
                        Toast.LENGTH_LONG
                    ).show()
                    true
                }
            }
    }

    override fun getItemCount() = values.size

    inner class ViewHolder(binding: HistoryListContentBinding) :
        RecyclerView.ViewHolder(binding.root) {
        val temp: TextView = binding.temp
        val time: TextView = binding.time
        val day: TextView = binding.day
        val layout: View = binding.layout
        val img: ImageView = binding.img
    }

}