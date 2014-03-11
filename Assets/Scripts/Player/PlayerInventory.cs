using UnityEngine;
using UnityObjectRetrieval;
using EventTools;
using System.Linq;

public class PlayerInventory : ExtendedMonoBehaviour
{
    class InventoryItem
    {
        public string Name;
        public Item Item;
    }

    // Cached components.

    PlayerInput m_Input;
    PlayerHand m_LeftHand;
    PlayerHand m_RightHand;

    // Scene objects.

    ItemFactory m_ItemFactory;

    // Private fields.

    InventoryItem m_LeftHandSlot = null;
    InventoryItem m_RightHandSlot = null;
    InventoryItem m_BackpackSlot = null;
    InventoryItem m_WearableSlot = null;

    // Unity event handlers.

    void Awake()
    {
        m_Input = Component<PlayerInput>();
        var hands = Descendants().Components<PlayerHand>();
        m_LeftHand = hands.First( h => h.Side == HandSide.Left );
        m_RightHand = hands.First( h => h.Side == HandSide.Right );

        m_ItemFactory = Scene.Object<ItemFactory>();

        Component<Player>().Config.Watch( HandleConfigure );
    }

    void Update()
    {
        if( ! IsUseBlocked )
        {
            if( m_Input.GetKeyDown( PlayerKey.UseLeft  ) ) m_LeftHand.TryStartUse();
            if( m_Input.GetKeyDown( PlayerKey.UseRight ) ) m_RightHand.TryStartUse();
        }
        if( m_Input.GetKeyUp( PlayerKey.UseLeft  ) ) m_LeftHand.TryStopUse();
        if( m_Input.GetKeyUp( PlayerKey.UseRight ) ) m_RightHand.TryStopUse();
    }

    // Event handlers.

    void HandleConfigure( PlayerConfig config )
    {
        var items = config.StartingItems;
        TryEquip( ref m_LeftHandSlot,  items.LeftHand  );
        TryEquip( ref m_RightHandSlot, items.RightHand );
        TryEquip( ref m_BackpackSlot,  items.Backpack  );
        TryEquip( ref m_WearableSlot,  items.Wearable  );

        UpdateEquipment();
    }

    // Private helpers.

    void UpdateEquipment()
    {
        // If there should be an item in the hand, and it is not the given item.
        if( m_LeftHandSlot == null )
        {
            if( m_LeftHand.Item != null ) m_LeftHand.Unequip();
        }
        else if( m_LeftHand.Item != m_LeftHandSlot.Item )
        {
            m_LeftHand.Equip( m_LeftHandSlot.Item );
        }

        if( m_RightHandSlot == null )
        {
            if( m_RightHand.Item != null ) m_RightHand.Unequip();
        }
        else if( m_RightHand.Item != m_RightHandSlot.Item )
        {
            m_RightHand.Equip( m_RightHandSlot.Item );
        }

        if( m_BackpackSlot != null )
        {
            (m_BackpackSlot.Item as MonoBehaviour).gameObject.SetActive( false );
        }

        // TODO: Wearable.
    }

    void TryEquip( ref InventoryItem slot, string name )
    {
        if( ! string.IsNullOrEmpty( name ) )
        {
            slot = new InventoryItem {
                Name = name,
                Item = m_ItemFactory.Build( name )
            };

            var component = slot.Item as MonoBehaviour;
            component.transform.parent = transform;
            component.gameObject.SetActive( false );
       } 
    }

    bool IsUseBlocked
    {
        get
        {
            return m_LeftHand.IsBlockingUse || m_RightHand.IsBlockingUse;
        }
    }
}