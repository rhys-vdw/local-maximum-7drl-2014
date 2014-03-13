using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using Pathfinding;

public class PathManager : MonoBehaviour
{
    void Awake()
    {
        Scene.Object<MapBuilder>().BuildCompleteEvent += HandleMapBuildComplete;
    }

    void HandleMapBuildComplete( MapBuilder mapBuilder )
    {
        var data = AstarPath.active.astarData;
        //var gridGraph = data.AddGraph( typeof( GridGraph ) ) as GridGraph;
        var gridGraph = data.gridGraph;

        gridGraph.width = mapBuilder.Columns;
        gridGraph.depth = mapBuilder.Rows;

        var tileSize = mapBuilder.TileSize;
        gridGraph.nodeSize = tileSize;

        var center = mapBuilder.Center;
        if( center.x % tileSize == 0 ) center.x -= tileSize / 2;
        if( center.z % tileSize == 0 ) center.z -= tileSize / 2;
        gridGraph.center = center;

        gridGraph.UpdateSizeFromWidthDepth();

        AstarPath.active.Scan();
    }
}
