using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class IEnumerableTransformExtension
{
    public static Transform ClosestOrNull( this IEnumerable<Transform> transforms, Vector3 position )
    {
        float min = Mathf.Infinity;
        Transform closest = null;

        foreach( var transform in transforms )
        {
            var distance = (transform.position - position).sqrMagnitude;
            if( distance < min )
            {
                min = distance;
                closest = transform;
            }
        }
        
        return closest;
    }
}