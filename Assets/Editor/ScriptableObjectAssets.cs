using UnityEngine;
using UnityEditor;

public class ScriptableObjectAssets
{
    [MenuItem("Assets/Create/Sprite Sheet")]
    public static void CreateSprieSheetAsset()
    {
        ScriptableObjectUtility.CreateAsset<SpriteSheet>();
    }

    [MenuItem("Assets/Create/Item Definition")]
    public static void CreateItemDefinitionAsset()
    {
        ScriptableObjectUtility.CreateAsset<ItemDefinition>();
    }
}
