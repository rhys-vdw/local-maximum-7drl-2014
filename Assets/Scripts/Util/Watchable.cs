using System;

namespace EventTools {

public struct Watchable<T>
{
    T m_Value;
    event Action<T> m_NotifyEvent;
    bool m_IsInitialized;

    public Watchable( T value )
    {
        m_Value = value;
        m_NotifyEvent = null;
        m_IsInitialized = true;
    }

    public T Value
    {
        get
        {
            return m_Value;
        }

        set
        {
            if( ! m_IsInitialized )
            {
                m_IsInitialized = true;
            }

            m_Value = value;
            Notify();
        }
    }

    public void Notify()
    {
        if( ! m_IsInitialized ) throw new InvalidOperationException(
            "Cannot Notify an unintialized Watchable!"
        );

        if( m_NotifyEvent != null )
        {
            m_NotifyEvent( m_Value );
        }
    }

    public void Watch( Action<T> notifyHandler )
    {
        m_NotifyEvent += notifyHandler;
        if( m_IsInitialized )
        {
            notifyHandler( m_Value );
        }
    }

    public void Unwatch( Action<T> notifyHandler )
    {
        m_NotifyEvent -= notifyHandler;
    }

    static public implicit operator T( Watchable<T> watchable )
    {
        return watchable.m_Value;
    }

    static Watchable<T> Create( T value )
    {
        return new Watchable<T>( value );
    }
}

}