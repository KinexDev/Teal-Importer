using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class TealScriptAsset : ScriptableObject
{
    public string source;
    public string luaCode;
    public string error;
}

#if UNITY_EDITOR

[CustomEditor(typeof(TealScriptAsset))]
public class TealScriptAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (TealScriptAsset)target;
        
        EditorGUILayout.LabelField("Teal", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(script.source);

        if (!string.IsNullOrEmpty(script.error))
        {
            EditorGUILayout.Space(12f);

            EditorGUILayout.HelpBox(script.error, MessageType.Error);
        }
        else
        {
            EditorGUILayout.LabelField("Lua", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(script.luaCode);
        }
    }
}
#endif