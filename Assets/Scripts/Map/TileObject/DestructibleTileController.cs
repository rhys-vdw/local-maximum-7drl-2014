using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class DestructibleTileController : ExtendedMonoBehaviour
{
    void Start()
    {
        Component<Tile>().TileType.Watch( HandleTypeChanged );
        Component<Health>().DeathEvent += HandleDeathEvent;
    }

    void HandleTypeChanged( TileType type )
    {
        if( type == TileType.Blocked )
        {
            Component<Collider>().enabled = true;
            var health = Component<Health>();
            health.Current = health.Max;
        }
        else
        {
            Component<Collider>().enabled = false;
        }
    }

    void HandleDeathEvent( Health health )
    {
        Destroy( gameObject );
    }
}
