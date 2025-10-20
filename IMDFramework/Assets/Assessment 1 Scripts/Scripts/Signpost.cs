using System.Collections;
using UnityEngine;

public class Signpost : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_SignpostDisplayObject;
    
    private Coroutine c_cSignpostMessageVisibilityCoroutine;
    [SerializeField] private float m_MessageVisibilityThreashold;
    private float m_MessageVisibilityTimer;
    
    public void Interact()
    { 
        m_SignpostDisplayObject.SetActive(true);
        Debug.Log("Message coroutine started");
        StartCoroutine(c_SignpostMessageVisibilityCoroutine());
    }
    
    private IEnumerator c_SignpostMessageVisibilityCoroutine()
    {
        while (true)
        {
            if (m_MessageVisibilityTimer >= m_MessageVisibilityThreashold)
            {
                m_MessageVisibilityTimer = 0;
                m_SignpostDisplayObject.SetActive(false);
                
                Debug.Log("Message coroutine ended");
                StopCoroutine(c_SignpostMessageVisibilityCoroutine());
                break;
            }
            
            m_MessageVisibilityTimer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
