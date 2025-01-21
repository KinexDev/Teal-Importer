using System.IO;
using UnityEditor;

public static class TealScriptCreator
{
    private static readonly string defaultCode = "print(\"Hello World!\");";
    
    [MenuItem("Assets/Create/Teal Script")]
    public static void CreateLuaScript()
    {
        var folderPath = string.Empty;
        if (Selection.activeObject != null && AssetDatabase.Contains(Selection.activeObject))
        {
            folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!AssetDatabase.IsValidFolder (folderPath)) folderPath = Path.GetDirectoryName (folderPath);
        }

        var path = EditorUtility.SaveFilePanel("New Teal Script", folderPath, "tealScript", "tl");

        if (string.IsNullOrEmpty(path))
            return;

        File.WriteAllText(path, defaultCode);
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
    }
}