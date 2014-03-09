using UnityEngine;
using System;
using System.Collections.Generic;

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
}
