using UnityEngine;

public interface IInputMapping
{
    bool GetKeyDown( PlayerKey key );
    bool GetKey( PlayerKey key );
    bool GetKeyUp( PlayerKey key );

    Vector2 Movement();
    Vector2 Aim();
}