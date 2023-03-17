using UnityEngine;
#if PLATFORM_STANDALONE_WIN
using Microsoft.Win32;
#endif

public class ButtonClick : MonoBehaviour
{
    public void Start()
    {
#if PLATFORM_STANDALONE_OSX || UNITY_EDITOR_OSX
        var button_register = GameObject.Find("Button_Register");
        if (button_register != null)
        {
            button_register.SetActive(false);
        }
#endif
    }

    //public string testTarget = @"C:\test.html";
    private string testTarget = @"https://6x7nu-oaaaa-aaaan-qdaua-cai.ic0.app";
    private string myRootKey = "vincenttest";
    public string applicationPath; // = @"C:\Projects\vs\TestBrowser\TestBrowser\bin\Debug\net6.0-windows\TestBrowser.exe";

    public void BrowserButtonClick()
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(testTarget) { UseShellExecute = true });
    }

    public void RegisterButtonClick()
    {
#if PLATFORM_STANDALONE_WIN
        // This is just used to demo how we register a protocol handler.
        // You should rely on the installer to register in the registry in real case.

        var regCurrentUser = Registry.CurrentUser;
        var regClassesRoot = regCurrentUser.OpenSubKey("SOFTWARE", true)?.OpenSubKey("Classes", true);
        if (regClassesRoot == null)
            return;

        var regMyRoot = regClassesRoot.OpenSubKey(myRootKey, true);
        if (regMyRoot != null)
        {
            regClassesRoot.DeleteSubKeyTree(myRootKey);
        }

        var newRegMyRoot = regClassesRoot.CreateSubKey(myRootKey, true);
        newRegMyRoot.SetValue("", "URL:" + myRootKey);
        newRegMyRoot.SetValue("URL Protocol", "");

        var regIcon = newRegMyRoot.CreateSubKey("DefaultIcon");
        regIcon.SetValue("", "OpenBrowser.exe");

        var regCommand = newRegMyRoot.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
        regCommand.SetValue("", "\"" + applicationPath + "\" \"%1\"");

        return;
#endif
    }
}
