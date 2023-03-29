using UnityEngine;

public class MyPluginCallback : AndroidJavaProxy
{
    public MyPluginCallback() : base("com.mycompany.testurl.MyPluginCallback")
    {
    }

    public void onSendMessage(string paramsPath)
    {
        MyPluginCallbackMainThreadDispatcher.GetInstance().EnqueueReceivedCallback(paramsPath);
    }
}
