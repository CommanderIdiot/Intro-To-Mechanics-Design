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

    public void Interact()
    {
        m_LightObject.SetActive(!m_LightObject.activeSelf);
        
        m_AudioComponent.Play();
        
        m_LeverParticles.Play();
    }
}
