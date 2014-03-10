// Anything implementing IITem can make the following assumptions:
//  - Start():
//    - Will be called every time the item is picked up.
//    - Will be called after being childed to the player.
//  - Equip():
//    - Will be called when the item is able to be used by the player, either
//      when first drawn from backpack, or after stopping running.
public interface IItem
{
    bool IsBlockingUse { get; }
    void OnEquip( PlayerHandSlot slot );
    void OnUnequip();
}