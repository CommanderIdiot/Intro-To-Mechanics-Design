using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spiketrap Settings")]
    [SerializeField] private float m_SpikeDamage;
    
    [SerializeField] private float m_CameraShakeMagnitude;
    [SerializeField] private float m_CameraShakeDuration;

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.transform.parent.TryGetComponent<HealthComponent>(out var HealthComponent))
        {
            HealthComponent.ApplyDamage(m_SpikeDamage, this);
        }
        
        if (Collision.transform.parent.TryGetComponent<CameraShake>(out var CameraShakeComponent))
        {
            CameraShakeComponent.ShakeCamera(m_CameraShakeMagnitude, m_CameraShakeDuration);
        }
    }
}
