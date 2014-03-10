using UnityEngine;
using System;

public class PlayerHand : MonoBehaviour
{
    public HandSide Side = HandSide.Left;

    public event Action TryStartUseEvent;
    public event Action TryStopUseEvent;

    public IItem Item { get; private set; }

    public bool IsBlockingUse
    {
        get { return Item != null && Item.IsBlockingUse; }
    }

    public void TryStartUse()
    {
        if( TryStartUseEvent != null ) TryStartUseEvent();
    }

    public void TryStopUse()
    {
        if( TryStopUseEvent != null ) TryStopUseEvent();
    }

    public void Equip( IItem item )
    {
        // TODO: This can be a coroutine with animations and such.

        Debug.Log( "equipping!", (MonoBehaviour) item );

        if( Item != null ) Unequip();
        Item = item;

        var component = Item as MonoBehaviour;
        component.transform.parent = transform;
        component.transform.localPosition = Vector3.zero;
        component.transform.localRotation = Quaternion.identity;
        component.gameObject.SetActive( true );
        
        Item.OnEquip( this );
    }

    public void Unequip()
    {
        Item.OnUnequip();
        (Item as MonoBehaviour).gameObject.SetActive( false );
    }
}