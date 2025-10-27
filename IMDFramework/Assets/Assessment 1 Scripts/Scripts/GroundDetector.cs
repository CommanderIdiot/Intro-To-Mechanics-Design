using System;
using System.Collections;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public event Action<bool> OnGroundContact;

    [SerializeField] public Transform m_RaycastPosition;

    [SerializeField] private LayerMask m_GroundLayer;

    private Coroutine m_GroundDetection;

    public bool b_IsOnGrounded;

    private bool b_IsGroundDetectionActive;

    public bool GroundDetection()
    {
        return b_IsOnGrounded;
    }

    private void Update()
    {
        b_IsOnGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
    }
}
/*
 *Plan:
 * 1. Character movement calls ground sensor.
 * 2. Runs fixed update, could maybe try to make it into a coroutine.
 * 3. Gives a notification to character movement to update if the player is grounded.
 * 4. Stop it spamming event announcement as much.
 */