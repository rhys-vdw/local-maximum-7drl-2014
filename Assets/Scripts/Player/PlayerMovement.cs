using UnityEngine;
using UnityObjectRetrieval;

public class PlayerMovement : ExtendedMonoBehaviour
{
    public float m_WalkSpeed = 2f;
    public float m_RunSpeed = 4f;

    PlayerInput m_Input;
    CharacterController m_Controller;

    void Awake()
    {
        m_Input = Component<PlayerInput>();
        m_Controller = Component<CharacterController>();
    }

    void FixedUpdate()
    {
        var isRunning = m_Input.GetKey( PlayerKey.Run );

        var speed = isRunning ? m_RunSpeed : m_WalkSpeed;

        var movement = m_Input.Movement();

        var step = new Vector3(
            movement.x * speed * Time.fixedDeltaTime,
            0f,
            movement.y * speed * Time.fixedDeltaTime
        );

        m_Controller.Move( step );
    }
}