using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public string ItemDefinitionPath = "Item Definitions";
    Dictionary<string, ItemDefinition> m_ItemDefinitions;

    void Awake()
    {
        m_ItemDefinitions = Resources
            .LoadAll( ItemDefinitionPath )
            .Select( r => r as ItemDefinition )
            .Where( i => i != null )
            .ToDictionary( i => i.name );

        Console.Logf( "Loaded {0} item definitions", m_ItemDefinitions.Count );
    }

    public bool TryGetDefinition( string name, out ItemDefinition item )
    {
        return m_ItemDefinitions.TryGetValue( name, out item );
    }

    public ItemDefinition GetDefinition( string name )
    {
        ItemDefinition item;
        if( ! TryGetDefinition( name, out item ) )
        {
            throw new System.InvalidOperationException( string.Format(
                "No ItemDefinition named {0} in directory */Resources/{1}.", name, ItemDefinitionPath
            ) );
        }
        return item;
    }
}