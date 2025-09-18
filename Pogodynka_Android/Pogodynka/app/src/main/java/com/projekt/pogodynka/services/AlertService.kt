package com.projekt.pogodynka.services

import android.app.Notification
import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.Service
import android.content.Context
import android.content.Intent
import android.os.Binder
import android.os.Build
import android.os.IBinder
import android.util.Log
import androidx.core.app.NotificationCompat
import com.projekt.pogodynka.R
import com.projekt.pogodynka.utils.FirebaseHelper
import java.util.Random

class AlertService : Service() {

    override fun onCreate() {
        super.onCreate()
        startForeground(NOTIFICATION_ID, createNotification())

        // Tu rejestrujemy listenera Firebase
        FirebaseHelper.setAlertRCBListener(applicationContext, ::notifyRCB)
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        return START_STICKY
    }

    private val TAG: String = AlertService::class.simpleName.toString()
    private val binder = LocalBinder()

    inner class LocalBinder : Binder() {
        fun getService(): AlertService = this@AlertService
    }

    override fun onBind(intent: Intent?): IBinder {
        return binder
    }

    private fun createNotification(): Notification {
        val channelId = "alert_channel_id"
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                channelId,
                "Alert Notifications",
                NotificationManager.IMPORTANCE_LOW
            )
            getSystemService(NotificationManager::class.java).createNotificationChannel(channel)
        }

        return NotificationCompat.Builder(this, channelId)
            .setContentTitle("Alert Service")
            .setContentText("Nasłuchiwanie alertów pogodowych")
            .setSmallIcon(R.drawable.sun_icon) // Upewnij się, że ikona istnieje
            .build()
    }

    private fun notifyRCB(title:String, msg:String) {
        val notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

        val channelId = "rcb_notification_channel"
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                channelId,
                "RCB Alerts",
                NotificationManager.IMPORTANCE_HIGH
            )
            notificationManager.createNotificationChannel(channel)
        }

        val notification = NotificationCompat.Builder(this, channelId)
            .setContentTitle(title)
//            .setContentText("Otrzymano nowy komunikat!")
            .setContentText(msg)
            .setSmallIcon(R.drawable.sun) // Zamień na swoją ikonę
            .setPriority(NotificationCompat.PRIORITY_HIGH)
            .build()

        notificationManager.notify(Random().nextInt(), notification)
    }
    fun follow(context:Context, city: String, cc: String) {
        Log.d(TAG, "FOLLOW ${city}")

        FirebaseHelper.follow(context, city, ::notifyRCB)

    }
    fun unfollow(city: String, cc: String) {
        Log.d(TAG, "UNFOLLOW ${city}")
        FirebaseHelper.removeListener(city)

    }

    companion object {
        const val NOTIFICATION_ID = 1
    }
}
