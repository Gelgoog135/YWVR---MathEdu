using UnityEngine;

using System.IO;

[UnityEditor.AssetImporters.ScriptedImporter(1, "tex")]
public class TEXFileImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        var text = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("text", text);
        ctx.SetMainObject(text);
    }
}