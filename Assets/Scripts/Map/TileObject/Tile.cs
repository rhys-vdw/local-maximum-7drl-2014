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

    public Watchable<TileType> TileType = new Watchable<TileType>();

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
