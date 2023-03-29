using System.Collections.Generic;
using System.IO;
using System.Web;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that queues and and processes the received MyPluginCallbacks.
/// </summary>
public class MyPluginCallbackMainThreadDispatcher : MonoBehaviour
{
    private static MyPluginCallbackMainThreadDispatcher instance = null;

    private List<string> m_ReceivedCallbacksQueue = new List<string>();

    private List<string> m_ReceivedCallbacksList = new List<string>();

    internal void EnqueueReceivedCallback(string callback)
    {
        lock (this)
        {
            m_ReceivedCallbacksQueue.Add(callback);
        }
    }

    internal static MyPluginCallbackMainThreadDispatcher GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        lock (this)
        {
            if (m_ReceivedCallbacksQueue.Count == 0)
                return;

            var temp = m_ReceivedCallbacksQueue;
            m_ReceivedCallbacksQueue = m_ReceivedCallbacksList;
            m_ReceivedCallbacksList = temp;
        }

        foreach (var paramsPath in m_ReceivedCallbacksList)
        {
            OnCallbackReceived(paramsPath);
        }

        m_ReceivedCallbacksList.Clear();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    void OnCallbackReceived(string paramsPath)
    {
        if (string.IsNullOrEmpty(paramsPath) || !File.Exists(paramsPath))
            return;

        Debug.Log("Params path '" + paramsPath + "' exists: " + File.Exists(paramsPath));

        var go = GameObject.Find("Callback URL");
        var text = go?.GetComponent<Text>();
        if (text != null)
            text.text = paramsPath;

        var parameters = File.ReadAllText(paramsPath);
        Debug.Log("Params length is: " + parameters.Length);

        const string kIdentityParam = "identity=";
        const string kDelegationParam = "&delegation=";
        var indexOfIdentity = parameters.IndexOf(kIdentityParam);
        if (indexOfIdentity == -1)
        {
            Debug.LogError("Cannot find identity");
            return;
        }
        var indexOfDelegation = parameters.IndexOf(kDelegationParam);
        if(indexOfDelegation == -1)
        {
            Debug.LogError("Cannot find delegation");
            return;
        }

        var identityLength = indexOfDelegation - indexOfIdentity - kIdentityParam.Length;
        var identity = HttpUtility.UrlDecode(parameters.Substring(indexOfIdentity + kIdentityParam.Length, identityLength));
        var delegation = HttpUtility.UrlDecode(parameters.Substring(indexOfDelegation + kDelegationParam.Length));

        Debug.Log("Identity length is: " + identity.Length);
        Debug.Log(identity);
        Debug.Log("Delegation length is: " + delegation.Length);
        Debug.Log(delegation);
    }
}
