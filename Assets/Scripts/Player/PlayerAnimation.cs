using UnityEngine;
using UnityObjectRetrieval;
using IEnumerator = System.Collections.IEnumerator;
using System;

public class PlayerAnimation : ExtendedMonoBehaviour
{
    Animation m_Animation;

    void Awake()
    {
        //m_Animation = SelfDescendants().Component<Animation>();
    }

    public void Play( string animationName, float duration, Action onComplete = null )
    {
        Play( animationName, onComplete );
        /*
        float speed = m_Animation[animationName].length / duration;
        Debug.Log( "speed = " + speed );
        m_Animation[animationName].speed = speed;
        Play( animationName, onComplete );
        */
    }

    public void Play( string animationName, Action onComplete = null )
    {
        onComplete();
        /*
        if( onComplete != null )
        {
            StartCoroutine( PlayAnimationCoroutine( animationName, onComplete ) );
        }
        else
        {
            m_Animation.Play( animationName );
        }
        */
    }

    IEnumerator PlayAnimationCoroutine( string animationName, Action onComplete )
    {
        m_Animation.Play( animationName );
        yield return new WaitForSeconds( m_Animation[ animationName ].length );
        onComplete();
    }
}