using UnityEngine;
using UnityObjectRetrieval;

public class Sword : ExtendedMonoBehaviour, IItem
{
    bool m_IsSwinging = false;
    public float m_SwingLength = 0.2f;

    PlayerHand m_Slot = null;
    PlayerAnimation m_Animation;

    public bool IsBlockingUse
    {
        get { return m_IsSwinging; }
    }

    void Start()
    {
        m_Animation = Ancestors().Component<PlayerAnimation>();
    }

    public void OnEquip( PlayerHand slot )
    {
        m_Slot = slot;
        m_Slot.TryStartUseEvent += HandleTryStartUse;
    }

    public void OnUnequip()
    {
        m_Slot = null;
        m_Slot.TryStartUseEvent -= HandleTryStartUse;
    }

    public void HandleTryStartUse()
    {
        Swing();
    }

    void Swing()
    {
        m_IsSwinging = true;
        var name = m_Slot.Side == HandSide.Left ? "SwingSwordLeft" : "SwingSwordRight";
        m_Animation.Play( name, () => { m_IsSwinging = false; } );
    }
}