public class Delegation
{
    public string expiration;
    public string pubkey;
}

public class SignedDelegation
{
    public Delegation delegation;
    public string signature;
}

public class DelegationChain
{
    public SignedDelegation[] delegations;
    public string publicKey;
}
