using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float, float, float> OnDamaged;
    public event Action<MonoBehaviour> OnDeath;

    [SerializeField] private float m_MaxHealth;
    private float m_CurrentHealth;

    private void start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void ApplyDamage(float Damage, MonoBehaviour DamageCauser)
    {
        float HealthChange = Mathf.Min(m_CurrentHealth, Damage);
        m_CurrentHealth -= HealthChange;
        
        OnDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, Damage);
        if (m_CurrentHealth == 0)
        {
            OnDeath?.Invoke(DamageCauser);
        }
    }
}
