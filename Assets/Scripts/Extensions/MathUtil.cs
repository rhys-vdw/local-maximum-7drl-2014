using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public static class MathUtil
{
    public static int Mod( int x, int m )
    {
        return ( x % m + m ) % m;
    } 

    public static float Mod( float x, float m )
    {
        return ( x % m + m ) % m;
    } 
}
