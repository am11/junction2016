package com.example.mikko.hellounity;


import android.content.Intent;
import android.support.annotation.NonNull;
import android.util.Log;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import io.proximi.proximiiolibrary.Proximiio;
import io.proximi.proximiiolibrary.ProximiioFactory;
import io.proximi.proximiiolibrary.ProximiioListener;

public class MainActivity extends UnityPlayerActivity {

    private Proximiio proximiio;
    private ProximiioListener listener;
    public String MapDataString;

    @Override
    protected void onStart() {
        super.onStart();

        proximiio = ProximiioFactory.getProximiio(UnityPlayer.currentActivity, this);
        listener = new ProximiioListener() {
            @Override
            public void position(double latitude, double longitude, double accuracy) {
                // Do something with the positioning system.
                // See ProximiioListener in the docs for all available methods.
                Log.w("OurJavaDebug", "Position changed: Latitude = " + latitude + "Longitude = " + longitude + "Accuracy = " + accuracy);
                MapDataString = latitude + ";" + longitude + ";" + accuracy;
            }

            @Override
            public void loginFailed(LoginError loginError) {
                Log.e("OurJavaError", "LoginError! (" + loginError.toString() + ")");
            }

            @Override
            public void loggedIn(boolean online) {
                Log.w("OurJavaDebug", "Player login status: " + String.valueOf( online));
            }
        };
        proximiio.addListener(listener);
        proximiio.setLogin("mikkojniiranen@gmail.com", "okkim123");
    }

    public String GetPositionString()
    {
        return MapDataString;
    }
    @Override
    protected void onStop() {
        super.onStop();
        proximiio.removeListener(listener);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        proximiio.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        proximiio.onActivityResult(requestCode, resultCode, data);
    }
}



