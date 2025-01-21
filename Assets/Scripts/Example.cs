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

        if (!asset.HasError())
        {
            Debug.LogError(asset.error);
            return;
        }

        script.DoString(asset.luaCode);
    }
}
