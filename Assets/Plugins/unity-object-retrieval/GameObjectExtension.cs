using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UnityObjectRetrieval
{

public static class GameObjectExtension
{
    public static TComponent ComponentOrNull<TComponent>( this GameObject gameObject )
        where TComponent : class
    {
        return gameObject.GetComponent( typeof(TComponent) ) as TComponent;
    }

    public static TComponent Component<TComponent>( this GameObject gameObject )
        where TComponent : class
    {
        var c = gameObject.GetComponent( typeof(TComponent) ) as TComponent;
        if( c == null ) {
            throw new ComponentNotFoundException( typeof( TComponent ) );
        }
        return c;
    }

    public static IEnumerable<TComponent> Components<TComponent>( this GameObject gameObject )
        where TComponent : class
    {
        return gameObject.GetComponents( typeof(TComponent) ).Cast<TComponent>();
    }
}

}