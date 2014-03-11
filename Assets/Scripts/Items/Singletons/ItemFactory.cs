using UnityEngine;
using UnityObjectRetrieval;

public class ItemFactory : MonoBehaviour
{
    ItemManager m_ItemManager;

    void Awake()
    {
        m_ItemManager = Scene.Object<ItemManager>();
    }

    public Item Build( string name )
    {
        var definition = m_ItemManager.GetDefinition( name );
        return Instantiate( definition.ItemPrefab ) as Item;
    }
}