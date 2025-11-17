using System;
using System.Collections;
using UnityEngine;

public class AudioComponent : MonoBehaviour
{
    #region Variables
    /* Audio source. */
    private AudioSource m_AudioSource;
    
    /* Character movement*/
    private CharacterMovement m_CharacterMovement;
    
    /* Events. */
    public event Action<int> OnSoundPlayed; 
        
    /* Bools. */
    private bool b_IsPlayingAudio;
    private bool b_IsLooping;
    private bool b_IsPlayingAudioLoop;
    
    
    /* Ints. */
    private int m_AudioArrayIndexPointer;
    private int m_AudioArrayMaxSize;
    
    /* Floats. */
    private float m_AudioClipLength;
    
    /* Coroutine. */
    private Coroutine m_PlayingAudio;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource m_JumpingAudioSource;
    [SerializeField] private AudioSource m_WalkingAudioSource;
    [SerializeField] private AudioSource m_LandingAudioSource;
    
    public AudioClip[] AudioClipArray;
    
    #endregion

    private void Awake()
    {
        if (GetComponentInParent<AudioSource>() != null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        };
        if (GetComponentInParent<CharacterMovement>() != null)
        {
            m_CharacterMovement = GetComponentInParent<CharacterMovement>();
        }
        
        m_AudioArrayMaxSize = AudioClipArray.Length;
    }
    
    public void PlaySound(int AudioClipIndex)
    {
        switch (AudioClipIndex) //Depending on the passed in int, it will play the appropriate audio source.
        {
            case 0:
                m_JumpingAudioSource.Play();
                
                GetAudioClipLength(AudioClipIndex);
                break;
            case 1:
                m_LandingAudioSource.Play();
                
                GetAudioClipLength(AudioClipIndex);

                break;
            case 2:
                m_WalkingAudioSource.Play();
                
                GetAudioClipLength(AudioClipIndex);

                break;
        }
        
    }
    private void StopPlaySound()
    {
        m_AudioClipLength = 0.0f;
        
        b_IsPlayingAudio = false;
    }
    
    /// <summary>
    /// Receives an audio array index number, gets the clip length then starts the appropriate coroutine for audio playing.
    /// </summary>
    /// <param name="AudioClipIndex"></param>
    private void GetAudioClipLength(int AudioClipIndex)
    {
        m_AudioArrayIndexPointer = AudioClipIndex;

        AudioClipArray[m_AudioArrayIndexPointer] = m_AudioSource.clip;
        
        m_AudioClipLength = m_AudioSource.clip.length;

        if (AudioClipIndex == 0 || AudioClipIndex == 1)
        {
            StartCoroutine(C_PlayingAudio());
        }
        else if (AudioClipIndex == 2)
        {
            b_IsLooping = true;
            
            StartCoroutine(C_PlayingLoopAudio(b_IsLooping));
        }
        
    }
   
    private IEnumerator C_PlayingAudio()
    {
        while (b_IsPlayingAudio)
        {
            yield return new  WaitForSecondsRealtime(m_AudioClipLength);
            
            StopPlaySound();
        }
    }

    private IEnumerator C_PlayingLoopAudio(bool b_IsPlayingAudioLoop)
    {
        while (b_IsPlayingAudioLoop)
        {
            yield return new  WaitForSecondsRealtime(m_AudioClipLength);
            
            if (!m_CharacterMovement.IsMoving()) //Stops the audio from playing.
            {
                m_WalkingAudioSource.Stop();
            }
        }
    }
}
