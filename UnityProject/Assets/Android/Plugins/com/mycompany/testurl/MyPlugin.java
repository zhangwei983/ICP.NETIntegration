package com.mycompany.testurl;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

// Implement in C# to call from java.
interface MyPluginCallback {
    void onSendMessage(String url);
}

public class MyPlugin {
    static final String TAG_PLUGIN = "MyPlugin";

    public static MyPlugin currentPlugin;

    private MyPluginCallback mMyPluginCallback;

    public static MyPlugin initImpl(MyPluginCallback pluginCallback) {
        if (currentPlugin != null)
            return currentPlugin;

        currentPlugin = new MyPlugin();
        currentPlugin.init(pluginCallback);

        return currentPlugin;
    }

    public void init(MyPluginCallback pluginCallback) {
        mMyPluginCallback = pluginCallback;
    }

    public void openBrowser(String url) {
        Log.i(TAG_PLUGIN, url);
        
        //String url = "https://6x7nu-oaaaa-aaaan-qdaua-cai.ic0.app";
        Uri uri = Uri.parse(url);
        Intent intent = new Intent(Intent.ACTION_VIEW, uri);
        UnityPlayer.currentActivity.startActivity(intent);
    }

    public void sendMessage() {
        Uri uri = UnityPlayer.currentActivity.getIntent().getData();
        if (uri != null && mMyPluginCallback != null)
        {
            String url = uri.toString();
            Log.i("MyPlugin", url);
            mMyPluginCallback.onSendMessage(url);
        }
    }
}
