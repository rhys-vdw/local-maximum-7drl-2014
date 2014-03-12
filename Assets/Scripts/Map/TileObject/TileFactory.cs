using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using System.Linq;

public class TileFactory : ExtendedMonoBehaviour
{
    public Tile TilePrefab;
    public float HeightScale = 2f;

    public SpriteSheet FloorSprites;
    public SpriteSheet WallSprites;
    public SpriteSheet RoofSprites;
    public SpriteSheet UndergroundSprites;

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

    static Dictionary<TileSide, Rule[]> CreateRules()
    {
        var rules = new Dictionary<TileSide, Rule[]>();

        // High part requiring roof. (Blocked)
        var B = TileType.Blocked | TileType.OutOfBounds;

        // Walkable (low).
        var F = TileType.Floor | TileType.Water | TileType.None | TileType.Lava;

        // Actual floor.
        var f = TileType.Floor;

        // Wildcard (any).
        var _ = (TileType) ~0;

        // -- Top Rules --

        rules[ TileSide.Top ] = new []
        {
            new Rule( new [,]
            {
                { _, _, _ },
                { _, B, _ },
                { _, _, _ }
            }, "roof" ),
        };

        // -- Front Rules --

        rules[ TileSide.Front ] = new []
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

        // -- Front Rules --

        rules[ TileSide.Floor ] = new []
        {
            new Rule( new [,]
            {
                { _, _, _ },
                { _, f, _ },
                { _, _, _ }
            }, "floor" ),
        };

        rules[ TileSide.Underground ] = new Rule[0];

        return rules;
    }

    void Awake()
    {
        m_Rules = CreateRules();
    }

    public Tile Build( string name, MapMask mask )
    {
        var tile = Instantiate( TilePrefab ) as Tile;
        tile.name = string.Format( "Tile: {0}, {1}", name, mask[1,1].ToString() );
        tile.transform.localScale = new Vector3( 1f, HeightScale, 1f );
        tile.TileType.Value = mask[1,1];

        DecorateTile( tile, mask );

        return tile;
    }

    public void DecorateTile( Tile tile, MapMask mask )
    {
        foreach( var side in EnumUtil.Values<TileSide>() )
        {
            var rule = m_Rules[side].FirstOrDefault( r => r.IsApplicable( mask ) );
            SetSprite( tile, side, rule == null ? null : rule.SpriteName );
        }
    }

    void SetSprite( Tile tile, TileSide side, string spriteName )
    {
        var planeRenderer = tile.GetPlaneMeshRenderer( side );
        if( spriteName == null )
        {
            planeRenderer.enabled = false;
        }
        else
        {
            planeRenderer.enabled = true;
            var sheet = SpriteSheet( side );
            sheet.Apply( spriteName, planeRenderer );
        }
    }

    SpriteSheet SpriteSheet( TileSide side )
    {
        var sheet =
            side == TileSide.Top         ? RoofSprites        :
            side == TileSide.Floor       ? FloorSprites       :
            side == TileSide.Front       ? WallSprites        :
            side == TileSide.Underground ? UndergroundSprites :
            null;

        if( sheet == null )
        {
            throw new System.InvalidOperationException( string.Format(
                "Uknown tile side: {0}", side
            ) );
        }

        return sheet;
    }
}