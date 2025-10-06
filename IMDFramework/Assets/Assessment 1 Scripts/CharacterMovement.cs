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

    private Coroutine m_MoveCoroutine;
    private bool m_IsMoveActive;
    
    private Coroutine m_CoyoteJumpCoroutine;
    private Coroutine m_JumpBufferCoroutine;
    
    private void Awake()
    {
        m_ActionMap = new PlayerControls();

		m_RB = GetComponent<Rigidbody2D>();
    }
    
    #region InputFunctions

    public void SetInMove(float direction)
    {
        m_InMove = direction;

        if (m_InMove == 0)
        {
            m_IsMoveActive = false;
        }
        else
        {
            if(m_IsMoveActive) { return;}
            
            m_IsMoveActive = true;

            m_MoveCoroutine = StartCoroutine(C_MovementUpdate());
        }
    }

    public void JumpPerformed()
    {
        if (m_IsGrounded || m_CoyoteTimeCounter > 0)
        {
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_CoyoteTimeCounter = 0;
            
            /*Add coyote jump coroutine here*/
            Debug.Log("Coroutine started");
            StartCoroutine(C_CoyoteJumpCoroutine());
        }
        /*Add jump buffering coroutine here*/
    }
    #endregion

    #region Coroutines

    private IEnumerator C_MovementUpdate()
    {
        while (m_IsMoveActive)
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
            }
            else
            {
                m_CoyoteTimeCounter -= Time.deltaTime;
            }
            yield return new WaitForSeconds(0.1f);
        }
        /* Try to do this:
         1. Make variable jump height where a timer runs when jump is held. If held for 0.5 seconds or less, do the base jump height and for more than 0.5 seconds double the jump height.
         2. Figure out how to do an anti-gravity apex.
         */
    }

    private IEnumerator JumpBufferCoroutine()
    {
        /*while (true)
        {
           Jump buffer ideas:
           if (is grounded)
                jump
            else

        }*/

    #endregion

        yield return new WaitForSeconds(0.1f);
    }
    
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
