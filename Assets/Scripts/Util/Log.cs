using UnityEngine;
using UObject = UnityEngine.Object;
using Object = System.Object;

public static class Console
{
    public static void Log( string str, UObject obj = null )
    {
        Debug.Log( str, obj );
    }

    public static void Logf( string format, params Object[] objects )
    {
        Debug.Log( string.Format( format, objects ) );
    }
}