using UnityEngine;
using UnityObjectRetrieval;

public class PlayerAim : ExtendedMonoBehaviour
{
    public Transform m_Body;
    public float RotationSpeed = 720f;
    public float MinAim = 0.2f;
    PlayerInput m_Input;
    PlayerMovement m_Movement;

    void Awake()
    {
        m_Input = Component<PlayerInput>();
        m_Movement = Component<PlayerMovement>();
    }

    void FixedUpdate()
    {
        var aim = m_Movement.IsRunning || m_Input.Aim().magnitude < MinAim
            ? m_Input.Movement()
            : m_Input.Aim();

        if( aim != Vector2.zero )
        {
            m_Body.rotation = Quaternion.RotateTowards(
                m_Body.rotation,
                Quaternion.LookRotation( new Vector3(
                    aim.x, 0f, aim.y
                ) ),
                RotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}