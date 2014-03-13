using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public float DetectionRadius = 5f;
    public float DetectionPeriod = 2f;

    Player m_TargetPlayer = null;
    List<Player> m_PlayersDetected = null;

    Transform m_Transform;
    LayerMask PlayerMask = ~0;

    void Awake()
    {
        m_Transform = transform;
    }

    void Start()
    {
        InvokeRepeating( "DetectPlayers", Random.value * DetectionPeriod, DetectionPeriod );
    }

    void DetectPlayers()
    {
        var playerColliders = Physics.OverlapSphere( m_Transform.position, DetectionRadius, PlayerMask );
        m_PlayersDetected = playerColliders
            .Select( c => c.Component<Player>() )
            .ToList();
    }
}
