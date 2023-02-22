using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public string testTarget = @"C:\test.html";

    public void BrowserButtonClick()
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(testTarget) { UseShellExecute = true });
    }

    public void RegisterButtonClick()
    {
        // TODO.
        Debug.Log("Register Protocol handler");
    }
}
