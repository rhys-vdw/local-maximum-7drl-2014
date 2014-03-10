using UnityEngine;
using UnityObjectRetrieval;

public class ItemFactory : MonoBehaviour
{
    ItemManager m_ItemManager;

    void Awake()
    {
        m_ItemManager = Scene.Object<ItemManager>();
    }

    public IItem Build( string name )
    {
        var definition = m_ItemManager.GetDefinition( name );
        var item = (Instantiate( definition.ItemPrefab ) as GameObject).Component<IItem>();
        Debug.Log( "ITEM", (item as MonoBehaviour) );
        return item;
    }
}