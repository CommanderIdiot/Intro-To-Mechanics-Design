using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region Variables
    [Header("Layer Masks")]
    [SerializeField] public LayerMask m_GroundLayer;
    
    [Header("Movement Variables")]
    [SerializeField] private float m_MoveSpeed;
    
    [SerializeField] private int m_WalkingSoundID;
    
    private float m_InMove;
    private bool b_IsMoveActive;
    
    [Header("Jump Variables")]
    [SerializeField] private float m_JumpStrength;
    [SerializeField] private float m_JumpPowerTimer;
    
    [SerializeField] private int m_JumpingSoundID;
    [SerializeField] private int m_LandingSoundID;
    
    private float m_MinimumAmountFramesSinceOffGround;
    
    private bool b_IsGrounded;
    private bool b_IsJumpActive = true;
    private bool b_IsCheckingFallActive;
    private bool b_IsFalling;
    
    [SerializeField] private int m_AscendLengthCounter;
    private int m_CurrentAscendLength;
    
    [SerializeField] private int m_JumpApexLengthCounter;
    private int m_CurrentJumpApexLength;
    
    [Header("Coyote Time")]
    private bool b_IsCoyoteCoroutineActive;

    [SerializeField] private float m_CoyoteTimeThreashold;
    private float m_CoyoteTimeCounter;
    
    [Header("Jump Buffer")]
    private bool b_IsJumpBufferActive;

    [SerializeField] private float m_JumpBufferThreashold;
    private float m_JumpBufferTimeCounter;
    
    [Header("Particle System")]
    [SerializeField] private ParticleSystem m_JumpingParticleEffect;
    
    [Header("Camera Variables")]
    private CameraShake m_PlayerCamera;
    
    [SerializeField] private float m_CameraShakeMagnitude;
    [SerializeField] private float m_CameraShakeDuration;
    
    /* Enum */
    public enum JumpStates
    {
        Ascend,
        Apex,
        Falling
    }
    
    private JumpStates m_JumpState = JumpStates.Ascend;
      
    /* Other Scripts/Components.*/
    private Rigidbody2D m_RB;
    
    private PlayerControls m_ActionMap;
    
    private GroundDetector m_GroundDetector;
    
    private AudioComponent m_AudioComponent;

    /* Coroutines. */
    private Coroutine m_CoyoteJumpCoroutine;
    
    private Coroutine m_JumpBufferCoroutine;
    
    private Coroutine m_MoveCoroutine;

    private Coroutine m_JumpStatusHandler;
    #endregion
    
    #region Functions
    private void Awake()
    {
        m_ActionMap = new PlayerControls();

        // Simply checks if a component is within the parent, then assigns it.
        if (GetComponentInParent<GroundDetector>() != null)
        {
            m_GroundDetector = GetComponent<GroundDetector>();
        }

        if (GetComponentInParent<CameraShake>() != null)
        {
            m_PlayerCamera = GetComponent<CameraShake>();
        }

        if (GetComponentInParent<AudioComponent>() != null)
        {
            m_AudioComponent = GetComponent<AudioComponent>();
        }

        m_RB = GetComponent<Rigidbody2D>();
    }

    public void SetInMove(float direction)
    {
        m_InMove = direction;
        
        if (m_InMove == 0) // If the player is not moving.
        {
            b_IsMoveActive = false;
        }
        else // If the player is moving.
        {
            if(b_IsMoveActive) { return;}
            
            b_IsMoveActive = true;
            b_IsCheckingFallActive = true;
            
            SoundCaller(m_WalkingSoundID);
            
            m_CoyoteTimeCounter = 0;
            
            StartCoroutine(C_MovementUpdate());
        }
    }
    
    public bool IsMoving()
    {
        return b_IsMoveActive;
    }
    public void JumpPerformed()
    {
        b_IsGrounded = m_GroundDetector.GroundDetection();
        
            if (m_GroundDetector.GroundDetection() || m_CoyoteTimeCounter >= 0 && m_CoyoteTimeCounter <= m_CoyoteTimeThreashold) // Attempts to detect if the player can jump.
            {
                #region Assignment 2 Audio/Particle Parts

                /* Checks if there is an audio source, then plays the sound if there is one.
                 * Checks if there is a particle system, then plays the effect if there is one.
                 * This is only included as it is needed in assignment 2, but I've put character movement in my assignment 1
                 * script folder.*/ 
                
                if (m_PlayerCamera != null)
                {
                    m_PlayerCamera.ShakeCamera(m_CameraShakeMagnitude, m_CameraShakeDuration);
                }
                
                if (m_JumpingParticleEffect != null)
                {
                    m_JumpingParticleEffect.Play();
                }
                
                SoundCaller(m_JumpingSoundID);
                
                #endregion

                if (b_IsJumpActive)
                {
                    m_CoyoteTimeCounter = 0;
                    m_MinimumAmountFramesSinceOffGround = 0;
                
                    StartCoroutine(C_JumpStatusHandler());
                }

            }
            else if (!b_IsJumpBufferActive) //If the player can't jump.
            {
                b_IsJumpBufferActive = true;
            
                StartCoroutine(C_JumpBufferCoroutine());
            }
    }

    public void JumpFallSetter(JumpStates jumpStates) //Just receives and sets the jump status from the input handler.
    {
        m_JumpState = JumpStates.Falling;
    }

    public void SoundCaller(int m_SoundID)
    {
        if (m_AudioComponent != null) // Only plays a sound if there's an audio component.
        {
                m_AudioComponent.PlaySound(m_SoundID);
        }
    }
    
    #endregion

    #region Coroutines
    
    /* Movement Coroutine.
     * Handles the movement of the character while it is active. */
    private IEnumerator C_MovementUpdate()
    {
        while (b_IsMoveActive)
        {
            yield return new WaitForFixedUpdate();
            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
            
                      if (b_IsJumpActive && !b_IsCheckingFallActive && !m_GroundDetector.GroundDetection())
                        {
                            b_IsCheckingFallActive = true;
                            
                            StartCoroutine(C_FallChecker());
                        }
        }
    }

    private IEnumerator C_JumpStatusHandler()  // This handles the jump to have three parts that are executed after each other.
    {
        m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        
        while (m_JumpState == JumpStates.Ascend) //Handles ascending.
        {
                m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Force);
                
                m_CurrentAscendLength++;
                if (m_CurrentAscendLength == m_AscendLengthCounter)
                {
                    m_JumpState = JumpStates.Apex;
                    
                    m_CurrentAscendLength = 0;
                    
                    yield break;
                }
                
                yield return new WaitForFixedUpdate();
        }

        if (m_JumpState == JumpStates.Apex) // Handles when the jump is at the maximum height.
        {
            m_RB.gravityScale = 0.2f;

            for (int i = 0; i < m_JumpApexLengthCounter + 1; i++)
            {
                m_CurrentJumpApexLength = i;
                
                if (m_CurrentJumpApexLength == m_JumpApexLengthCounter)
                {
                    m_JumpState = JumpStates.Falling;
                    
                    m_CurrentJumpApexLength = 0;

                    yield break;
                } 
                yield return new WaitForFixedUpdate(); 
            }
        }

        while (m_JumpState == JumpStates.Falling) //Handles when the jump has finished and the player is falling.
        {
            m_RB.gravityScale = 1.0f;

            if (m_GroundDetector.GroundDetection())
            {
                m_JumpState = JumpStates.Ascend;
                
                SoundCaller(m_LandingSoundID);

                
                StopCoroutine(C_JumpStatusHandler());
                
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        
        yield return null;
    }
    
    private IEnumerator C_FallChecker() // Intended to check if the player is currently falling.
    {
        while (b_IsCheckingFallActive)
        {
            if (m_RB.linearVelocityY < 0)
            {
                if (!b_IsCoyoteCoroutineActive && !m_GroundDetector.GroundDetection()) //If the player is falling.
                {
                    b_IsCoyoteCoroutineActive = true;
                    
                    StartCoroutine(C_CoyoteJumpCoroutine());
                
                    b_IsCheckingFallActive = false;
                }
            }

            if (m_GroundDetector.GroundDetection()) // Detects if the player is currently grounded.
            {
                b_IsCheckingFallActive = false;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator C_CoyoteJumpCoroutine()
    {
        while (b_IsCoyoteCoroutineActive)
        {
            //This works on excluding the first few frames after the jump then checks if the bools are true.
            if (m_MinimumAmountFramesSinceOffGround > 0.5 && !m_GroundDetector.GroundDetection())
            {
                if (0 >= m_CoyoteTimeCounter && m_CoyoteTimeCounter <= m_CoyoteTimeThreashold)
                {
                    if (b_IsJumpActive)
                    {
                        b_IsCoyoteCoroutineActive = false;
                    
                        JumpPerformed();
                    
                        b_IsJumpActive = false;
                    
                        StopCoroutine(C_CoyoteJumpCoroutine());
                    }
                    
                    break;
                }
                
                if (m_CoyoteTimeThreashold >= m_CoyoteTimeThreashold )
                {
                    b_IsCoyoteCoroutineActive = false;

                    m_CoyoteTimeCounter = 0;

                    StopCoroutine(C_CoyoteJumpCoroutine());
                    
                    break;
                }
            }
            
            m_CoyoteTimeCounter += Time.deltaTime;
            m_MinimumAmountFramesSinceOffGround += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
    

    private IEnumerator C_JumpBufferCoroutine()
    {
        while (b_IsJumpBufferActive)
        {
            if (m_JumpBufferTimeCounter <= m_JumpBufferThreashold) // Checks if the jump is within the buffer time.
            {
                if (m_GroundDetector.GroundDetection() && b_IsJumpActive)
                {
                    m_JumpBufferTimeCounter = 0;
                    
                    b_IsJumpActive = true;
                    
                    b_IsCoyoteCoroutineActive = false;
                    b_IsJumpBufferActive = false;
                    
                    JumpPerformed();
                    
                    break; 
                }
            }
            else if (m_JumpBufferTimeCounter >= m_JumpBufferThreashold) // Fail condition for jump buffering.
            {
                m_JumpBufferTimeCounter = 0;
                
                b_IsJumpBufferActive = false;
                
                break;
            }
            
            m_JumpBufferTimeCounter += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion 
}

