# ICP.NET Integration
An example about how to integrate [ICP.NET](https://github.com/edjCase/ICP.NET) into Unity.

## How to run
1. Install Unity 2021.3LTS;
2. Open the `UnityProject`;
3. Open the `Assets\Scenes\SampleScene.unity`;
4. Play in the Unity Editor.

## ICP.NET Integration
Here describes how the managed dlls under `UnityProject\Packages\com.ic.agent\ICP.NET` are generated.

- `EdjCase.ICP.Agent.dll` & `EdjCase.ICP.Candid.dll` are compiled from [ICP.NET](https://github.com/edjCase/ICP.NET) directly, they're targeted to .NET standard 2.0, so it’s okay to use them directly.
- For all the other dependencies, including  
    - Chaos.NaCl.dll (1.0.0)  
    - Dahomey.Cbor.dll (1.16.1)  
    - System.Collections.Immutable (6.0.0)  
    - System.Runtime.CompilerServices.Unsafe (6.0.0)  
    - System.IO.Pipelines (6.0.1)  
    - Microsoft.Bcl.HashCode.dll (1.1.1)  

  Download the packages with the correct version from https://www.nuget.org/packages and choose the dll with netstandard2.0 version.
