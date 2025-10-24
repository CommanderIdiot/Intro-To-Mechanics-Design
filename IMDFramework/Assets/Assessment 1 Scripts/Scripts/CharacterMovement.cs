using System;
using System.Collections;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;
    
	[SerializeField] public LayerMask m_GroundLayer;

    private PlayerControls m_ActionMap;
    
    private GroundDetector m_GroundDetector;

    //Coroutines.
    private Coroutine m_CoyoteJumpCoroutine;
    
    private Coroutine m_JumpBufferCoroutine;
    
    private Coroutine m_MoveCoroutine;
    
    //Bools.
    private bool b_IsGrounded;
    
    private bool b_IsCoyoteCoroutineActive;
    
    private bool b_IsJumpActive = true;

    private bool b_IsJumpBufferActive = false;
    
    private bool b_IsMoveActive;
    
    //Floats
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_JumpStrength;
    private float m_InMove;
    
    [SerializeField] private float m_CoyoteTimeThreashold = 0.4f;
    [SerializeField] float m_JumpPowerTimer;
    private float m_CoyoteTimeCounter;
    private float m_FrameTimeThreashold = 0;

    [SerializeField] private float m_JumpBufferThreashold = 0.4f;
    private float m_JumpBufferTimeCounter;

    #region Awake/Start/OnDestroy Functions
    private void Awake()
    {
        m_ActionMap = new PlayerControls();

        m_GroundDetector = GetComponent<GroundDetector>();
        
        m_RB = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        m_GroundDetector.OnGroundContact += Handle_OnGroundContact;
    }
    private void OnDestroy()
    {
        m_GroundDetector.OnGroundContact -= Handle_OnGroundContact;
    }

    #endregion
    
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

    public void JumpPerformed(float? mJumpPressedLength)
    {
        if (b_IsGrounded || m_CoyoteTimeCounter > 0)
        {
            switch (m_JumpPowerTimer) //Get the passed number for how long the jump was pressed. Then change power of the jump accordingly.
            {
                case <= 0.5f:
                    m_RB.AddForce(Vector2.up * (0.5f * m_JumpStrength), ForceMode2D.Impulse);
                    break;
                case >= 0.5f:
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    break;
            }
            
            //have the ground sensor called in here
            
            //Need to account for falling and coyote jump being executable during this time.
                
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_CoyoteTimeCounter = 0;
            m_FrameTimeThreashold = 0;
            
            //Performs if the coyote time is not active while jump is active
            if (!b_IsCoyoteCoroutineActive && b_IsJumpActive)
            {
                b_IsCoyoteCoroutineActive = true;
                
                b_IsJumpActive = false;
                
                StartCoroutine(C_CoyoteJumpCoroutine());
            }
        }
        //Performs if the jump buffer is not active
        else if (!b_IsJumpBufferActive)
        {
            b_IsJumpBufferActive = true;
            
            StartCoroutine(C_JumpBufferCoroutine());
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
            //Make it run off a boolean instead.
            if (m_FrameTimeThreashold > 0.5 && b_IsGrounded && b_IsCoyoteCoroutineActive)
            {
                m_CoyoteTimeCounter = m_CoyoteTimeThreashold;
                
                b_IsJumpActive = true;
                
                b_IsCoyoteCoroutineActive = false;
                
                StopCoroutine(C_CoyoteJumpCoroutine());

                break;
            }
            
            m_CoyoteTimeCounter -= Time.deltaTime;
            m_FrameTimeThreashold += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator C_JumpBufferCoroutine()
    {
        while (true)
        {
            if (m_JumpBufferTimeCounter <= m_JumpBufferThreashold)
            {
                if (b_IsGrounded && b_IsJumpActive)
                {
                    m_JumpBufferTimeCounter = 0;
                    
                    b_IsJumpActive = true;
                    
                    b_IsCoyoteCoroutineActive = false;
                    b_IsJumpBufferActive = false;
                    
                    JumpPerformed(null);
                    
                    StopCoroutine(C_JumpBufferCoroutine());
                    
                    break; 
                }
            }
            else if (m_JumpBufferTimeCounter >= m_JumpBufferThreashold)
            {
                m_JumpBufferTimeCounter = 0;
                
                b_IsJumpBufferActive = false;
                
                StopCoroutine(C_JumpBufferCoroutine());
                
                break;
            }
            
            m_JumpBufferTimeCounter += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion 

    #region Events
    private void Handle_OnGroundContact(bool obj)
    {
        b_IsGrounded = m_GroundDetector.b_IsGrounded;
    }
    #endregion
}

/* Ideas for jump mechanics in movement:
 * 1. Jump performed after it gets triggered starts a coroutine that handles the jump in all stages.
 * 2. Adjusting size of the collider at the start of the jump to be smaller. Maybe need more colliders to do this or adjust the sizing of the only one.
 * 3. Adjusting the size of the collider towards the end of the fall to try and make people catch onto platforms they fall onto.
 * 4. Reset the collider size back to normal.
 * 5. The falling part of it gives the character sticky feet when falling.
 * 6. Figure out how to make the jump add a small amount of force for how long they have it held for.
 * 7. Crouching on ledges seems to be just detect if you're on a ledge should probably be on a different script.
 */
