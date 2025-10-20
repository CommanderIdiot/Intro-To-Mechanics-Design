
using System.Collections;
using System.Text;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] public Transform m_RaycastPosition;
    
	[SerializeField] public LayerMask m_GroundLayer;

    private PlayerControls m_ActionMap;

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

    public void JumpPerformed() //Nullable float? Not sure.
    {
        if (b_IsGrounded || m_CoyoteTimeCounter > 0)
        {
            /*switch (m_JumpPowerTimer) //Get the passed number for how long the jump was pressed. Then change power of the jump accordingly.
            {
                case <= 0.5f:
                    m_RB.AddForce(Vector2.up * (0.5f * m_JumpStrength), ForceMode2D.Impulse);
                    break;
                case >= 0.5f:
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    break;
            }*/
            
            //Need to account for falling and coyote jump being executable during this time.
                
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_CoyoteTimeCounter = 0;
            m_FrameTimeThreashold = 0;
            
            Debug.Log("Jump function caused super jump.");
            
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
                
                StopCoroutine((C_CoyoteJumpCoroutine()));

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
                    
                    Debug.Log("Jump Coroutine caused super jump.");
                    JumpPerformed();
                    
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

    private void FixedUpdate()
	{
        b_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
	}
}
