using UnityEngine;
using System.Collections;
using UnityObjectRetrieval;
using System;
using InvalidOperationException = System.InvalidOperationException;

public class MapBuilder : MonoBehaviour
{
    public event Action<MapBuilder> BuildCompleteEvent;

    Tile[,] m_Tiles;
    public readonly float TileSize = 1f;

    // Store the map.
    Map m_Map;

    // Cached components.
    Transform m_Transform;

    // Children.
    Transform m_TileParent;
    Transform m_FeatureParent;

    // Scene objects.
    PlayerStartFactory m_PlayerStartFactory;
    TileManager m_TileManager;

    public Vector3 Center
    {
        get { return new Vector3( Width / 2, 0f, Length / 2 ); }
    }

    public int Columns
    {
        get { return m_Tiles.GetLength( 0 ); }
    }

    public int Rows
    {
        get { return m_Tiles.GetLength( 1 ); }
    }

    public float Width
    {
        get { return m_Tiles.GetLength( 0 ) * TileSize; }
    }

    public float Length
    {
        get { return m_Tiles.GetLength( 1 ) * TileSize; }
    }

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_PlayerStartFactory = Scene.Object<PlayerStartFactory>();
        m_TileManager = Scene.Object<TileManager>();

        m_TileParent = new GameObject( "Tiles" ).transform;
        m_TileParent.parent = m_Transform;
        m_TileParent.localPosition = Vector3.zero;

        m_FeatureParent = new GameObject( "Features" ).transform;
        m_FeatureParent.parent = m_Transform;
        m_FeatureParent.localPosition = Vector3.zero;
    }

    public void SetTileType( int x, int y, TileType newType )
    {
        m_Map.Tiles[x, y] = newType;
        m_Tiles[x, y].Type.Value = newType;
        m_Tiles[x, y].name = "Updated to: " + newType;

        MaskAroundCenter( x, y ).Each( (u, v, type) => {
            if( type != TileType.OutOfBounds ) UpdateTile( m_Tiles[x - 1 + u, y - 1 + v] );
        } );
    }

    MapMask MaskAroundCenter( int x, int y )
    {
        return new MapMask( m_Map.Tiles, x - 1, y - 1 );
    }

    void UpdateTile( Tile tile )
    {
        var mask = MaskAroundCenter( tile.X, tile.Y );
        m_TileManager.DecorateTile( tile, mask );
    }

    public void Build( Map map )
    {
        m_Tiles = new Tile[map.Width, map.Height];
        m_Map = map;

        for( int y = 0; y < map.Height; y++ )
        {
            for( int x = 0; x < map.Width; x++ )
            {
                var mask = MaskAroundCenter( x, y );

                var tile = m_TileManager.RequestTile( x, y, mask );

                tile.name = string.Format(
                    "Tile (pos=[{0},{1}] type={2})",
                    x, y, tile.Type.Value
                );

                var tileTransform = tile.transform;
                tileTransform.position = Position( x, y );
                tileTransform.parent = m_TileParent;
                m_Tiles[x, y] = tile;
            }
        }

        foreach( var featurePoint in map.Features )
        {
            AddFeature( featurePoint.Feature, Position( featurePoint.Point ) );
        }

        if( BuildCompleteEvent != null ) BuildCompleteEvent( this );
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
        return m_Transform.position + new Vector3( x * TileSize, 0, y * TileSize );
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
}
