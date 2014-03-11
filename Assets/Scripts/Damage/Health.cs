using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action<Health> DeathEvent;
    public event Action<Health, int> ChangeEvent;

    public int Max = 100;
    public float RegenerationRate = 0f;

    int m_Current;
    float m_Regeneration = 0f;

    public int Current
    {
        get { return m_Current; }
        set
        {
            if( m_Current != value )
            {
                int prev = m_Current;
                m_Current = Mathf.Min( Max, value );

                if( m_Current <= 0 && DeathEvent != null )
                {
                    DeathEvent( this );
                }

                // Change event is still fired on death, but after the death
                // event so the receiver may choose to flag and ignore it.
                if( ChangeEvent != null )
                {
                    ChangeEvent( this, m_Current - prev );
                }
            }
        }
    }

    void Start()
    {
        m_Current = Max;
    }

    void FixedUpdate()
    {
        if( m_Current < Max )
        {
            m_Regeneration += RegenerationRate * Time.fixedDeltaTime;
            if( m_Regeneration > 1 )
            {
                m_Regeneration--;
                Current++;
            }
        }
    }
}