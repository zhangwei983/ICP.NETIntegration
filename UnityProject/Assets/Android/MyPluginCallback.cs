using System;
using UnityEngine;

public class MyPluginCallback : AndroidJavaProxy
{
    public MyPluginCallback() : base("com.mycompany.testurl.MyPluginCallback")
    {
    }

    public void onSendMessage(String url)
    {
        Debug.Log(url);

        MyPluginCallbackMainThreadDispatcher.GetInstance().EnqueueReceivedCallback(url);
    }
}
