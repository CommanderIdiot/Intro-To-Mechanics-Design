using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_DoorObject;
    
    [SerializeField] private int m_DoorCreekSound;
    
    private bool b_LeverAudioLoop = false;


    [SerializeField] private AudioComponent m_AudioComponent;
    
    [SerializeField] private ParticleSystem m_DoorParticles;
    
    public void Interact()
    {
        m_DoorObject.SetActive(!m_DoorObject.activeSelf);

        m_AudioComponent.PlaySound(m_DoorCreekSound);
        
        m_DoorParticles.Play();
    }
}
