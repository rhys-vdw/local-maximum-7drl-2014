using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class Bow : ExtendedMonoBehaviour
{
    public float m_CooldownTime = 1f;
    public float m_MinCharge = 0.1f;
    MissileAttack m_Attack;
    Item m_Item;

    bool m_IsCharging = false;
    float m_ChargeTime = 0.5f;
    float m_Charge = 0f;

    void Awake()
    {
        m_Attack = Component<MissileAttack>();

        m_Item = Component<Item>();
        m_Item.HoldUseEvent += HandleHoldUse;
        m_Item.StopUseEvent += HandleStopUse;
    }

    void Update()
    {
        if( m_IsCharging )
        {
            m_Charge += (1f / m_ChargeTime) * Time.deltaTime;
        }
    }

    void HandleHoldUse()
    {
        if( ! m_IsCharging )
        {
            m_IsCharging = true;
            m_Charge = 0f;
        }
    }

    void HandleStopUse()
    {
        if( m_IsCharging && m_Charge > m_MinCharge )
        {
            // TODO: Alter damage/speed etc based on charge.
            m_Attack.Attack();
            m_Item.SetCoolDownTimeout( m_CooldownTime );

            m_Charge = 0f;
            m_IsCharging = false;
        }
    }
}
