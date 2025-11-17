using System;
using UnityEngine;

public class LightHumController : MonoBehaviour
{
    [Header("Light Object")]
    [SerializeField] private AudioSource m_AudioSource;

    public void LightStatus(bool CurrentStatus)
    {
        switch (CurrentStatus)
        {
            case true:
                m_AudioSource.Play();
                break;
            case false:
                m_AudioSource.Stop();
                break;
        }
    }
}
