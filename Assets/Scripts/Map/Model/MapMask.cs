using System;
using System.Collections.Generic;

public class MapMask
{
    int m_Left;
    int m_Top;
    TileType[,] m_Map;

    public MapMask( TileType[,] map, int left = 0, int top = 0 )
    {
        m_Left = left;
        m_Top = top;
        m_Map = map;
    }

    public TileType Center
    {
        get { return this[1,1]; }
    }

    public TileType this[ int x, int y ]
    {
        get
        {
            if( x < 0 || x > 2 || y < 0 || y > 2  ||
                x + m_Left < 0 || x + m_Left >= m_Map.GetLength( 0 ) ||
                y + m_Top  < 0 || y + m_Top  >= m_Map.GetLength( 1 ) )
            {
                return TileType.OutOfBounds;
            }
            return m_Map[ x + m_Left, y + m_Top ];
        }
    }

    public void Each( Action<int, int, TileType> operation )
    {
        for( int y = 0; y < 3; y++ )
        {
            for( int x = 0; x < 3; x++ )
            {
                operation( x, y, this[x, y] );
            }
        }
    }

    public override string ToString()
    {
        List<string> args = new List<string>();
        Each( (x, y, type) => { args.Add( type.ToString() ); } );
        args.Add( m_Left.ToString() );
        args.Add( m_Top.ToString() );

        return string.Format(
@"Mask. Corner: ({9}, {10})
{0}, {1}, {2},
{3}, {4}, {5},
{6}, {7}, {8}", args.ToArray()
        );
    }

    /*
    public override int GetHashCode()
    {
        int hash = 17;
        for( int y = 0; y < 2; y++ )
        {
            for( int x = 0; x < 2; x++ )
            {
                hash = hash * 23 + this[x,y].GetHashCode();
            }
        }
        return hash;
    }

    public override bool Equals( object o )
    {
        var other = o as MapMask;

        if( other == null )
        {
            return false;
        }

        for( int y = 0; y < 2; y++ )
        {
            for( int x = 0; x < 2; x++ )
            {
                var a = other[x,y];
                var b = this[x,y];

                if( a != b && a != TileType.Any && b != TileType.Any )
                {
                    return false;
                }
            }
        }

        return true;
    }
    */
}

/* Just leaving this here for safe keeping.

    TileType[,] CopyMask( TileType[,] source, int x, int y)
    {
        x--;
        y--;
        
        TileType[,] mask = new TileType[3,3];
        int startX = Mathf.Max( 0, x );
        int startY = Mathf.Max( 0, y );
        int endX = Mathf.Min( source.GetLength( 0 ), x + 3 );
        int endY = Mathf.Min( source.GetLength( 1 ), y + 3 );

        for( int u = startX; u < endX; u++ )
        {
            for( int v = startY; v < endY; v++ )
            {
                mask[u - x, v - y] = source[ u, v ];
            }
        }

        return mask;
    }
    */

