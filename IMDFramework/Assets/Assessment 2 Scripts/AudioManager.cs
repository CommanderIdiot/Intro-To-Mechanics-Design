using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioComponent m_AudioComponent;
    
    private void Awake()
    {
        m_AudioComponent = GetComponent<AudioComponent>();
    }

    private void Start()
    {
        m_AudioComponent.OnSoundPlayed += Handle_SoundPlayed;
    }

    private void OnDestroy()
    {
        m_AudioComponent.OnSoundPlayed -= Handle_SoundPlayed;
    }

    private void Handle_SoundPlayed(int ClipIndex)
    {
        m_AudioComponent.PlaySound(ClipIndex);
    }
}
