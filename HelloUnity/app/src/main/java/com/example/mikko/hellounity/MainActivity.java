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
    public String positionDataString;

    @Override
    protected void onStart() {
        super.onStart();

        proximiio = ProximiioFactory.getProximiio(UnityPlayer.currentActivity, this);
        listener = new ProximiioListener() {
            @Override
            public void position(double lat, double lon, double accuracy) {
                // Do something with the positioning system.
                // See ProximiioListener in the docs for all available methods.
                positionDataString = lat + ";" + lon + ";" + accuracy;
            }
            @Override
            public void loginFailed(LoginError loginError) {
                positionDataString = loginError.toString();
                Log.e("OurJavaError", "LoginError! (" + loginError.toString() + ")");
            }


        };
        proximiio.addListener(listener);
        positionDataString = "Listener added";
        proximiio.setLogin("mikkojniiranen@gmail.com", "okkim123");
        positionDataString = "Login called";

    }

    public String GetPositionString()
    {
        if (positionDataString == null || positionDataString.trim() == "")
        {
            return "Position string empty";
        }
        return positionDataString;
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



