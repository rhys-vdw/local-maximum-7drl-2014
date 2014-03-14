using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;
using UnityObjectRetrieval;

public class FollowPlayer : ExtendedMonoBehaviour
{
    public LayerMask PlayerMask = ~0;
    public float DetectionRadius = 5f;
    public float DetectionPeriod = 2f;
    public float RetrackPeriod = 2f;

    Transform m_Target = null;
    Health m_TargetHealth = null;

    Transform m_Transform;
    PathFollower m_PathFollower;

    void Awake()
    {
        m_Transform = transform;
        m_PathFollower = Component<PathFollower>();
        m_PathFollower.ReachedEndOfPathEvent += HandleReachedEndOfPath;
    }

    void Start()
    {
        InvokeRepeating( "DetectPlayers", Random.value * DetectionPeriod, DetectionPeriod );
        InvokeRepeating( "RetrackPath", Random.value * RetrackPeriod, RetrackPeriod );
    }

    void OnDisable()
    {
        ReleaseTarget();
    }

    void HandleReachedEndOfPath( PathFollower pathFollower )
    {
        RetrackPath();
    }

    void RetrackPath()
    {
        m_PathFollower.RetrackPath();
    }

    void DetectPlayers()
    {
        if( m_Target == null )
        {
            var playerColliders = Physics.OverlapSphere( m_Transform.position, DetectionRadius, PlayerMask );
            m_Target = playerColliders.Select( c => c.transform ).ClosestOrNull( m_Transform.position );
            if( m_Target != null )
            {
                m_PathFollower.Target = m_Target;
                m_TargetHealth = m_Target.SelfAncestors().Component<Health>();
                m_TargetHealth.DeathEvent += HandleTargetDeath;
            }
        }
    }

    void HandleTargetDeath( Health health )
    {
        ReleaseTarget();
    }

    void ReleaseTarget()
    {
        m_Target = null;
        if( m_TargetHealth != null )
        {
            m_TargetHealth.DeathEvent -= HandleTargetDeath;
            m_TargetHealth = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( transform.position, DetectionRadius );
    }
}
