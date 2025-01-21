using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class TealScriptAsset : ScriptableObject
{
    #if UNITY_EDITOR
    public bool definitionFile;
    #endif
    public string source;
    public string luaCode;
    public string error;

    public bool HasError()
    {
        return !string.IsNullOrEmpty(error);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(TealScriptAsset))]
public class TealScriptAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (TealScriptAsset)target;
        
        EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(script.source);
        
        if (script.definitionFile) return;
        
        if (!string.IsNullOrEmpty(script.error))
        {
            EditorGUILayout.Space(12f);

            EditorGUILayout.HelpBox(script.error, MessageType.Error);
        }
        else
        {
            EditorGUILayout.LabelField("Compiled", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(script.luaCode);
        }
    }
}
#endif