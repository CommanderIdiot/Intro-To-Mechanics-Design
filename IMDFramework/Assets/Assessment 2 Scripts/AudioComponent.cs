using System;
using System.Collections;
using UnityEngine;

public class AudioComponent : MonoBehaviour
{

    #region Variables
    /* Audio source. */
    private AudioSource m_AudioSource;
    
    /* Events. */
    public event Action<int> OnSoundPlayed; 
        
    /* Bools. */
    private bool b_IsPlayingAudio;
    
    /* Ints. */
    private int m_AudioArrayIndexPointer;
    
    /* Floats. */
    private float m_AudioClipLength;
    
    /* Coroutine. */
    private Coroutine m_PlayingAudio;

    public AudioClip[] AudioClipArray;
    
    #endregion

    private void Awake()
    {
        if (GetComponentInParent<AudioSource>() != null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        };
    }
    
    public void PlaySound(int AudioClipIndex)
    {
        if (m_AudioSource.clip != null)
        {
            m_AudioArrayIndexPointer = AudioClipIndex;
            
            m_AudioClipLength = AudioClipArray[m_AudioArrayIndexPointer].length;
        }
        
        m_AudioSource.clip = AudioClipArray[m_AudioArrayIndexPointer];
        
        m_AudioSource.Play();
        
        b_IsPlayingAudio = true;
        StartCoroutine(C_PlayingAudio());
    }

    private void StopPlaySound()
    {
        m_AudioSource.clip = null;
        b_IsPlayingAudio = false;
    }
    
    private IEnumerator C_PlayingAudio()
    {
        while (b_IsPlayingAudio)
        {
            yield return new  WaitForSecondsRealtime((m_AudioClipLength * 10));
            
            StopPlaySound();
        }
    }
}

/*Plan:
 * 1. Make a coroutine to play the sound fully.
 * 2. Make a function that just has the sound ID passed in to reduce amount of repeated code.
 */
