using UnityEngine;
using UnityObjectRetrieval;

public class TileFactory : ExtendedMonoBehaviour
{
    public Transform PlanePrefab;

    public SpriteSheet FloorSprites;
    public SpriteSheet WallSprites;
    public SpriteSheet DebugSprites;

    public float HeightScale = 1f;

    public Transform Build( string name, TileType[,] mask )
    {
        var tile = new GameObject(
            string.Format( "{0}: {1}", name, mask[1,1].ToString() )
        ).transform;

        if( mask[1,1] == TileType.Blocked )
        {
            AddTop( tile, "roof" );
            if( mask[1,0] != TileType.Blocked )
            {
                if( mask[0,1] == TileType.Blocked && mask[2,1] == TileType.Blocked )
                {
                    AddFront( tile, "wall" );
                }
                else if( mask[0,1] == TileType.Blocked && mask[2,1] != TileType.Blocked )
                {
                    AddFront( tile, "wall-right" );
                }
                else if( mask[0,1] != TileType.Blocked && mask[2,1] == TileType.Blocked )
                {
                    AddFront( tile, "wall-left" );
                }
                else
                {
                    AddFront( tile, "wall-left-right" );
                }
            }
            
            var collider = tile.gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3( 0f, 0.5f, 0f );
        }
        else if( mask[1,1] == TileType.Floor )
        {
            AddFloor( tile );
        }


        tile.localScale = new Vector3( 1f, HeightScale, 1f );
        return tile;
    }

    void AddFront( Transform tile, string name )
    {
        AddPlane(
            tile,
            "front -> " + name,
            new Vector3( 0f, 0.5f, -0.5f ),
            Quaternion.Euler( 0f, 180f, 0f )
        );
        WallSprites.AutoApply( name, tile );
    }

    void AddTop( Transform tile, string name )
    {
        AddPlane(
            tile,
            "top -> " + name,
            new Vector3( 0f, 1f, 0f ),
            Quaternion.Euler( 270f, 0f, 0f )
        );
        DebugSprites.AutoApply( name, tile );
    }

    void AddFloor( Transform tile )
    {
        AddPlane(
            tile,
            "floor",
            new Vector3( 0f, 0f, 0f ),
            Quaternion.Euler( 270f, 0f, 0f )
        );
        FloorSprites.AutoApplyRandom( tile );
    } 

    void AddPlane( Transform tile, string name, Vector3 position, Quaternion rotation )
    {
        var plane = Instantiate( PlanePrefab ) as Transform;
        plane.name = name;
        plane.parent = tile;
        plane.localPosition = position;
        plane.localRotation = rotation;
    }
}