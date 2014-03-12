using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class Bow : ExtendedMonoBehaviour
{
    public float m_CooldownTime = 1f;
    MissileAttack m_Attack;

    bool m_IsCharging = false;
    float m_ChargeTime = 0.5f;
    float m_Charge = 0f;

    void Awake()
    {
        m_Attack = Component<MissileAttack>();

        var item = Component<Item>();
        item.TryStartUseEvent += HandleTryStartUse;
        item.TryStopUseEvent += HandleTryStopUse;
    }

    void Update()
    {
        if( m_IsCharging )
        {
            Debug.Log( "Charging -- " + m_Charge );
            m_Charge += (1f / m_ChargeTime) * Time.deltaTime;
        }
    }

    void HandleTryStartUse()
    {
        if( ! m_IsCharging )
        {
            m_IsCharging = true;
            m_Charge = 0f;
        }
    }

    void HandleTryStopUse()
    {
        // TODO: Alter damage/speed etc based on charge.
        m_Attack.Attack();

        m_Charge = 0f;
        m_IsCharging = false;
    }
}
