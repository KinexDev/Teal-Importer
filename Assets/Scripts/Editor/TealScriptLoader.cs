using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using UnityEditor;

namespace Lua.Editor
{
    [ScriptedImporter(1, ".tl")]
    public class TealScriptLoader : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var _script = ScriptableObject.CreateInstance<TealScriptAsset>();
            _script.source = File.ReadAllText(ctx.assetPath);
            
            if (ctx.assetPath.Contains(".d"))
            {
                _script.definitionFile = true;
            }
            else
            {
                TealCompiler.CheckTypes(ctx.assetPath, _script);
            }
            
            ctx.AddObjectToAsset(AssetDatabase.AssetPathToGUID(ctx.assetPath), _script);
            ctx.SetMainObject(_script);
        }
    }
}