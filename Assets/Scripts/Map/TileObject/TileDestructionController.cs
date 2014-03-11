using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class TileDestructionController : ExtendedMonoBehaviour
{
    void Start()
    {
        Component<Health>().DeathEvent += HandleDeathEvent;
    }

    void HandleDeathEvent( Health health )
    {
        Destroy( gameObject );
    }
}
