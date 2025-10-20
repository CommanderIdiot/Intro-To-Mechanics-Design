using System;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;
    public bool m_IsGrounded;
    private void FixedUpdate()
    {
        m_IsGrounded = Physics2D.Raycast(m_CharacterMovement.m_RaycastPosition.position, Vector2.down, 0.1f, m_CharacterMovement.m_GroundLayer);
    }
}

/*
 *Plan:
 * 1. Character movement calls ground sensor.
 * 2. Runs fixed update, could maybe try to make it into a coroutine.
 * 3. Gives a notification to character movement to update it.
 * 
 */