using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class TealDeclarationScriptAsset : ScriptableObject
{
    public string source;
}

#if UNITY_EDITOR

[CustomEditor(typeof(TealDeclarationScriptAsset))]
public class TealDeclarationScriptAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (TealDeclarationScriptAsset)target;
        EditorGUILayout.TextArea(script.source);
    }
}
#endif