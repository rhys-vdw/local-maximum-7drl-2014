using UnityEngine;
using Pathfinding;
using UnityObjectRetrieval;
using System;

public class PathFollower : ExtendedMonoBehaviour
{
    public float NextWaypointProximity = 0.1f;

    public float MaxSpeed = 3f;
    public float Acceleration = 10f;

    public event Action<PathFollower> ReachedEndOfPathEvent;

    Transform m_Target;
    Path m_Path;
    float m_Speed = 0f;
    int m_CurrentWaypoint = 0;


    // Cached components.

    Vector3 m_Target;
    Seeker m_Seeker;
    CharacterController m_Controller;

    void Awake()
    {
        m_Transform = transform;
        m_Seeker = Component<Seeker>();
        m_Controller = Component<CharacterController>();
    }

    public void Go( Vector target )
    {
        m_Target = target;
        RetrackPath();
    }

    public void Stop()
    {
        m_Path = null;
    }

    void FixedUpdate()
    {
        if( m_Path != null && m_CurrentWaypoint < m_Path.vectorPath.Count )
        {
            m_Speed = Mathf.Min( m_Speed + Acceleration * Time.fixedDeltaTime, MaxSpeed );

            var nextWaypoint = m_Path.vectorPath[ m_CurrentWaypoint ];
            var direction = (nextWaypoint - m_Transform.position).WithY( 0f ).normalized;
            var step = direction * m_Speed * Time.fixedDeltaTime;

            m_Controller.Move( step );

            if( Vector3.Distance( m_Transform.position, nextWaypoint ) < NextWaypointProximity )
            {
                m_CurrentWaypoint++;
                if( m_CurrentWaypoint == m_Path.vectorPath.Count && ReachedEndOfPathEvent != null )
                {
                    ReachedEndOfPathEvent( this );
                }
            }
        }
    }

    public void RetrackPath()
    {
        if( m_Target == null )
        {
            m_Path = null;
        }
        else
        {
            m_Seeker.StartPath( m_Transform.position, m_Target, (p) => {
                m_Path = p;
                m_CurrentWaypoint = 0;
            } );
        }
    }
}