using UnityEngine;
using System.Collections;
using UnityObjectRetrieval;
using InvalidOperationException = System.InvalidOperationException;

public class MapBuilder : MonoBehaviour
{
    public Transform WallPrefab;
    public Transform FloorPrefab;

    Transform[,] m_Tiles;
    readonly Vector3 TileSize = new Vector3( 1f, 0f, 1f );

    // Cached components.
    Transform m_Transform;

    // Scene objects.
    PlayerStartFactory m_PlayerStartFactory;

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_PlayerStartFactory = Scene.Object<PlayerStartFactory>();
    }

    public void Build( Map map )
    {
        m_Tiles = new Transform[map.Width, map.Height];

        for( int y = 0; y < map.Height; y++ )
        {
            for( int x = 0; x < map.Width; x++ )
            {
                var prefab = TilePrefab( map.Tiles[x, y] );
                if( prefab != null )
                {
                    m_Tiles[x, y] = InstantiateAtPoint( prefab, x, y );
                }
            }
        }

        foreach( var featurePoint in map.Features )
        {
            AddFeature( featurePoint.Feature, Position( featurePoint.Point ) );
        }
    }

    Transform InstantiateAtPoint( Transform tilePrefab, int x, int y )
    {
        var tile = (Instantiate( tilePrefab ) as Transform);
        tile.position = Position( x, y );
        return tile;
    }

    Vector3 Position( GridPoint point )
    {
        return Position( point.X, point.Y );
    }

    Vector3 Position( int x, int y )
    {
        return m_Transform.position + Vector3.Scale(
            TileSize,
            new Vector3( x, 0, y )
        );
    }

    Transform AddFeature( FeatureType feature, Vector3 position )
    {
        if( feature == FeatureType.Player0Start ) return m_PlayerStartFactory.Build( 0, position );
        if( feature == FeatureType.Player1Start ) return m_PlayerStartFactory.Build( 1, position );
        if( feature == FeatureType.Player2Start ) return m_PlayerStartFactory.Build( 2, position );
        if( feature == FeatureType.Player3Start ) return m_PlayerStartFactory.Build( 3, position );

        throw new InvalidOperationException( string.Format(
            "Unrecognized feature type: {0}", feature
        ) );
    }

    Transform TilePrefab( TileType tile )
    {
        if( tile == TileType.Blocked ) return WallPrefab;
        if( tile == TileType.Floor ) return FloorPrefab;
        return null;
    }
}
