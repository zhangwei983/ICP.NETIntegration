using UnityEngine;

namespace IC.GameKit
{
    public class PluginProxy : MonoBehaviour
    {
        private static string sTestTarget = @"https://6x7nu-oaaaa-aaaan-qdaua-cai.ic0.app";

#if UNITY_ANDROID
        private AndroidJavaObject mPlugin = null;
#endif

        public void Start()
        {
#if UNITY_ANDROID
            var pluginClass = new AndroidJavaClass("com.icgamekit.plugin.ICGameKitPlugin");
            mPlugin = pluginClass.CallStatic<AndroidJavaObject>("initImpl");
#endif
        }

        public void OpenBrowser()
        {
#if UNITY_ANDROID
            mPlugin.Call("openBrowser", sTestTarget);
#endif
        }

        public void OnApplicationPause(bool pause)
        {
            // If it's resuming.
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
}
