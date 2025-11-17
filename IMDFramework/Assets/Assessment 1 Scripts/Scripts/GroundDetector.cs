using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Raycast Position")]
    [SerializeField] public Transform m_RaycastPosition;

    [Header("Layer Mask")]
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