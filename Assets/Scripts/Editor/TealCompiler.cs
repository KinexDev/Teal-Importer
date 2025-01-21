using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TealCompiler : AssetPostprocessor
{
    public static string global_environment
    {
        get
        {
            var completeDir = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "Assets/clr_methods.d.tl";
            return completeDir;
        }
    }
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var asset in importedAssets)
        {
            if (asset.EndsWith(".tl"))
            {
                CheckTypes(asset);
            }
        }
    }

    public static void CheckTypes(string asset)
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
                Thread.Sleep(500);
            }

            EditorUtility.ClearProgressBar();

            if (process.ExitCode != 0)
            {
                var obj = AssetDatabase.LoadAssetAtPath<TealScriptAsset>(asset);
                
                var error = process.StandardError.ReadToEnd();
                Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
                Debug.LogError($"Checked script '{asset}' \n{error}");
                obj.error = error;
                AssetDatabase.SaveAssets();
                
                Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
                
            }
            else
            {
                Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
                Debug.Log($"Checked script '{asset}' \n{process.StandardOutput.ReadToEnd()}");
                Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
                CheckSyntax(asset);
            }
        }
    }

    public static void CheckSyntax(string asset)
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
                Thread.Sleep(500);
            }

            var obj = AssetDatabase.LoadAssetAtPath<TealScriptAsset>(asset);
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

            AssetDatabase.SaveAssets();
        }
    }
}