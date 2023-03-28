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
    static final String sParamName = "identity=";

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

        String url = Uri.decode(uri.toString());
        Log.i(TAG_PLUGIN, url);

        int index = url.indexOf(sParamName);
        if (index == -1)
            return;

        String identity = url.substring(index + sParamName.length());
        Log.i(TAG_PLUGIN, identity);

        // Write to a temporary file to internal storage and read it back from C# side.
        // The reason is we can only pass 1024 bytes as string back to the C# side, but the identity string is more than 3k bytes.
        String identityPath = UnityPlayer.currentActivity.getFilesDir().getPath() + "/identity.json";
        File identityFile = new File(identityPath);
        try {
            if (identityFile.exists())
                identityFile.delete();

            FileOutputStream fileOutputStream = new FileOutputStream(identityFile);
            fileOutputStream.write(identity.getBytes());
            fileOutputStream.flush();
            fileOutputStream.close();
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        // Pass the identity path back to C#.
        mMyPluginCallback.onSendMessage(identityPath);
    }
}
