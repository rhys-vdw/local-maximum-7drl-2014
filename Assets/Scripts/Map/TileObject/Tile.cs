using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using EventTools;

public class Tile : MonoBehaviour
{
    public int X = -1;
    public int Y = -1;

    public MeshRenderer Top;
    public MeshRenderer Floor;
    public MeshRenderer Front;
    public MeshRenderer Underground;

    public BoxCollider WallCollider;
    public BoxCollider FloorCollider;

    public Watchable<TileType> Type = new Watchable<TileType>();

    void Awake()
    {
        Type.Watch( HandleTileTypeChanged );
    }

    void HandleTileTypeChanged( TileType type )
    {
        FloorCollider.enabled = !(type == TileType.None);
        WallCollider.enabled = type == TileType.Blocked;
    }

    public MeshRenderer GetPlaneMeshRenderer( TileSide side )
    {
        return
            side == TileSide.Top         ? Top         :
            side == TileSide.Floor       ? Floor       :
            side == TileSide.Front       ? Front       :
            side == TileSide.Underground ? Underground :
            null;
    }
}
