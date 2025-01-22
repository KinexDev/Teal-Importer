# Teal-Importer
Teal (a statically typed superset of lua) importer for unity.

![image](https://github.com/user-attachments/assets/fdad1077-d39e-4969-bdc5-79636f640ca0)

# Requirements
Teal must be installed and added to you environment variables.
The download instructions are on the github https://github.com/teal-language/tl

# Limitations
This currently only supports windows, but you could easily be extended to work on other operating systems.

# Usage
When you import/create a new `tl` file, it will automatically be converted to a `TealScriptAsset`, this contains the teal code along with the transpiled code and the errors in the script.

# How It Works
It opens up the command prompt and then tells the `tl` compiler to type check first, it then transpiles to lua, if any issues are found an error will be thrown in the console and the script will be tagged with the error.

![image](https://github.com/user-attachments/assets/9278b320-c94b-4970-941e-69e7da9c917c)

# Declaration files
Declaration files '.d.tl' are recognised by the `tl` compiler, they have to be explicitly defined inside `tlconfig.json` (the tl config has to be stored in assets, the declaration file can be moved but you need to specify the path in tlconfig)

Example of `clr_methods.d.tl`
```lua
global printError:function(message:string)
global test:number
```
# Execution
The transpiled code is executed by moonsharp (a lua interpreter in written C#), but it can be executed by any kind of lua interpreter.

```cs
using System;
using UnityEngine;
using MoonSharp;
using MoonSharp.Interpreter;

public class Example : MonoBehaviour
{
    public TealScriptAsset asset;
    public void Start()
    {
        var script = new Script();

        script.Globals["print"] = (Action<object>)print;
        
        //custom stuff defined in the declaration file
        script.Globals["printError"] = (Action<string>)Debug.LogError;
        script.Globals["test"] = 10;

        if (asset.HasError())
        {
            Debug.LogError(asset.error);
            return;
        }

        script.DoString(asset.luaCode);
    }
}

```

# Result

![image](https://github.com/user-attachments/assets/01822d65-c019-4914-ae1a-5c98b4356ae0)

![image](https://github.com/user-attachments/assets/154ef0ae-f974-458c-98f3-4f3e2bfec102)
