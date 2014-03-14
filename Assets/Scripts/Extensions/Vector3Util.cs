using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public static class Vector3Util
{
    public static Vector3 SprayDirection( Vector3 direction, float angle )
    {
        return Vector3.Slerp(
            direction,
            Random.onUnitSphere,
            angle / 180f
        );
    }

    public static float DistanceSquared( Vector3 from, Vector3 to )
    {
        return ( to - from ).sqrMagnitude;
    }
}
