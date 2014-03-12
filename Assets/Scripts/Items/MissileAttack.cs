using UnityEngine;
using UnityObjectRetrieval;

public class MissileAttack : ExtendedMonoBehaviour
{
    public int Damage = 10;
    public float Speed = 10f;
    public float AppearOffset = 0f;
    public float AppearHeight = 0.5f;

    public Missile MissilePrefab;
    public float SpreadAngle = 0f;

    Transform m_AimTransform;

    // Unity messages.

    void Start()
    {
        m_AimTransform = Ancestors().Component<PlayerAim>().Body;
    }

    public void Attack()
    {
        var missile = Instantiate( MissilePrefab ) as Missile;

        var direction = Vector3.Lerp( Random.insideUnitSphere, m_AimTransform.forward, 1f - SpreadAngle / 360f );
        var velocity = direction * Speed;

        missile.Damage = Damage;

        var start = m_AimTransform.position +
            m_AimTransform.forward * AppearOffset +
            Vector3.up * AppearHeight;
    
        missile.Fire( start, velocity );
    }
}