using System;
using UnityEngine;

public class DoorCameraShake : MonoBehaviour
{
    [Header("Camera Shake Effect")]
    [SerializeField] private float m_ShakeMagnitude;
    [SerializeField] private float m_ShakeDuration;

    [Header("Door Object")]
    [SerializeField] private GameObject m_DoorObject;
    
    private DoorController m_DoorController;
    
    private void Awake()
    {
        /*if (GetComponentInParent<DoorController>() == null)
        {
            m_DoorController = gameObject.AddComponent<DoorController>();
        }*/
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (m_DoorObject.activeSelf)
        {
            if (Collision.transform.parent.TryGetComponent<CameraShake>(out var CameraShakeComponent))
            {
                CameraShakeComponent.ShakeCamera(m_ShakeMagnitude, m_ShakeDuration);
            }
        }
    }
}
