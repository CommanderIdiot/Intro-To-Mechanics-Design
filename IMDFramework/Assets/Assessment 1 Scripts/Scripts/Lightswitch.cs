using UnityEngine;

public class Lightswitch : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_LightObject;
    
    [SerializeField] private int m_LeverCreeking;
    
    private bool b_LeverAudioLoop = false;

    
    [SerializeField] private AudioComponent m_AudioComponent;
    
    [SerializeField] private ParticleSystem m_LeverParticles;
    
    public void Interact()
    {
        m_LightObject.SetActive(!m_LightObject.activeSelf);
        
        m_AudioComponent.PlaySound(m_LeverCreeking);
        
        m_LeverParticles.Play();
    }
}
