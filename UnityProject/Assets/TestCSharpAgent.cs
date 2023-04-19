using UnityEngine;
using UnityEngine.UI;
using EdjCase.ICP.Agent;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid.Utilities;
using System.Collections.Generic;
using EdjCase.ICP.Agent.Models;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Web;

public class TestCSharpAgent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMessageSent(string identityPath)
    {
        if (string.IsNullOrEmpty(identityPath) || !File.Exists(identityPath))
            return;

        Debug.Log("Identity path '" + identityPath + "' exists.");

        var parameters = File.ReadAllText(identityPath);
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
        if (indexOfDelegation == -1)
        {
            Debug.LogError("Cannot find delegation");
            return;
        }

        var identityLength = indexOfDelegation - indexOfIdentity - kIdentityParam.Length;
        var identityString = HttpUtility.UrlDecode(parameters.Substring(indexOfIdentity + kIdentityParam.Length, identityLength));
        var delegationString = HttpUtility.UrlDecode(parameters.Substring(indexOfDelegation + kDelegationParam.Length));

        var identity = JsonConvert.DeserializeObject<string[]>(identityString);
        var delegation = JsonConvert.DeserializeObject<DelegationChainModel>(delegationString);
        TestCSharpAgent.CallCanister(identity, delegation);
    }

    public static async void CallCanister(string[] identity, DelegationChainModel delegationChainModel)
    {
        Debug.Assert(identity != null && identity.Length == 2);
        Debug.Assert(delegationChainModel != null && delegationChainModel.delegations.Length >= 1);

        // Initialize Ed25519Identity. 
        var publicKey = DerEncodedPublicKey.FromEd25519(ByteUtil.FromHexString(identity[0]));
        var privateKey = ByteUtil.FromHexString(identity[1]);
        var innerIdentity = new Ed25519Identity(publicKey, privateKey);

        // Initialize DelegationIdentity.
        var chainPublicKey = DerEncodedPublicKey.FromDer(ByteUtil.FromHexString(delegationChainModel.publicKey));
        var delegations = new List<SignedDelegation>();
        foreach (var signedDelegationModel in delegationChainModel.delegations)
        {
            var pubKey = DerEncodedPublicKey.FromDer(ByteUtil.FromHexString(signedDelegationModel.delegation.pubkey));
            var expiration = ICTimestamp.FromNanoSeconds(Convert.ToUInt64(signedDelegationModel.delegation.expiration, 16));
            var delegation = new Delegation(pubKey.Value, expiration);

            var signature = ByteUtil.FromHexString(signedDelegationModel.signature);
            var signedDelegation = new SignedDelegation(delegation, signature);
            delegations.Add(signedDelegation);
        }
        var delegationChain = new DelegationChain(chainPublicKey, delegations);
        var delegationIdentity = new DelegationIdentity(innerIdentity, delegationChain);

        // Initialize HttpAgent.
        var agent = new HttpAgent(delegationIdentity);

        Principal canisterId = Principal.FromText("72rj2-biaaa-aaaan-qdatq-cai");

        // Intialize Client and make the call.
        var client = new GreetingClient.GreetingClient(agent, canisterId);
        var content = await client.Greet();

        var go = GameObject.Find("My Princinpal");
        var text = go?.GetComponent<Text>();
        if (text != null)
            text.text = content;
    }
}
