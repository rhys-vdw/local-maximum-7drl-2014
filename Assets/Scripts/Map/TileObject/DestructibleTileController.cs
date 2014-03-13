using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class DestructibleTileController : ExtendedMonoBehaviour
{
    void Start()
    {
        Component<Tile>().Type.Watch( HandleTypeChanged );
        Component<Health>().DeathEvent += HandleDeathEvent;
    }

    void HandleTypeChanged( TileType type )
    {
        var health = Component<Health>();
        if( type == TileType.Blocked )
        {
            health.enabled = true;
            health.Current = health.Max;
        }
        else
        {
            health.enabled = false;
        }
    }

    void HandleDeathEvent( Health health )
    {
        var tile = Component<Tile>();
        Scene.Object<MapBuilder>().SetTileType( tile.X, tile.Y, TileType.Destroyed );
    }
}
