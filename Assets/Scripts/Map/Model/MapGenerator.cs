using UnityEngine;
using System.Linq;

[System.Serializable]
public class MapGenerationOptions
{
    public int Width;
    public int Height;
}

public class MapGenerator : MonoBehaviour
{
    public Map GenerateMap( MapGenerationOptions options )
    {
        var map = new Map( options.Width, options.Height );

        for( int y = 0; y < map.Height; y++ )
        {
            for( int x = 0; x < map.Width; x++ )
            {
                var flootProb = 1.1f * Mathf.Pow(((float) Mathf.Min( x, map.Width - x - 1)) / map.Width, 0.2f);
                map.Tiles[x, y] = Random.value < flootProb ? TileType.Floor : TileType.Blocked;
            }
        }

        var startTiles = (from x in Enumerable.Range( 0, map.Width )
                          from y in Enumerable.Range( 0, 10 )
                          where map.Tiles[x, y] == TileType.Floor
                          select new GridPoint( x, y )).ToList();

        startTiles.Shuffle();

        map.Features.AddRange( new [] {
            new GridFeature { Point = startTiles[0], Feature = FeatureType.Player0Start },
            new GridFeature { Point = startTiles[1], Feature = FeatureType.Player1Start },
            new GridFeature { Point = startTiles[2], Feature = FeatureType.Player2Start },
            new GridFeature { Point = startTiles[3], Feature = FeatureType.Player3Start }
        } );

        return map;
    }
}