using UnityEngine;

public class MyPluginCallback : AndroidJavaProxy
{
    public MyPluginCallback() : base("com.mycompany.testurl.MyPluginCallback")
    {
    }

    public void onSendMessage(int number)
    {
        Debug.Log("Value from callback is: " + number);
    }
}
