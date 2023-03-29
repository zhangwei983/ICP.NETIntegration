package com.mycompany.testurl;

import android.content.Intent;
import android.net.Uri;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.io.File;
import java.io.FileOutputStream;

// Implement in C# to call from java.
interface MyPluginCallback {
    void onSendMessage(String url);
}

public class MyPlugin {
    static final String TAG_PLUGIN = "MyPlugin";

    public static MyPlugin sCurrentPlugin;

    private MyPluginCallback mMyPluginCallback;

    public static MyPlugin initImpl(MyPluginCallback pluginCallback) {
        if (sCurrentPlugin != null)
            return sCurrentPlugin;

        sCurrentPlugin = new MyPlugin();
        sCurrentPlugin.init(pluginCallback);

        return sCurrentPlugin;
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
        if (mMyPluginCallback == null)
            return;

        Uri uri = UnityPlayer.currentActivity.getIntent().getData();
        if (uri == null)
            return;

        String url = uri.toString();
        int index = url.indexOf("identity=");
        if (index == -1)
            return;

        String params = url.substring(index);
        Log.i(TAG_PLUGIN, params);

        // Write to a temporary file to internal storage and read it back from C# side.
        // The reason is we can only pass 1024 bytes as string back to the C# side, but the params string with identity&delegation is more than 3k bytes.
        String paramsPath = UnityPlayer.currentActivity.getFilesDir().getPath() + "/params.file";
        File paramsFile = new File(paramsPath);
        try {
            if (paramsFile.exists())
                paramsFile.delete();

            FileOutputStream fileOutputStream = new FileOutputStream(paramsFile);
            fileOutputStream.write(params.getBytes());
            fileOutputStream.flush();
            fileOutputStream.close();
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        // Pass the params path back to C#.
        mMyPluginCallback.onSendMessage(paramsPath);
    }
}
