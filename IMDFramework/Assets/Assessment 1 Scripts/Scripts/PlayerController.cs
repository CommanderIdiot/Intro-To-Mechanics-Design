using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private HealthComponent m_HealthComponent;

    private void Awake()
    {
        m_HealthComponent = GetComponent<HealthComponent>();
        Debug.Assert(m_HealthComponent != null);
    }

    private void Start()
    {
        m_HealthComponent.OnDamaged += Handle_OnHealthDamage;
        m_HealthComponent.OnDeath += Handle_OnPlayerDeath;
    }

    private void OnDestroy()
    {
        m_HealthComponent.OnDamaged -= Handle_OnHealthDamage;
        m_HealthComponent.OnDeath -= Handle_OnPlayerDeath;
    }

    private void Handle_OnHealthDamage(float CurrentHealth, float MaxHealth, float InboundDamage)
    {
        Debug.Log($"Current health is: {CurrentHealth} out of {MaxHealth} and the damage receieved was: {InboundDamage}");
    }

    private void Handle_OnPlayerDeath(MonoBehaviour CauseOfDeath)
    {
        Debug.Log($"Player has died to {CauseOfDeath.gameObject.name}");
    }
}