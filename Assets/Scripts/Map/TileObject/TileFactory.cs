using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using System.Linq;

public class TileFactory : ExtendedMonoBehaviour
{
    public Transform PlanePrefab;

    public SpriteSheet FloorSprites;
    public SpriteSheet WallSprites;
    public SpriteSheet DebugSprites;

    public float HeightScale = 1f;

    enum TileSide
    {
        Top,
        Front,
        Floor
    }

    class TileConfig
    {
        public TileSide Side { get; private set; }
        public string Name { get; private set; }
        TileType[,] m_Filter;

        public TileConfig( TileType[,] filter, TileSide side, string name )
        {
            m_Filter = filter;
            Side = side;
            Name = name;
        }

        public bool IsApplicable( MapMask mask )
        {
            for( int y = 0; y < 3; y++ )
            {
                for( int x = 0; x < 3; x++ )
                {
                    if( (m_Filter[x,y] & mask[y,2-x]) == 0 ) return false;
                }
            }
            return true;
        }
    }

    List<TileConfig> m_TileConfigs = new List<TileConfig>();

    void AddTileConfig( TileType[,] filter, TileSide side, string name )
    {
        m_TileConfigs.Add( new TileConfig(
            filter, side, name
        ) );
    }

    void InitializeMaskMap()
    {
        // High part requiring roof. (Blocked)
        var B = TileType.Blocked | TileType.OutOfBounds;

        // Walkable (low).
        var F = TileType.Floor | TileType.Water | TileType.None | TileType.Lava;

        // Actual floor.
        var f = TileType.Floor;

        // Wildcard (any).
        var _ = (TileType) ~0;

        AddTileConfig( new [,]
            { { _, _, _ },
              { _, B, _ },
              { _, _, _ } },
          TileSide.Top,
          "roof"
        );

        AddTileConfig( new [,]
            { { _, _, _ },
              { _, B, _ },
              { _, F, _ } },
          TileSide.Front,
          "wall-center"
        );

        AddTileConfig( new [,]
            { { _, _, _ },
              { _, f, _ },
              { _, _, _ } },
          TileSide.Floor,
          "floor"
        );
    }

    void Awake()
    {
        InitializeMaskMap();
    }

    public Transform Build( string name, MapMask mask )
    {
        var tile = new GameObject(
            string.Format( "{0}: {1}", name, mask[1,1].ToString() )
        ).transform;

        foreach( var config in m_TileConfigs.Where( c => c.IsApplicable( mask ) ) )
        {
            switch( config.Side )
            {
                case TileSide.Top:    AddTop(   tile, config.Name ); break;
                case TileSide.Front:  AddFront( tile, config.Name ); break;
                case TileSide.Floor:  AddFloor( tile, config.Name ); break;
                default: throw new System.InvalidOperationException(
                        "Unknown TileSide: " + config.Side
                );
            }
        }

        if( mask.Center == TileType.Blocked )
        {
            var collider = tile.gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3( 0f, 0.5f, 0f );
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

    void AddFloor( Transform tile, string name )
    {
        AddPlane(
            tile,
            "floor -> " + name,
            new Vector3( 0f, 0f, 0f ),
            Quaternion.Euler( 270f, 0f, 0f )
        );
        FloorSprites.AutoApply( name, tile );
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