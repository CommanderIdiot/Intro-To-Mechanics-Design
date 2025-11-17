using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Object")]
    [SerializeField] private GameObject m_DoorObject;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioSource m_AudioComponent;

    [SerializeField] private AudioClip m_AudioClip;
    
    [Header("Particle System")]
    [SerializeField] private ParticleSystem m_DoorParticles;
    
    public void Interact()
    {
        m_DoorObject.SetActive(!m_DoorObject.activeSelf);

        m_AudioComponent.Play();
        
        m_DoorParticles.Play();
    }
}
