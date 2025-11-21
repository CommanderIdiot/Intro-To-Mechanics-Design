using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Unity.Properties;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class SignpostUIPopupMessage : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private UIDocument UIDocument;
    [SerializeField] private ScriptableObject m_SignpostText;
    
    [Header("Message Visibility Settings")]
    private Coroutine m_SignpostMessageVisibilityCoroutine;
    [SerializeField] private float m_MessageVisibilityThreashold;
    private float m_MessageVisibilityTimer;
    
    [CreateProperty] private ScriptableObject SignText { get { return m_SignpostText; } }
    
    public void Interact()
    { 
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
                
                
                
                Debug.Log("Message coroutine ended");
                StopCoroutine(c_SignpostMessageVisibilityCoroutine());
                break;
            }
            
            m_MessageVisibilityTimer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
