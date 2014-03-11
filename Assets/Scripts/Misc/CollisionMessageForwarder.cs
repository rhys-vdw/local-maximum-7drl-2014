using UnityEngine;

public class CollisionEventForwarder : MonoBehaviour
{
    public GameObject Target;

    void OnCollisionEnter( Collision collision )
    {
        if( Target != null )
            Target.SendMessage( "OnForwardedCollisionEnter", collision );
    }

    void OnCollisionStay( Collision collision )
    {
        if( Target != null )
            Target.SendMessage( "OnForwardedCollisionStay", collision );
    }

    void OnCollisionExit( Collision collision )
    {
        if( Target != null )
            Target.SendMessage( "OnForwardedCollisionExit", collision );
    }
}