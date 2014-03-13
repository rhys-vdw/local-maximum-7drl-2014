using UnityEngine;
using UnityObjectRetrieval;

public class Missile : ExtendedMonoBehaviour
{
    public int Damage = 10;

    Vector3 m_Velocity = Vector3.zero;

    Rigidbody m_Rigidbody;
    Transform m_Transform;

    public void Fire( Vector3 fromPosition, Vector3 velocity )
    {
        m_Transform.position = fromPosition;
        m_Transform.LookAt( fromPosition + velocity );
        m_Velocity = velocity;
    }

    void Awake()
    {
        m_Transform = Component<Transform>();
        m_Rigidbody = Component<Rigidbody>();
    }

    void FixedUpdate()
    {
        m_Rigidbody.MovePosition( m_Transform.position + m_Velocity * Time.fixedDeltaTime );
    }

    void OnCollisionEnter( Collision collision )
    {
        var health = collision.transform.SelfAncestors().ComponentOrNull<Health>();
        if( health != null )
        {
            health.Current -= Damage;
        }

        Destroy( gameObject );
    }
}