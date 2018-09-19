package com.example.lucky.unityandroid;

import android.os.Vibrator;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

public class MainActivity extends UnityPlayerActivity {

    private Vibrator vibrator;

    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        vibrator = (Vibrator)this.getSystemService(this.VIBRATOR_SERVICE);
        super.onCreate(savedInstanceState);
    }

    public void Start()
    {
        vibrator.vibrate(new long[]{1000, 2000},0);
    }

    public void Stop()
    {
        vibrator.cancel();
    }

    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if(keyCode==KeyEvent.KEYCODE_BACK && event.getRepeatCount() == 0){
            Log.i("lucky","is pressed ");
            UnityPlayer.UnitySendMessage("GameSystem","onBack","abc");
            mUnityPlayer.quit();
        }
        return false;
    }

}
