using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class SignpostUIPopupMessage : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private UIDocument m_SignpostTextDocument;
    [SerializeField] private SignpostTextStorage m_SignpostText;
    
    [SerializeField] private UIDocument m_SignpostMessageDocument;
    [SerializeField] private SignpostTextStorage m_SignpostPopupText;
    
    //private SignpostUIMessageController m_SignpostUIMessageController;
    
    [Header("Message Visibility Settings")]
    [SerializeField] private GameObject m_MessageObject;
    private Coroutine m_SignpostMessageVisibilityCoroutine;
    [SerializeField] private float m_MessageVisibilityThreashold;
    private float m_MessageVisibilityTimer;

    public event Action<string, string> OnStartUp;

    public event Action<string> OnMessageRead;
    
    [CreateProperty] private ScriptableObject SignText { get { return m_SignpostText; } }

    private void Awake()
    {
        m_MessageObject.SetActive(false);
//        m_SignpostTextDocument.GetComponent<Label>().text = m_SignpostText.TextStorage;
        //Call something to load scriptable objects into both text & message?
    }

    private void Start()
    {
        OnStartUp?.Invoke(m_SignpostText.TextStorage, m_SignpostPopupText.TextStorage);

    }
    
    public void Interact()
    {
        m_MessageObject.SetActive(true);
        //OnMessageRead?.Invoke(m_SignpostText.TextStorage);
        //Have a way for this to interact with the signpost UI (as its own object)
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
                m_MessageObject.SetActive(false);
                
                Debug.Log("Message coroutine ended");
                StopCoroutine(c_SignpostMessageVisibilityCoroutine());
                break;
            }
            
            m_MessageVisibilityTimer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
