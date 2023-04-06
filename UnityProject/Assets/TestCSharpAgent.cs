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

    public static async void SendMessage(string[] identity, DelegationChainModel delegationChainModel)
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

        Debug.Log("Greeting result is: " + content);

        var go = GameObject.Find("My Princinpal");
        var text = go?.GetComponent<Text>();
        if (text != null)
            text.text = content;
    }
}
