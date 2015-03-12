package com.nateyb.spintervals;

import android.app.Activity;
import android.media.MediaMetadataRetriever;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.SeekBar;
import android.widget.TextView;

import java.text.DecimalFormat;


public class MainActivity extends Activity implements SeekBar.OnSeekBarChangeListener {
    MediaPlayer player;
    TextView tv_fullLength;
    TextView tv_currentProgress;
    SeekBar sb_songProgress;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        player = MediaPlayer.create(this, R.raw.spectrum);

        tv_fullLength = (TextView)findViewById(R.id.tv_fullLength);
        tv_currentProgress = (TextView)findViewById(R.id.tv_fullLength);
        sb_songProgress = (SeekBar)findViewById(R.id.sb_songProgress);

        MediaPlayer.TrackInfo[] trackinfos = player.getTrackInfo();
        MediaMetadataRetriever retriever = new MediaMetadataRetriever();
        int length = 0;
        int min = length / 60;
        int sec = length % 60;
        DecimalFormat format = new DecimalFormat("");
        String songDuration = String.format("%d:%d", min, sec);
        tv_fullLength.setText(songDuration);
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {

    }

    public void onStartTrackingTouch(SeekBar seekBar) {

    }

    public void onStopTrackingTouch(SeekBar seekBar) {

    }

    public void operation_click(View v) {

    }
}
