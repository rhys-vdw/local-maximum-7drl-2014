using UnityEngine;
using UnityObjectRetrieval;

public class PlayerAim : ExtendedMonoBehaviour
{
    public Transform Body;
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
            Body.rotation = Quaternion.RotateTowards(
                Body.rotation,
                Quaternion.LookRotation( new Vector3(
                    aim.x, 0f, aim.y
                ) ),
                RotationSpeed * Time.fixedDeltaTime
            );
        }
    }
}