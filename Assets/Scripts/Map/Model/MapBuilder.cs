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

    // Children.
    Transform m_TileParent;
    Transform m_FeatureParent;

    // Scene objects.
    PlayerStartFactory m_PlayerStartFactory;

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_PlayerStartFactory = Scene.Object<PlayerStartFactory>();

        m_TileParent = new GameObject( "Tiles" ).transform;
        m_TileParent.parent = m_Transform;
        m_TileParent.localPosition = Vector3.zero;

        m_FeatureParent = new GameObject( "Features" ).transform;
        m_FeatureParent.parent = m_Transform;
        m_FeatureParent.localPosition = Vector3.zero;
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
                    var tile = InstantiateAtPoint( prefab, x, y );
                    tile.parent = m_TileParent;
                    m_Tiles[x, y] = tile;
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

    Transform AddFeature( FeatureType featureType, Vector3 position )
    {
        var feature = 
            ( featureType == FeatureType.Player0Start ) ? m_PlayerStartFactory.Build( 0, position ) :
            ( featureType == FeatureType.Player1Start ) ? m_PlayerStartFactory.Build( 1, position ) :
            ( featureType == FeatureType.Player2Start ) ? m_PlayerStartFactory.Build( 2, position ) :
            ( featureType == FeatureType.Player3Start ) ? m_PlayerStartFactory.Build( 3, position ) :
            null;

        if( feature == null ) throw new InvalidOperationException( string.Format(
            "Unrecognized feature type: {0}", feature
        ) );

        feature.parent = m_FeatureParent;
        return feature;
    }

    Transform TilePrefab( TileType tile )
    {
        if( tile == TileType.Blocked ) return WallPrefab;
        if( tile == TileType.Floor ) return FloorPrefab;
        return null;
    }
}
