using UnityEngine;
using System;

public class PlayerHand : MonoBehaviour
{
    public HandSide Side = HandSide.Left;

    public Item Item { get; private set; }

    public bool IsBlockingUse
    {
        get { return Item != null && Item.IsBlockingUse; }
    }

    public bool IsCoolingDown
    {
        get { return Item != null && Item.IsCoolingDown; }
    }

    public void TryStartUse()
    {
        if( Item != null ) Item.TryStartUse();
    }

    public void TryHoldUse()
    {
        if( Item != null ) Item.TryHoldUse();
    }

    public void TryStopUse()
    {
        Debug.Log( name + " TRY STOP" );
        if( Item != null ) Item.TryStopUse();
    }

    public void Equip( Item item )
    {
        // TODO: This can be a coroutine with animations and such.

        Debug.Log( "equipping!", (MonoBehaviour) item );

        if( Item != null ) Unequip();
        Item = item;

        item.transform.parent = transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.gameObject.SetActive( true );
        
        Item.OnEquip( this );
    }

    public void Unequip()
    {
        Item.OnUnequip( this );
        Item.gameObject.SetActive( false );
        Item = null;
    }
}