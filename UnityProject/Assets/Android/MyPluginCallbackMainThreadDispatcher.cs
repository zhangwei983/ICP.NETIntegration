using System.Collections.Generic;
using System.IO;
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

        foreach (var identityPath in m_ReceivedCallbacksList)
        {
            OnCallbackReceived(identityPath);
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

    void OnCallbackReceived(string identityPath)
    {
        if (string.IsNullOrEmpty(identityPath) || !File.Exists(identityPath))
            return;

        Debug.Log("Identity path '" + identityPath + "' exists: " + File.Exists(identityPath));

        var go = GameObject.Find("Callback URL");
        var text = go?.GetComponent<Text>();
        if (text != null)
            text.text = identityPath;

        var identityJson = File.ReadAllText(identityPath);
        Debug.Log("VincentIdentity is: " + identityJson.Length);
    }
}
