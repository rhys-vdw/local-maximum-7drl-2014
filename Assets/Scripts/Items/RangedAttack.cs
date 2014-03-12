using UnityEngine;
using UnityObjectRetrieval;

public class WeaponMissileAttack : ExtendedMonoBehaviour
{
    public int Damage = 10;
    public float Speed = 10f;

    public Missile MissilePrefab;
    public float SpawnHeight = 1f;
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
        missile.Fire( m_AimTransform.position, velocity );
    }
}