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
        m_ActionMap.Default.Jump.canceled += Handle_JumpCancelled;
        m_ActionMap.Default.Interact.performed += Handle_InteractPerformed;
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_SetInMove;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
        m_ActionMap.Default.Jump.canceled -= Handle_JumpCancelled;
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
        m_CharacterMovement.JumpPerformed();
        //m_CharacterMovement.JumpSetter(CharacterMovement.JumpStates.Ascend);
    }

    private void Handle_JumpCancelled(InputAction.CallbackContext context)
    {
        m_CharacterMovement.JumpSetter(CharacterMovement.JumpStates.Falling);
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
