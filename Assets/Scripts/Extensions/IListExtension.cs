using UnityEngine;
using System.Collections.Generic;
using System;

public static class IListExtension
{
    public static T RandomElement<T>( this IList<T> source )
    {
        if( source == null )
        {
            throw new ArgumentNullException( "source is null" );
        }

        if( source.Count == 0 )
        {
            throw new InvalidOperationException( "The source sequence is empty." );
        }

        return source[ UnityEngine.Random.Range( 0, source.Count ) ];
    }

    public static void Shuffle<T>( this IList<T> list )
    {
        if( list == null )
        {
            throw new ArgumentNullException( "source is null" );
        }

        int n = list.Count;
        while (n > 1) 
        {
            int k = UnityEngine.Random.Range( 0, n-- );
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }
}
