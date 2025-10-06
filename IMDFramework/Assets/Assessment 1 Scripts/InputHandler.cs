using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;
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
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_SetInMove;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
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
    }
    #endregion
}
