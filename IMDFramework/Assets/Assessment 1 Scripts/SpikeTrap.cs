using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float m_SpikeDamage;

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.transform.parent.TryGetComponent<HealthComponent>(out var HealthComponent))
        {
            HealthComponent.ApplyDamage(m_SpikeDamage, this);
        }
    }
}
