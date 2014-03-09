using UnityEngine;
using System.Collections.Generic;
using System;

namespace EventTools
{

public class EventMap<TKey>
{
    Dictionary<TKey, Action> m_EventMap = new Dictionary<TKey, Action>();

    public void AddHandler( TKey eventKey, Action handler )
    {
        if( m_EventMap.ContainsKey( eventKey ) )
        {
            m_EventMap[eventKey] += handler;
        }
        else
        {
            m_EventMap.Add( eventKey, handler );
        }
    }

    public void RemoveHandler( TKey eventKey, Action handler )
    {
        if( m_EventMap.ContainsKey( eventKey ) )
        {
            var existing = (m_EventMap[eventKey] -= handler);
            if( existing == null )
            {
                m_EventMap.Remove( eventKey );
            }
        }
    }

    public void FireEvent( TKey eventKey )
    {
        Action handler;
        if( m_EventMap.TryGetValue( eventKey, out handler ) )
        {
            handler();
        }
    }

#if DEBUG
    public int HandlerCount( TKey eventKey )
    {
        Action handler;
        if( m_EventMap.TryGetValue( eventKey, out handler ) )
        {
            return handler.GetInvocationList().Length;
        }
        return 0;
    }
#endif
}

public class EventMap<TKey, TArgument>
{
    Dictionary<TKey, Action<TArgument>> m_EventMap = new Dictionary<TKey, Action<TArgument>>();

    public void AddHandler( TKey eventKey, Action<TArgument> handler )
    {
        if( m_EventMap.ContainsKey( eventKey ) )
        {
            m_EventMap[eventKey] += handler;
        }
        else
        {
            m_EventMap.Add( eventKey, handler );
        }
    }

    public void RemoveHandler( TKey eventKey, Action<TArgument> handler )
    {
        if( m_EventMap.ContainsKey( eventKey ) )
        {
            var existing = (m_EventMap[eventKey] -= handler);
            if( existing == null )
            {
                m_EventMap.Remove( eventKey );
            }
        }
    }

    public void FireEvent( TKey eventKey, TArgument arg )
    {
        Action<TArgument> handler;
        if( m_EventMap.TryGetValue( eventKey, out handler ) )
        {
            handler( arg );
        }
    }

#if DEBUG
    public int HandlerCount( TKey eventKey )
    {
        Action<TArgument> handler;
        if( m_EventMap.TryGetValue( eventKey, out handler ) )
        {
            return handler.GetInvocationList().Length;
        }
        return 0;
    }
#endif
}

}