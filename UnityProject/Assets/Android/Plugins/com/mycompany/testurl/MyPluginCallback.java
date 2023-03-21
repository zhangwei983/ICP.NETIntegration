package com.mycompany.testurl;

import android.util.Log;

// Implemented in C# to receive callback.
public interface MyPluginCallback {
    void onSendMessage(int number);
}
