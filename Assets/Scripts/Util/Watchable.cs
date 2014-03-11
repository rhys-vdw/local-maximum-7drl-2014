using System;

namespace EventTools {

public struct Watchable<T>
{
    T m_Value;
    event Action<T> m_NotifyEvent;
    public bool IsInitialized { get; private set; }

    public Watchable( T value )
    {
        m_Value = value;
        m_NotifyEvent = null;
        IsInitialized = true;
    }

    public T Value
    {
        get
        {
            return m_Value;
        }

        set
        {
            if( ! IsInitialized )
            {
                IsInitialized = true;
            }

            m_Value = value;
            Notify();
        }
    }

    public void Notify()
    {
        if( ! IsInitialized ) throw new InvalidOperationException(
            "Unintialized Watchable cannot Notify."
        );

        if( m_NotifyEvent != null )
        {
            m_NotifyEvent( m_Value );
        }
    }

    public void Watch( Action<T> notifyHandler )
    {
        m_NotifyEvent += notifyHandler;
        if( IsInitialized )
        {
            notifyHandler( m_Value );
        }
    }

    public void Unwatch( Action<T> notifyHandler )
    {
        m_NotifyEvent -= notifyHandler;
    }

    public void Update( Action<T> updater )
    {
        if( ! IsInitialized ) throw new InvalidOperationException(
            "Cannot update an unintialized Watchable!"
        );

        updater( m_Value );
        Notify();
    }

    static Watchable<T> Create( T value )
    {
        return new Watchable<T>( value );
    }

    static public implicit operator T( Watchable<T> watchable )
    {
        return watchable.m_Value;
    }
}

}