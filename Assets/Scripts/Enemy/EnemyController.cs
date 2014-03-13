using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;
using Pathfinding;
using UnityObjectRetrieval;

public class EnemyController : ExtendedMonoBehaviour
{
    public LayerMask PlayerMask = ~0;
    public float DetectionRadius = 5f;
    public float DetectionPeriod = 2f;

    Player m_TargetPlayer = null;
    List<Player> m_PlayersDetected = null;

    Transform m_Transform;
    Seeker m_Seeker;


    void Awake()
    {
        m_Transform = transform;
        m_Seeker = Component<Seeker>();
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
