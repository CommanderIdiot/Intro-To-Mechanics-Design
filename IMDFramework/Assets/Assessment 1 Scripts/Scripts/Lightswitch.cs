using System;
using UnityEngine;

public class Lightswitch : MonoBehaviour, IInteractable
{
    [Header("Light Object")]
    [SerializeField] private GameObject m_LightObject;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioSource m_AudioComponent;

    [SerializeField] private AudioClip m_AudioClip;
    
    [Header("Particle System")]
    [SerializeField] private ParticleSystem m_LeverParticles;

    private LightHumController m_LightHumController;
    private void Awake()
    {
        if (GetComponentInParent<LightHumController>() != null)
        {
            m_LightHumController = GetComponentInParent<LightHumController>();
        }
    }

    public void Interact()
    {
        m_LightObject.SetActive(!m_LightObject.activeSelf);
        
        m_AudioComponent.Play();
        
        m_LightHumController.LightStatus(m_LightObject.activeSelf);
        
        m_LeverParticles.Play();
    }
}
