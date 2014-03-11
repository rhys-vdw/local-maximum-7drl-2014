using UnityEngine;
using UnityObjectRetrieval;
using IEnumerator = System.Collections.IEnumerator;

public class SwordDealDamageOnAttack : ExtendedMonoBehaviour
{
    public int Damage = 30;
    public float Range = 1f;
    public float SwingWidth = 0.8f;
    public float HandOffset = 0f;
    public float DamageDelay = 0f;
    public float DamageDuration = 0.2f;

    Collider m_Collider;

    void Awake()
    {
        var item = Component<Item>();
        item.EquipEvent += HandleEquip;

        var sword = Component<Sword>();
        sword.AttackEvent += HandleAttack;
    }

    void OnDestroy()
    {
        if( m_Collider != null )
        {
            Destroy( m_Collider.gameObject );
        }
    }

    // Message handlers.

    void HandleEquip( PlayerHand hand )
    {
        if( m_Collider == null )
        {
            m_Collider = CreateCollider();
        }
        var position = m_Collider.transform.localPosition;
        position.x = (hand.Side == HandSide.Right ? 1 : -1) * HandOffset;
        m_Collider.transform.localPosition = position;
    }

    void HandleAttack( Sword sword )
    {
        StartCoroutine( AttackCoroutine() );
    }

    // Unity message handlers.

    void OnForwardedTriggerEnter( Collider collider )
    {
        /*
        var damageReceiver = collision.transform.SelfDescendants().ComponentOrNull<DamageReceiver>();
        damageReceiver.Damage( Damage );
        */
        var health = collider.SelfDescendants().ComponentOrNull<Health>();
        if( health != null )
        {
            health.Current -= Damage;
        }
    }

    // Private helpers.

    IEnumerator AttackCoroutine()
    {
        if( m_Collider.gameObject.activeSelf )
        {
            Debug.LogWarning( "Tried to deal damage while already active", this );
            yield break;
        }

        yield return new WaitForSeconds( DamageDelay );
        m_Collider.gameObject.SetActive( true );
        yield return new WaitForSeconds( DamageDuration );
        m_Collider.gameObject.SetActive( false );
    }

    Collider CreateCollider()
    {
        var playerAim = Ancestors().Component<PlayerAim>();

        var boxObject = new GameObject();

        var boxTransform = boxObject.transform;
        boxTransform.parent = playerAim.Body.transform;
        boxTransform.localPosition = Vector3.zero;
        boxTransform.localRotation = Quaternion.identity;

        var boxCollider = boxObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3( SwingWidth, 2f, Range );
        boxCollider.center = new Vector3( 0f, 1f, Range / 2 );
        boxCollider.isTrigger = true;

        boxObject.AddComponent<Rigidbody>().isKinematic = true;

        var forwarder = boxObject.AddComponent<CollisionMessageForwarder>();
        forwarder.Target = this.gameObject;

        boxObject.SetActive( false );

        return boxCollider;
    }
}