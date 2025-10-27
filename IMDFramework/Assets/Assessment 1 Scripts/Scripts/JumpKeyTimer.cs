using Unity.VisualScripting;
using UnityEngine;

public class JumpKeyTimer : MonoBehaviour
{
    private float m_KeyPressedTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        KeyTimer();
    }

    public float KeyTimer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_KeyPressedTimer += Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_KeyPressedTimer = Time.time - m_KeyPressedTimer;
            return (m_KeyPressedTimer);
        }
        return (m_KeyPressedTimer);
    }
}
