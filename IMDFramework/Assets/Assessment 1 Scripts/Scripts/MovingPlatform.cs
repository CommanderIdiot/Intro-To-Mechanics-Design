using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  [SerializeField] private float m_Amplitude = 1;
  [SerializeField] private float m_Frequency = 1;

  private void FixedUpdate()
  {
    float yOffset = Mathf.Sin(Time.time * m_Frequency) * m_Amplitude;
    
    transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
  }
}
