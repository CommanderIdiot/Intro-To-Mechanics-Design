using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] public Transform m_RaycastPosition;
	[SerializeField] public LayerMask m_GroundLayer;
	private float m_InMove;
    private PlayerControls m_ActionMap;
    private bool m_IsGrounded;

    [SerializeField] private float m_CoyoteTimeThreashold = 0.4f;
    private float m_CoyoteTimeCounter;
    private bool b_IsCoyoteCoroutineActive;

    [SerializeField] private float m_JumpBufferThreashold = 0.4f;
    private float m_JumpBufferTimeCounter;
    
    private Coroutine m_MoveCoroutine;
    private bool b_IsMoveActive;
    private bool b_IsJumpActive = true;
    
    private Coroutine m_CoyoteJumpCoroutine;
    private Coroutine m_JumpBufferCoroutine;
    
    private void Awake()
    {
        m_ActionMap = new PlayerControls();

		m_RB = GetComponent<Rigidbody2D>();
    }
    
    #region Move/Jump Functions

    public void SetInMove(float direction)
    {
        m_InMove = direction;

        if (m_InMove == 0)
        {
            b_IsMoveActive = false;
        }
        else
        {
            if(b_IsMoveActive) { return;}
            
            b_IsMoveActive = true;

            m_MoveCoroutine = StartCoroutine(C_MovementUpdate());
        }
    }

    public void JumpPerformed()
    {
        if (m_IsGrounded || m_CoyoteTimeCounter > 0)
        {
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_CoyoteTimeCounter = 0;

            if (!b_IsCoyoteCoroutineActive && b_IsJumpActive)
            {
                Debug.Log("Coyote time coroutine started");
                StartCoroutine(C_CoyoteJumpCoroutine());
                b_IsJumpActive = false;
            }
            else if (b_IsCoyoteCoroutineActive && !b_IsJumpActive)
            {
                Debug.Log("Jump buffer coroutine started");
                StartCoroutine(C_JumpBufferCoroutine());
            }
        }
    }
    
    #endregion

    #region Coroutines

    private IEnumerator C_MovementUpdate()
    {
        while (b_IsMoveActive)
        {
            yield return new WaitForFixedUpdate();
            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
        }
    }
    
    private IEnumerator C_CoyoteJumpCoroutine()
    {
        while (true)
        {
            if (m_IsGrounded)
            {
                m_CoyoteTimeCounter = m_CoyoteTimeThreashold;
                StopCoroutine((C_CoyoteJumpCoroutine()));
                b_IsJumpActive = true;
                Debug.Log("Coyote time coroutine stopped");
                break;
            }
            else
            {
                m_CoyoteTimeCounter -= Time.deltaTime;
            }

            yield return new WaitForFixedUpdate();
        }
        /* Try to do this:
         1. Make variable jump height where a timer runs when jump is held. If held for 0.5 seconds or less, do the base jump height and for more than 0.5 seconds double the jump height.
         2. Figure out how to do an anti-gravity apex.
         */
    }

    private IEnumerator C_JumpBufferCoroutine()
    {
        // RETHINK THIS, maybe "if not grounded, set a timer to be active until it is grounded. THEN check to see if this is under a limit, then jump/not jump.
        while (true)
        {
            if (!m_IsGrounded)
            {
                m_JumpBufferTimeCounter = m_JumpBufferThreashold;
                StopCoroutine((C_JumpBufferCoroutine()));
                Debug.Log("Jump buffer coroutine stopped.");
                break;
            }
            else
            {
                m_JumpBufferTimeCounter -= Time.deltaTime;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
    
    /*private void Update()
    {
        /* Potentially have it set as coroutines or timers to avoid using update?
        Also, possibly do similar for jump buffering, but will only jump if the jump key was pressed within an x time limit?

        if (m_IsGrounded)
        {
            m_CoyoteTimeCounter = m_CoyoteTimeThreashold;
        }
        else
        {
            m_CoyoteTimeCounter -= Time.deltaTime;
        }
    }*/

    private void FixedUpdate()
	{
        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
	}
}
