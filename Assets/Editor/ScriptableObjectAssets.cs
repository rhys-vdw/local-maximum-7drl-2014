using UnityEngine;
using UnityEditor;

public class ScriptableObjectAssets
{
    [MenuItem("Assets/Create/Sprite Sheet")]
    public static void CreateTileSetManagerAsset()
    {
        ScriptableObjectUtility.CreateAsset<SpriteSheet>();
    }
}
