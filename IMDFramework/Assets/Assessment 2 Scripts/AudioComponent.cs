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
    private int AudioArrayIndexPointer = 0;
    
    /* Coroutine. */
    private Coroutine m_PlayingAudio;

    public AudioClip[] AudioClipArray;
    
    #endregion
    
    
    public void PlaySound(int AudioClipIndex)
    {
        if (AudioClipIndex != null)
        {
            AudioArrayIndexPointer = AudioClipIndex;
        }
        
        m_AudioSource.clip = AudioClipArray[AudioArrayIndexPointer];
        
        m_AudioSource.Play();
        
        //b_IsPlayingAudio = true;
        //StartCoroutine(C_PlayingAudio());
    }

    private void StopPlaySound()
    {
        m_AudioSource.clip = null;
    }
    
    private IEnumerator C_PlayingAudio()
    {
        while (b_IsPlayingAudio)
        {
            yield return new  WaitForEndOfFrame();
        }
    }
}
