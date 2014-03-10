using UnityEngine;
using UnityObjectRetrieval;
using EventTools;
using System.Linq;

public class PlayerInventory : ExtendedMonoBehaviour
{
    PlayerInput m_Input;

    PlayerHandSlot LeftSlot;
    PlayerHandSlot RightSlot;

    void Awake()
    {
        m_Input = Component<PlayerInput>();
        var hands = Descendants().Components<PlayerHandSlot>();
        LeftSlot = hands.First( h => h.Side == HandSide.Left );
        RightSlot = hands.First( h => h.Side == HandSide.Right );
    }

    void UpdateTryStartUse( PlayerHandSlot slot, PlayerKey key )
    {
        if( m_Input.GetKeyDown( key ) ) slot.TryStartUse();
    }

    void UpdateTryStopUse( PlayerHandSlot slot, PlayerKey key )
    {
        if( m_Input.GetKeyUp( key ) ) slot.TryStopUse();
    }

    bool IsUseBlocked
    {
        get
        {
            return LeftSlot.IsBlockingUse || RightSlot.IsBlockingUse;
        }
    }

    void FixedUpdate()
    {
        if( ! IsUseBlocked )
        {
            UpdateTryStartUse( LeftSlot, PlayerKey.UseLeft );
            UpdateTryStartUse( RightSlot, PlayerKey.UseRight );
        }
        UpdateTryStopUse( LeftSlot, PlayerKey.UseLeft );
        UpdateTryStopUse( RightSlot, PlayerKey.UseRight );
    }
}