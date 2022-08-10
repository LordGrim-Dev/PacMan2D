using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptableUtility
{
    
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    
    public static T CreateAssetAtPath<T>(string inPath, string inName) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = inPath;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New" + inName + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        return asset;
    }
    
}
