using System;
using System.Collections;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public event Action<bool> OnGroundContact;
    
    [SerializeField] public Transform m_RaycastPosition;
    
    [SerializeField] private LayerMask m_GroundLayer;

    private Coroutine m_GroundDetection;
    
    public bool b_IsGrounded;

    private bool b_IsGroundDetectionActive;

    /*public void GroundDetection()
    {
        b_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
        
        Debug.Log("Ground Detection Finished.");
        
        OnGroundContact?.Invoke(b_IsGrounded);

        b_IsGrounded = false;
    }*/

    private void Start()
    {
        if (!b_IsGroundDetectionActive)
        {
            StartCoroutine(C_GroundDetection());
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(C_GroundDetection());
    }

    private IEnumerator C_GroundDetection()
    {
        while (true)
        {
            b_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
            
            if (b_IsGrounded)
            {
                OnGroundContact?.Invoke(b_IsGrounded);

                yield return new WaitForFixedUpdate();
            }
            
            b_IsGrounded = false;
            OnGroundContact?.Invoke(false);

            
            yield return new WaitForFixedUpdate();
        }
    }
}

/*
 *Plan:
 * 1. Character movement calls ground sensor.
 * 2. Runs fixed update, could maybe try to make it into a coroutine.
 * 3. Gives a notification to character movement to update if the player is grounded.
 * 4. Stop it spamming event announcement as much.
 */