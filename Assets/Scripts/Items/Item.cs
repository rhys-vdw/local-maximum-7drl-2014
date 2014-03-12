using UnityEngine;
using System;

// Any co-components to Item can assume that Start() Will be called once when
// the item is picked up, after it is childed to the adventurer.
public class Item : MonoBehaviour
{
    public event Action TryStartUseEvent;
    public event Action TryStopUseEvent;

    // Called every time the item is placed in the hand of an adventurer.
    public event Action<PlayerHand> EquipEvent;

    // Called when dropped or when moving inventory slot.
    public event Action UnequipEvent;

    float m_UnblockUseTime = 0f;
    float m_CooldownEndTime = 0f;

    public bool IsBlockingUse
    {
        get { return Time.time < m_UnblockUseTime; }
    }

    public bool IsCoolingDown
    {
        get { return Time.time < m_CooldownEndTime; }
    }

    // Prevent the player from using an item for a time.
    public void SetBlockUseTimeout( float duration )
    {
        m_UnblockUseTime = Time.time + duration;
    }

    public void SetCooldown( float duration )
    {
        m_CooldownEndTime = Time.time + duration;
    }

    // Forward these events on to our other components. This way they are not
    // responsible for subscribing and unsubscribing to the hand every time
    // they're equipped.
    public void HandleTryStartUse()
    {
        if( TryStartUseEvent != null ) TryStartUseEvent();
    }

    public void HandleTryStopUse()
    {
        if( TryStopUseEvent != null ) TryStopUseEvent();
    }

    public void OnEquip( PlayerHand hand )
    {
        hand.TryStartUseEvent += HandleTryStartUse;
        hand.TryStopUseEvent += HandleTryStopUse;

        if( EquipEvent != null ) EquipEvent( hand );
    }

    public void OnUnequip( PlayerHand hand )
    {
        hand.TryStartUseEvent -= HandleTryStartUse;
        hand.TryStopUseEvent -= HandleTryStopUse;

        if( UnequipEvent != null ) UnequipEvent();
    }
}