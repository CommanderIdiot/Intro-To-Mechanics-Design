using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class CharacterMovement : MonoBehaviour
{
    #region Variables
    private Rigidbody2D m_RB;
    
    [SerializeField] public LayerMask m_GroundLayer;

    private PlayerControls m_ActionMap;
    
    private GroundDetector m_GroundDetector;

    /* Coroutines. */
    private Coroutine m_CoyoteJumpCoroutine;
    
    private Coroutine m_JumpBufferCoroutine;
    
    private Coroutine m_MoveCoroutine;

    private Coroutine m_JumpStatusHandler;
    
    /* Bools. */
    private bool b_IsGrounded;
    
    private bool b_IsCoyoteCoroutineActive;
    
    private bool b_IsJumpActive = true;

    private bool b_IsJumpBufferActive = false;
    
    private bool b_IsMoveActive;
    
    /* Ints */
    [SerializeField] private int m_AscendLengthCounter;
    private int m_CurrentAscendLength;
    
    [SerializeField] private int m_JumpApexLengthCounter;
    private int m_CurrentJumpApexLength;
    
    /* Floats */
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_JumpStrength;
    private float m_InMove;
    
    [SerializeField] private float m_CoyoteTimeThreashold = 0.4f;
    [SerializeField] float m_JumpPowerTimer;
    private float m_CoyoteTimeCounter;
    private float m_FrameTimeThreashold = 0;

    [SerializeField] private float m_JumpBufferThreashold = 0.4f;
    private float m_JumpBufferTimeCounter;

    private float m_RigidBodyGravity;
    
    /* Audio Sources*/
    [SerializeField] private AudioSource m_JumpingSound;
    
    /* Particle Systems */
    [SerializeField] private ParticleSystem m_JumpingParticleEffect;
    
    /* Enum */
    public enum JumpStates
    {
        Ascend,
        Apex,
        Falling
    }
    
    private JumpStates m_JumpState = JumpStates.Ascend;

    #endregion
    
    #region Awake/Start/OnDestroy Functions

    private void Awake()
    {
        m_ActionMap = new PlayerControls();

        if (GetComponentInParent<GroundDetector>() != null)
        {
            m_GroundDetector = GetComponent<GroundDetector>();
            Debug.Log("Ground detector attached");
        }

        m_RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_RigidBodyGravity = m_RB.gravityScale;
        Debug.Log("Gravity scale saved.");
    }
    #endregion
    
    #region Move/Jump/Jump Setter Functions

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
        b_IsGrounded = m_GroundDetector.GroundDetection();
        Debug.Log("IsGrounded equals "  + b_IsGrounded);
        
            if (b_IsGrounded || m_CoyoteTimeCounter > 0)
            {
                #region Assignment 2 Audio/Particle Parts

                /* Checks if there is an audio source, then plays the sound if there is one.
                 * Checks if there is a particle system, then plays the effect if there is one.
                 * This is only included as it is needed in assignment 2, but I've put character movement in my assignment 1
                 * script folder.
                 */
                
                if (GetComponentInParent<AudioSource>() != null)
                {
                    m_JumpingSound.Play();
                    Debug.Log("Insert jump sound here");
                }

                if (m_JumpingParticleEffect != null)
                {
                    m_JumpingParticleEffect.Play();
                    Debug.Log("Insert jump effect here"); 
                }

                #endregion
                
                m_CoyoteTimeCounter = 0;
                m_FrameTimeThreashold = 0;
                
                StartCoroutine(C_JumpStatusHandler()); //Starts the jump status handler.
            }
            //Performs if the jump buffer is not active
            /*else if (!b_IsJumpBufferActive)
            {
                b_IsJumpBufferActive = true;
            
                StartCoroutine(C_JumpBufferCoroutine());
            }*/
    }

    public void JumpFallSetter(JumpStates jumpStates) //Just receives and sets the jump status from the input handler.
    {
        m_JumpState = JumpStates.Falling;
    }
    
    #endregion

    /* make a jump handler coroutine STARTED
     * have it use a while loop for rising that adds a force mode 2d . force STARTED
     * if for the apex
     * a while until it touches the ground to stop the coroutine
     * reset state after jump is finished
     * add a jump cancel event into input handler to cancel the jump
     *
     * get ground detector found in awake COMPLETED
     * then call the function to return the detection as true or false COMPLETED
     * have an update to run the raycast COMPLETED
    */

    #region Coroutines
    
    /* Movement Coroutine.
     * Handles the movement of the character while it is active.
     */
    private IEnumerator C_MovementUpdate()
    {
        while (b_IsMoveActive)
        {
            yield return new WaitForFixedUpdate();
            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

            if (m_RB.linearVelocityY > m_RB.gravityScale)
            {
                StartCoroutine(C_CoyoteJumpCoroutine());
            }
        }
    }

    private IEnumerator C_JumpStatusHandler()
    {
        //Enters jump handler.
        m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse); //Adds a small force to the character.
        
        while (m_JumpState == JumpStates.Ascend) //Starts when the jumpstates is ascend.
        {
                m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Force); //Adds a small force to the character.
                
                m_CurrentAscendLength++; //Should increase the current ascend length to equal i.
                if (m_CurrentAscendLength == m_AscendLengthCounter) //When this is true.
                {
                    m_JumpState = JumpStates.Apex; //Changes the jumpstate to apex.
                    
                    m_CurrentAscendLength = 0;
                    
                    yield break; //Breaks out.
                }
                
                yield return new WaitForFixedUpdate(); //Returns to wait 0.1 seconds
        }

        if (m_JumpState == JumpStates.Apex)
        {
            Debug.Log("Inside of jump apex");

            m_RB.gravityScale = 0.2f;

            for (int i = 0; i < m_JumpApexLengthCounter + 1; i++)
            {
                m_CurrentJumpApexLength = i;
                
                if (m_CurrentJumpApexLength == m_JumpApexLengthCounter)
                {
                    m_JumpState = JumpStates.Falling; //Changes the jumpstate to falling.
                    
                    m_CurrentJumpApexLength = 0;

                    yield break;
                } 
                yield return new WaitForFixedUpdate(); 
            }
        }

        while (m_JumpState == JumpStates.Falling)
        {
            Debug.Log("Inside of falling");

            m_RB.gravityScale = 1.0f;

            if (b_IsGrounded == m_GroundDetector.GroundDetection())
            {
                m_JumpState = JumpStates.Ascend;
                
                StopCoroutine(C_JumpStatusHandler());
                
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
    
    /* Coyote time coroutine.
     * Detects if the character is grounded by:
     * 1. Has a certain time passed? This is in order to avoid double jumping.
     * 2. Is the character on the ground?
     * 
     */
    private IEnumerator C_CoyoteJumpCoroutine()
    {
        while (b_IsCoyoteCoroutineActive)
        {
            //This works on excluding the first few frames after the jump then checks if the bools are true.
            if (m_FrameTimeThreashold > 0.5 && b_IsGrounded)
            {
                m_CoyoteTimeCounter = m_CoyoteTimeThreashold;
                
                b_IsJumpActive = true;
                
                b_IsCoyoteCoroutineActive = false;
                
                m_GroundDetector.GroundDetection();
                
                StartCoroutine(C_JumpStatusHandler());
                
                StopCoroutine(C_CoyoteJumpCoroutine());

                break;
            }

            if (m_CoyoteTimeCounter <= 0)
            {
                StopCoroutine(C_CoyoteJumpCoroutine());

                b_IsCoyoteCoroutineActive = false;
                
                break;
            }
            
            m_CoyoteTimeCounter -= Time.deltaTime;
            m_FrameTimeThreashold += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
    /* Jump buffer coroutine.
     * 
     */
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
                    
                    m_GroundDetector.GroundDetection();
                    
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
