package com.projekt.pogodynka.utils

import android.content.Context
import android.preference.PreferenceManager.getDefaultSharedPreferences
import android.util.Log
import com.google.firebase.Firebase
import com.google.firebase.auth.auth
import com.google.firebase.database.DataSnapshot
import com.google.firebase.database.DatabaseError
import com.google.firebase.database.FirebaseDatabase
import com.google.firebase.database.Logger
import com.google.firebase.database.ValueEventListener
import com.projekt.pogodynka.Globalne
import java.time.LocalDate
import java.time.ZoneId
import java.time.format.DateTimeFormatter
import kotlin.reflect.KFunction2

/**
 * Singleton wspierający komunikację z bazą Firebase, w tym:
 *  - ustanawianie połączenia z bazą danych
 *  - tworzenie Listenerów na konkretnych lokacjach
 *  - usuwanie  Listenerów na konkretnych lokacjach
 *
 */
object FirebaseHelper {
    val listeners = mutableMapOf<String, ValueEventListener>()

    private fun logInAndRun(context: Context, callback: () -> Unit) {
        val auth = Firebase.auth
        auth.signOut()
        auth.signInWithEmailAndPassword(Globalne.email, Globalne.password)
            .addOnCompleteListener { task ->
                if (task.isSuccessful) {
                    auth.isSignInWithEmailLink(Globalne.email)
                    Log.d("TAG", "signInWithEmail:success")
                    callback()

                }
                else {
                    val exception = task.exception
                    Log.e("TAG", "Błąd logowania: ${exception?.message}")
                    // Możesz też pokazać komunikat użytkownikowi
                }
            }
    }

    /**
     * pobieramy alert RCB z Firebase
     */
    fun setAlertRCBListener(context: Context, callback: (title:String, msg:String) -> Unit) {
        FirebaseDatabase.getInstance().setLogLevel(Logger.Level.DEBUG)

        logInAndRun(context, { setListener("msg", callback) })
        val sharedPreferences = getDefaultSharedPreferences(context)
        var followCities:MutableList<String> =
            sharedPreferences.getString(Globalne.FOLLOW_CITIES, "")?.split("|")?.toMutableList()?: mutableListOf()
        followCities.forEach {
            cityCC ->
            if(cityCC.contains("_")) {
                val (city, cc) = cityCC.split("_")
                logInAndRun(context, { setListener(city, callback) })
            }
        }

    }

    private fun setListener(city:String, callback: (title: String, msg:String) -> Unit) {
        // Sign in success, update UI with the signed-in user's information

        val database = FirebaseDatabase.getInstance("https://pogodynka-e398e-default-rtdb.europe-west1.firebasedatabase.app")
        val myRef = database.getReference(city)

        val title = if(city == "msg") "🚨 Alert RCB" else "\uD83D\uDEA8 $city"

        val listener = object : ValueEventListener {
            override fun onDataChange(snapshot: DataSnapshot) {
                if(!snapshot.exists()) return
                Log.i("TAG", "czyta ${snapshot.key} == ${snapshot.value}")
                val msg = snapshot.value.toString()
                val dateString = msg.split(" ")
                if(dateString.size < 2) return
                val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd")
                val d = LocalDate.parse(dateString[1], formatter)

                val millis1 = d.atStartOfDay(ZoneId.systemDefault()).toInstant().toEpochMilli()
                val localDate = LocalDate.now()
                val millis = localDate.atStartOfDay(ZoneId.systemDefault()).toInstant().toEpochMilli()
                if(millis1 >= millis) {
                    callback(title, snapshot.value.toString())
                }
                else
                    return
            }

            override fun onCancelled(error: DatabaseError) {
                // Obsługa błędu
                Log.e("TAG", "bład ${error.message}")
            }
        }
        myRef.addValueEventListener(listener)

        listeners[city] = listener
    }
    fun removeListener(city:String) {
        if(listeners.containsKey(city)) {
            val database = FirebaseDatabase.getInstance("https://pogodynka-e398e-default-rtdb.europe-west1.firebasedatabase.app")
            val ref = database.getReference(city)
            ref.removeEventListener(listeners[city]!!)
            listeners.remove(city)
        }
    }
    fun signOut() {
        val auth = Firebase.auth
        Log.d("FAP", "SignOut...")
        auth.signOut()
    }

    fun follow(context: Context, city: String, callback: KFunction2<String, String, Unit>) {
        logInAndRun(context, { setListener(city, callback) })
    }

}