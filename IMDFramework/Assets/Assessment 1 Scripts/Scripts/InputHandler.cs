using System;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;

    [SerializeField] private LayerMask m_InteractableLayer;
    private PlayerControls m_ActionMap;

    private float m_JumpPressedLength;

    private float m_JumpPressedDown;
    private float m_JumpPressedReleased;
    private void Awake()
    {
        m_ActionMap = new PlayerControls();
    }
    
    #region Bindings
    private void OnEnable()
    {
        m_ActionMap.Enable();

        m_ActionMap.Default.MoveHoriz.performed += Handle_SetInMove;
        m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
        m_ActionMap.Default.Interact.performed += Handle_InteractPerformed;
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_SetInMove;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
        m_ActionMap.Default.Interact.performed -= Handle_InteractPerformed;
    }
    #endregion
    
    #region InputFunctions

    private void Handle_SetInMove(InputAction.CallbackContext context)
    {
        m_CharacterMovement.SetInMove(context.ReadValue<float>());
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        m_CharacterMovement.SetInMove(0);
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        //m_JumpPressedLength = Figure out how to get key pressed length
        
        //get on pressed and on cancelled and apply a negative force.
        
        /*Plan:
         * 1. Get the time of when the key is pressed.
         * 2. Store the time of when the key is pressed.
         * 3. Get the time of when the key is released.
         * 4. Store the time of when the key is released.
         * 5. Calculate and pass through the result into jump performed.
         */
        
        /*Get if the jump is held down.
         * Do a short while loop of when the key is down.
         * Break it when the key is released.
         * 
         */
        
        if (Input.GetKeyDown("Jump"))
        {
            m_JumpPressedDown = Time.deltaTime;
            while (true)
                {
                    if (Input.GetKeyUp("Jump"))
                    {
                        m_JumpPressedReleased = Time.deltaTime;
                        
                        m_JumpPressedLength = m_JumpPressedReleased - m_JumpPressedDown;
                        
                        Debug.Log("Broken out of while loop");
                        break;
                    }
                    
                    
                }
        }
        
        
        
        /*if (GetComponentInParent<GroundDetector>() == true)
        {
            Debug.Log("Ground Detection Called.");

            GroundDetector m_GroundDetector = GetComponentInParent<GroundDetector>();
            m_GroundDetector.StartCoroutine(C_GroundDetection);
        }*/

        m_CharacterMovement.JumpPerformed(m_JumpPressedLength); //Pass key pressed length
    }
    
    private void Handle_InteractPerformed(InputAction.CallbackContext context)
    {
        Collider2D Collider = Physics2D.OverlapCircle(transform.position, 1, m_InteractableLayer);

        if (Collider != null && Collider.transform.TryGetComponent<IInteractable>(out var Interactable))
        {
            Interactable.Interact();
        }
    }
    #endregion
}
