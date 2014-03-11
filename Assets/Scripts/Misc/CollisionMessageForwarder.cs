using UnityEngine;

public class CollisionMessageForwarder : MonoBehaviour
{
    public GameObject Target;

    void OnCollisionEnter( Collision collision )
    {
        if( Target != null )
            Message( "OnForwardedCollisionEnter", collision );
    }

    void OnCollisionStay( Collision collision )
    {
        if( Target != null )
            Message( "OnForwardedCollisionStay", collision );
    }

    void OnCollisionExit( Collision collision )
    {
        if( Target != null )
            Message( "OnForwardedCollisionExit", collision );
    }

    void OnTriggerEnter( Collider collider )
    {
        if( Target != null )
            Message( "OnForwardedTriggerEnter", collider );
    }

    void OnTriggerStay( Collider collider )
    {
        if( Target != null )
            Message( "OnForwardedTriggerStay", collider );
    }

    void OnTriggerExit( Collider collider )
    {
        if( Target != null )
            Message( "OnForwardedTriggerExit", collider );
    }

    void Message( string message, object argument )
    {
        Target.SendMessage( message, argument, SendMessageOptions.DontRequireReceiver );
    }
}