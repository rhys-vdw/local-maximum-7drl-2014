using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public struct GridFeature
{
    public GridPoint Point;
    public FeatureType Feature;
}

public class Map
{
    public TileType[,] Tiles;
    public List<GridFeature> Features;

    public Map( int width, int height )
    {
        Tiles = new TileType[width, height];
        Features = new List<GridFeature>();
    }

    public int Width
    {
        get { return Tiles.GetLength( 0 ); }
    }

    public int Height
    {
        get { return Tiles.GetLength( 1 ); }
    }

    public string ToVisualString()
    {
        var builder = new StringBuilder();

        for( int y = 0; y < Tiles.GetLength( 1 ); y++ )
        {
            for( int x = 0; x < Tiles.GetLength( 0 ); x++ )
            {
                builder.Append(
                    Tiles[x,y] == TileType.Blocked ? '#' :
                    Tiles[x,y] == TileType.Floor   ? '.' :
                    'X'
                );
            }
            builder.Append( '\n' );
        }

        return builder.ToString();
    }
}
