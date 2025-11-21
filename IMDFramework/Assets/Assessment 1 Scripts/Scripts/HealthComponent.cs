using System;
using UnityEngine;
using Unity.Properties;


namespace Assessment_1_Scripts.Scripts
{
    public class HealthComponent : MonoBehaviour
    {
        /* Events */
        public event Action<float, float, float> OnDamaged;
        public event Action<MonoBehaviour> OnDeath;

        [Header ("Health")]
        [SerializeField] private float m_MaxHealth;
        private float m_CurrentHealth;

        [Header ("Particle System")]
        [SerializeField] private ParticleSystem m_InjuryParticleSystem;
    
        [CreateProperty]
        public float PlayerCurrentHealth { get { return m_CurrentHealth; } }
        
        private void Start()
        {
            m_CurrentHealth = m_MaxHealth;
        }

        public void ApplyDamage(float Damage, MonoBehaviour DamageCauser)
        {
            float HealthChange = Mathf.Min(m_CurrentHealth, Damage);
            m_CurrentHealth -= HealthChange;
        
            m_InjuryParticleSystem.Play();
        
            OnDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, Damage);
            if (m_CurrentHealth == 0)
            {
                OnDeath?.Invoke(DamageCauser);
            }
        }
    }
}
