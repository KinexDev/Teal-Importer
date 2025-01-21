using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class TealCompiler
{
    public static void CheckTypes(string asset, TealScriptAsset obj)
    {
        var completeDir = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + asset;
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/C " + $"tl check \"{completeDir}\"")
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = Application.dataPath
        };

        var process = Process.Start(psi);

        EditorUtility.DisplayProgressBar("Script Compilation", "Checking tl types.", 0f);

        if (process != null)
        {
            while (!process.HasExited)
            {
                EditorUtility.DisplayProgressBar("Script Compilation", "Checking tl types.", 0f);
                Thread.Sleep(10);
            }

            EditorUtility.ClearProgressBar();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
                Debug.LogError($"Checked script '{asset}' \n{error}");
                Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
                obj.error = error;
                
            }
            else
            {
                Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
                Debug.Log($"Checked script '{asset}' \n{process.StandardOutput.ReadToEnd()}");
                Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
                CheckSyntax(asset, obj);
            }
        }
    }

    public static void CheckSyntax(string asset, TealScriptAsset obj)
    {
        var completeDir = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + asset;
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/C " + $"tl gen \"{completeDir}\"")
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = Application.dataPath
        };

        var process = Process.Start(psi);

        EditorUtility.DisplayProgressBar("Script Compilation", "Compiling teal script.", 0f);

        if (process != null)
        {
            while (!process.HasExited)
            {
                EditorUtility.DisplayProgressBar("Script Compilation", "Compiling teal script.", 0f);
                Thread.Sleep(10);
            }

            var luaFileDir = completeDir.Substring(0, completeDir.Length - 2) + "lua";


            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
                Debug.LogError($"Errors found in script '{asset}', \n{error}");
                Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
                obj.error = error;
            }
            else
            {
                obj.luaCode = File.ReadAllText(luaFileDir);
                File.Delete(luaFileDir);
            }
        }
    }
}