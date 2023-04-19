using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    private static string sTestTarget = @"https://6x7nu-oaaaa-aaaan-qdaua-cai.ic0.app";

#if UNITY_ANDROID
    private AndroidJavaObject mPlugin = null;
#endif

    public void Start()
    {
#if UNITY_ANDROID
        var pluginClass = new AndroidJavaClass("com.mycompany.testurl.MyPlugin");
        mPlugin = pluginClass.CallStatic<AndroidJavaObject>("initImpl");
#endif
    }

    public void BrowserButtonClick()
    {
#if UNITY_ANDROID
        mPlugin.Call("openBrowser", sTestTarget);
#endif
    }

    public void OnApplicationPause(bool pause)
    {
        // if it's resuming.
        if (!pause)
        {
#if UNITY_ANDROID
            // OnApplicationPause will be called while launching the app, before mPlugin is initialized.
            if (mPlugin == null)
                return;

            mPlugin.Call("sendMessage");
#endif
        }
    }
}
