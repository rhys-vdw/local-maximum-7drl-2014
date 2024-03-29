using UnityEngine;
using UnityObjectRetrieval;
using System;

public class Sword : ExtendedMonoBehaviour
{
    public float BlockUseDuration = 0.2f;
    public float MinAttackPeriod = 0.15f;

    public string LeftAttackAnimation = "SwingSwordLeft";
    public string RightAttackAnimation = "SwingSwordRight";

    public event Action<Sword> AttackEvent;

    Item m_Item;
    PlayerHand m_Hand = null;
    PlayerAnimation m_Animation;

    void Awake()
    {
        m_Item = Component<Item>();
        m_Item.StartUseEvent += HandleStartUse;
        m_Item.EquipEvent += HandleEquip;
        m_Item.UnequipEvent += HandleUnquip;
    }

    void Start()
    {
        m_Animation = Ancestors().Component<PlayerAnimation>();
    }

    public void HandleEquip( PlayerHand hand )
    {
        m_Hand = hand;
    }

    public void HandleUnquip()
    {
        m_Hand = null;
    }

    public void HandleStartUse()
    {
        Attack();
    }

    void Attack()
    {
        m_Item.SetBlockUseTimeout( BlockUseDuration );
        m_Item.SetCoolDownTimeout( MinAttackPeriod );

        var name = m_Hand.Side == HandSide.Left ? "SwingSwordLeft" : "SwingSwordRight";
        m_Animation.Play( name, MinAttackPeriod );

        if( AttackEvent != null ) AttackEvent( this );
    }
}