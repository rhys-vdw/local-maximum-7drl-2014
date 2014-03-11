using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

[System.Serializable]
public class ShakeConfig
{
    public float Duration = 0.2f;
    public float MinMagnitude = 0.02f;
    public float MaxMagnitude = 0.2f;
    public GoShakeType ShakeType = GoShakeType.Position;
    public GoEaseType EaseType = GoEaseType.Linear;
}

public class ShakeOnDamage : ExtendedMonoBehaviour
{
    public ShakeConfig Config;
    public Transform OptionalTarget = null;

    Transform m_Target;
    Vector3 m_StartPosition;
    GoTween m_Tween;

    void Start()
    {
        Component<Health>().ChangeEvent += HandleHealthChange;
        m_Target = OptionalTarget ?? transform;
        m_StartPosition = m_Target.localPosition;
    }

    void HandleHealthChange( Health health, int change )
    {
        if( change < 0 )
        {
            if( m_Tween != null ) CancelTween();

            Debug.Log( "Health is now: " + health.Current );

            var magnitude = Mathf.Lerp( Config.MaxMagnitude, Config.MinMagnitude, (float) health.Current / health.Max );

            m_Tween = Go.to( m_Target, Config.Duration, new GoTweenConfig()
                .shake( new Vector3( magnitude, 0, magnitude ), Config.ShakeType, 1, true )
                .setEaseType( Config.EaseType )
                .onComplete( t => m_Target.localPosition = m_StartPosition )
            );
        }
    }

    void CancelTween()
    {
        m_Tween.destroy();
        m_Tween = null;
        m_Target.localPosition = m_StartPosition;
    }
}
