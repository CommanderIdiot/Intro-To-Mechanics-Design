using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] private Transform m_RaycastPosition;
	[SerializeField] private LayerMask m_GroundLayer;
	private float m_InMove;
    private PlayerControls m_ActionMap;
    private bool m_IsGrounded;

    [SerializeField] private float m_CoyoteTimeThreashold = 0.4f;
    private float m_CoyoteTimeCounter;

    
    private Coroutine m_CoyoteJumpCoroutine;
    private Coroutine m_JumpBufferCoroutine;
    
    private void Awake()
    {
        m_ActionMap = new PlayerControls();

		m_RB = GetComponent<Rigidbody2D>();
    }

    #region Bindings
    private void OnEnable()
    {
        m_ActionMap.Enable();

        m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
    }
    #endregion

    #region InputFunctions

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        m_InMove = context.ReadValue<float>();
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        m_InMove = 0f;
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        if (m_IsGrounded || m_CoyoteTimeCounter > 0)
        {
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_CoyoteTimeCounter = 0;
            
            /*Add coyote jump coroutine here*/
            Debug.Log("Coroutine started");
            StartCoroutine(CoyoteJumpCoroutine());
        }
        /*Add jump buffering coroutine here*/
    }
    #endregion

    private IEnumerator CoyoteJumpCoroutine()
    {
        while (true)
        {
            if (m_IsGrounded)
            {
                m_CoyoteTimeCounter = m_CoyoteTimeThreashold;
                StopCoroutine((CoyoteJumpCoroutine()));
            }
            else
            {
                m_CoyoteTimeCounter -= Time.deltaTime;
            }
            yield return new WaitForSeconds(0.1f);
        }
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
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
	}
}
