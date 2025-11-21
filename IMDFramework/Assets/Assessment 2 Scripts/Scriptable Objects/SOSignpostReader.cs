using UnityEngine;
using UnityEngine.UIElements;

public class SOSignpostReader : MonoBehaviour
{
    [SerializeField] private UIDocument m_SignpostTextDocument;

    [SerializeField] private UIDocument m_SignpostMessageDocument;

    [Header("Signpost UI Popup Message")]
    [SerializeField] private SignpostUIPopupMessage m_SignpostUIPopupMessage;
    
    private void OnEnable()
    {
        m_SignpostUIPopupMessage.OnMessageRead += Handle_OnMessageRead;
        m_SignpostUIPopupMessage.OnStartUp += Handle_OnStartUp;

    }

    private void OnDestroy()
    {
        m_SignpostUIPopupMessage.OnMessageRead -= Handle_OnMessageRead;
        m_SignpostUIPopupMessage.OnStartUp -= Handle_OnStartUp;

    }

    private void Handle_OnStartUp(string BoardText, string PopupMessage)
    {
        //Look in week 5's slideshow for UI on slide 54/55 to see how to query for lables
        m_SignpostTextDocument.GetComponent<Label>().text = BoardText;
        m_SignpostMessageDocument.GetComponent<Label>().text = PopupMessage;
    }
    
    private void Handle_OnMessageRead(string SignpostTextMessage)
    {
        Debug.Log(SignpostTextMessage);
        //m_Document.GetComponent<Label>().text = SignpostTextMessage;
    }
}
