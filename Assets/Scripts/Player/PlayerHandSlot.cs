using UnityEngine;
using System;

public class PlayerHandSlot : MonoBehaviour
{
    public HandSide Side = HandSide.Left;

    public event Action TryStartUseEvent;
    public event Action TryStopUseEvent;

    IItem m_Item;

    public bool IsBlockingUse
    {
        get { return m_Item != null && m_Item.IsBlockingUse; }
    }

    public void TryStartUse()
    {
        Debug.Log( "TryStartUse: " + Side );
        if( TryStartUseEvent != null ) TryStartUseEvent();
    }

    public void TryStopUse()
    {
        Debug.Log( "TryStopUse: " + Side );
        if( TryStopUseEvent != null ) TryStopUseEvent();
    }

    public void EquipItem( IItem item )
    {
        // TODO: This can be a coroutine with animations and such.
        Unequip();
        m_Item = item;
        m_Item.OnEquip( this );
    }

    public void Unequip()
    {
        if( m_Item != null )
        {
            m_Item.OnUnequip();
        }
    }
}