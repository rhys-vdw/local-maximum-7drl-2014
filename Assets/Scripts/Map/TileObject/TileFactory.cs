using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using System.Linq;

public class TileFactory : ExtendedMonoBehaviour
{
    public Transform PlanePrefab;

    public SpriteSheet FloorSprites;
    public SpriteSheet WallSprites;
    public SpriteSheet RoofSprites;

    public float HeightScale = 1f;

    enum TileSide
    {
        Top,
        Front,
        Floor
    }

    class Rule
    {
        public string SpriteName { get; private set; }
        TileType[,] m_Filter;

        public Rule( TileType[,] filter, string spriteName )
        {
            m_Filter = filter;
            SpriteName = spriteName;
        }

        public bool IsApplicable( MapMask mask )
        {
            for( int y = 0; y < 3; y++ )
            {
                for( int x = 0; x < 3; x++ )
                {
                    if( (m_Filter[x, y] & mask[y, 2 - x]) == 0 ) return false;
                }
            }
            return true;
        }
    }

    Dictionary<TileSide, Rule[]> m_Rules = new Dictionary<TileSide, Rule[]>();

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

        m_Rules[ TileSide.Top ] = new []
        {
            new Rule( new [,]
            {
                { _, _, _ },
                { _, B, _ },
                { _, _, _ }
            }, "roof" ),
        };

        m_Rules[ TileSide.Front ] = new []
        {
            new Rule( new [,]
            {
                { _, _, _ },
                { B, B, B },
                { _, F, _ }
            }, "wall-center" ),

            new Rule( new [,]
            {
                { _, _, _ },
                { F, B, _ },
                { F, F, _ }
            }, "wall-left" ),

            new Rule( new [,]
            {
                { _, _, _ },
                { _, B, F },
                { _, F, F }
            }, "wall-right" ),

            new Rule( new [,]
            {
                { _, _, _ },
                { F, B, F },
                { F, F, F }
            }, "wall-left-right" ),

            new Rule( new [,]
            {
                { _, _, _ },
                { _, B, _ },
                { _, F, _ }
            }, "wall-center" ),
        };

        m_Rules[ TileSide.Floor ] = new []
        {
            new Rule( new [,]
            {
                { _, _, _ },
                { _, f, _ },
                { _, _, _ }
            }, "floor" ),
        };
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

        foreach( var side in EnumUtil.Values<TileSide>() )
        {
            var rule = m_Rules[side].FirstOrDefault( r => r.IsApplicable( mask ) );
            if( rule != null )
            {
                AddPlane( tile, side, rule.SpriteName );
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

    // Plane positioning stuff.

    class TransformInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    static readonly Dictionary<TileSide, TransformInfo> PlaneTransformInfo = new Dictionary<TileSide, TransformInfo>
    {
        {
            TileSide.Top,
            new TransformInfo
            {
                Position = new Vector3( 0f, 1f, 0f ),
                Rotation = Quaternion.Euler( 270f, 0f, 0f )
            }
        },
        {
            TileSide.Front,
            new TransformInfo
            {
                Position = new Vector3( 0f, 0.5f, -0.5f ),
                Rotation = Quaternion.Euler( 0f, 180f, 0f )
            }
        },
        {
            TileSide.Floor,
            new TransformInfo
            {
                Position = new Vector3( 0f, 0f, 0f ),
                Rotation = Quaternion.Euler( 270f, 0f, 0f )
            }
        },
    };
 
    void AddPlane( Transform tile, TileSide side, string spriteName )
    {
        var transformInfo = PlaneTransformInfo[ side ];
        var plane = Instantiate( PlanePrefab ) as Transform;
        plane.name = string.Format( "{0}: {1}", side, spriteName );
        plane.parent = tile;
        plane.localPosition = transformInfo.Position;
        plane.localRotation = transformInfo.Rotation;

        var sheet =
            side == TileSide.Top ? RoofSprites :
            side == TileSide.Floor ? FloorSprites :
            side == TileSide.Front ? WallSprites :
            null;

        sheet.AutoApply( spriteName, tile );
    }
}