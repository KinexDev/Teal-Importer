using System;
using UnityEngine;
using MoonSharp;
using MoonSharp.Interpreter;

public class Example : MonoBehaviour
{
    public void Start()
    {
        var script = new Script();

        script.Globals["printError"] = (Action<string>)Debug.LogError;
        script.Globals["test"] = 10;
    }
}
