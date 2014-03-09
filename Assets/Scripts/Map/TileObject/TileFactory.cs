using UnityEngine;
using UnityObjectRetrieval;

public class TileFactory : ExtendedMonoBehaviour
{
    public Transform PlanePrefab;
    public SpriteSheet SpriteSheet;
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
                AddFront( tile, "wall" );
            }
            
            var collider = tile.gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3( 0f, 0.5f, 0f );
        }
        else if( mask[1,1] == TileType.Floor )
        {
            AddFloor( tile, "floor" );
        }


        tile.localScale = new Vector3( 1f, HeightScale, 1f );
        return tile;
    }

    void AddFront( Transform tile, string name )
    {
        AddPlane(
            tile,
            name,
            new Vector3( 0f, 0.5f, -0.5f ),
            Quaternion.Euler( 0f, 180f, 0f )
        );
    }

    void AddTop( Transform tile, string name )
    {
        AddPlane(
            tile,
            name,
            new Vector3( 0f, 1f, 0f ),
            Quaternion.Euler( 270f, 0f, 0f )
        );
    }

    void AddFloor( Transform tile, string name )
    {
        AddPlane(
            tile,
            name,
            new Vector3( 0f, 0f, 0f ),
            Quaternion.Euler( 270f, 0f, 0f )
        );
    } 

    void AddPlane( Transform tile, string name, Vector3 position, Quaternion rotation )
    {
        var plane = Instantiate( PlanePrefab ) as Transform;
        plane.name = name;
        plane.parent = tile;
        plane.localPosition = position;
        plane.localRotation = rotation;
        SpriteSheet.AutoApply( name, plane );
    }
}