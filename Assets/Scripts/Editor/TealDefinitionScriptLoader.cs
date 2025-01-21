using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using UnityEditor;

namespace Lua.Editor
{
    [ScriptedImporter(1, ".d.tl")]
    public class TealDefinitionScriptLoader : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var _script = ScriptableObject.CreateInstance<TealDeclarationScriptAsset>();
            _script.source = File.ReadAllText(ctx.assetPath);

            ctx.AddObjectToAsset(AssetDatabase.AssetPathToGUID(ctx.assetPath), _script);
            ctx.SetMainObject(_script);
        }
    }
}