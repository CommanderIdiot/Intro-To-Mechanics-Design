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
    
    /* Coroutine */

    #endregion

    /* Do this so it receives an event to do something?
     * or
     * Do this so it is called by another script as a function?
     */

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
