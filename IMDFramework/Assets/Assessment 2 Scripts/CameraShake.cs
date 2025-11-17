using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Variables
    /* Camera */
    private Camera m_PlayerCamera = null;
    
    /* Floats */
    private float m_PlayerShakeMagnitude;
    private float m_PlayerShakeDuration;
    
    #endregion

    private void Awake()
    {
        if (Camera.main != null)
        {
            m_PlayerCamera =  Camera.main;
        }
    }
    
    public void ShakeCamera(float  ShakeMagnitude, float ShakeDuration)
    {
        StartCoroutine(C_CameraShaking(ShakeMagnitude, ShakeDuration));
    }
    
    /// <summary>
    /// Takes a shake magnitude & duration, then shakes the camera in a random manner for the duration up to the shake magnitude's maximum value.
    /// </summary>
    /// <param name="m_PlayerShakeMagnitude"></param>
    /// <param name="m_PlayerShakeDuration"></param>
    /// <returns></returns>
    public IEnumerator C_CameraShaking(float m_PlayerShakeMagnitude, float m_PlayerShakeDuration)
    {
        Vector3 OriginalPosition = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < m_PlayerShakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * m_PlayerShakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f)  * m_PlayerShakeMagnitude;
            
            Camera.main.transform.localPosition = new Vector3(OriginalPosition.x + x, OriginalPosition.y + y, OriginalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = OriginalPosition;
    }
}
