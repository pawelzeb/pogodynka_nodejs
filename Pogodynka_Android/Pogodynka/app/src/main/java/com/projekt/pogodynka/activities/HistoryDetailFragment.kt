package com.projekt.pogodynka.activities

import android.content.ClipData
import android.os.Bundle
import android.util.Log
import android.view.DragEvent
import androidx.fragment.app.Fragment
import com.google.android.material.appbar.CollapsingToolbarLayout
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import com.projekt.pogodynka.R
import com.projekt.pogodynka.placeholder.PlaceholderContent
import com.projekt.pogodynka.databinding.FragmentHistoryDetailBinding
import com.projekt.pogodynka.model.WeatherData
import com.squareup.picasso.Callback
import com.squareup.picasso.OkHttp3Downloader
import com.squareup.picasso.Picasso
import okhttp3.OkHttpClient
import java.math.BigDecimal
import java.math.RoundingMode
import java.util.concurrent.TimeUnit

/**
 * A fragment representing a single History detail screen.
 * This fragment is either contained in a [HistoryListFragment]
 * in two-pane mode (on larger screen devices) or self-contained
 * on handsets.
 */
class HistoryDetailFragment : Fragment() {

    /**
     * The placeholder content this fragment is presenting.
     */
    private var item: WeatherData? = null

    lateinit var itemDetailTextView: TextView
    private var toolbarLayout: CollapsingToolbarLayout? = null

    private var _binding: FragmentHistoryDetailBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    private val dragListener = View.OnDragListener { v, event ->
        if (event.action == DragEvent.ACTION_DROP) {
            val clipDataItem: ClipData.Item = event.clipData.getItemAt(0)
            val dragData = clipDataItem.text
            item = PlaceholderContent.ITEM_MAP[dragData]
            updateContent()
        }
        true
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        arguments?.let {
            if (it.containsKey(ARG_ITEM_ID)) {
                // Load the placeholder content specified by the fragment
                // arguments. In a real-world scenario, use a Loader
                // to load content from a content provider.
                item = PlaceholderContent.ITEM_MAP[it.getString(ARG_ITEM_ID)]
            }
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        _binding = FragmentHistoryDetailBinding.inflate(inflater, container, false)
        val rootView = binding.root

        toolbarLayout = binding.toolbarLayout


        updateContent()
        rootView.setOnDragListener(dragListener)

        return rootView
    }
    fun round(number: Float, scale:Int = 2) = BigDecimal(number.toDouble()).setScale(scale, RoundingMode.HALF_UP).toFloat()

    private fun updateContent() {

        // Show the placeholder content as text in a TextView.
        item?.let {
            with(it) {
                toolbarLayout?.title = "$name (${getDate()})"
                binding.description.text = description
                binding.temp.text = "${round(temp, 0)}°C"
                binding.tempMax.text = "Max: ${round(temp_max, 0)}°C"
                binding.tempMin.text = "Min: ${round(temp_min, 0)}°C"
                binding.windSpeed.text = "${round(windSpeed)} m/s"
                binding.pressure.text = "${pressure}mBar"
                binding.windBearing.rotation = windDeg.toFloat()

                val okHttpClient = OkHttpClient.Builder()
                    .connectTimeout(10, TimeUnit.SECONDS) // Czas oczekiwania na połączenie
                    .readTimeout(60, TimeUnit.SECONDS) // Czas oczekiwania na odpowiedź
                    .writeTimeout(60, TimeUnit.SECONDS) // Czas oczekiwania na zapis
                    .build()

                val url = "https://openweathermap.org/img/w/${icon}.png"
                val picasso = Picasso.Builder(requireContext())
                    .downloader(OkHttp3Downloader(okHttpClient))
                    .build()

                picasso.load(url)
                    .into(binding.img, object : Callback {
                        override fun onSuccess() {
                            Log.d("TAG", "Wczytał plik graficzny")
                        }
                        override fun onError(e: Exception?) {
                            Log.e("TAG", "Nie wczytał pliku graficznego ${e?.message}")
                        }
                    })
            }
        }
    }

    companion object {
        /**
         * The fragment argument representing the item ID that this fragment
         * represents.
         */
        const val ARG_ITEM_ID = "item_id"
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}