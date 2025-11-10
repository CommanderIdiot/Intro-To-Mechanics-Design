using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float m_SpikeDamage;
    
    [SerializeField] private float m_CameraShakeMagnitude;
    [SerializeField] private float m_CameraShakeDuration;

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.transform.parent.TryGetComponent<HealthComponent>(out var HealthComponent))
        {
            HealthComponent.ApplyDamage(m_SpikeDamage, this);
            
            if (Collision.transform.parent.TryGetComponent<CameraShake>(out var CameraShakeComponent))
            {
                CameraShakeComponent.ShakeCamera(m_CameraShakeMagnitude, m_CameraShakeDuration);
            }
        }
    }

    /*public IEnumerator C_CameraShake(float ShakeMagnitude, float ShakeDuration)
    {
        Vector3 OriginalPosition = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < ShakeDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * ShakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f)  * ShakeMagnitude;
            
            Camera.main.transform.localPosition = new Vector3(OriginalPosition.x + x, OriginalPosition.y + y, OriginalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = OriginalPosition;
    }*/
}
