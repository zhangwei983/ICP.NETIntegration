using UnityEngine;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Auth;
using EdjCase.ICP.Agent.Responses;
using EdjCase.ICP.Candid.Models;

public class TestCSharpAgent : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        System.Uri url = new System.Uri($"https://ic0.app");

        var identity = new AnonymousIdentity();

        IAgent agent = new HttpAgent(identity, url);

        Principal canisterId = Principal.FromText("rrkah-fqaaa-aaaaa-aaaaq-cai");

        string method = "get_proposal_info";

        CandidArg arg = CandidArg.FromCandid(CandidValueWithType.FromObject((ulong)94182));

        var responseTask = agent.QueryAsync(canisterId, method, arg);
        QueryResponse response = await responseTask;

        QueryReply reply = response.ThrowOrGetReply();

        if (reply != null)
        {
            Debug.Log(reply.Arg.Values[0].Value);
            Debug.Log(reply.Arg.Values[0].Type);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
