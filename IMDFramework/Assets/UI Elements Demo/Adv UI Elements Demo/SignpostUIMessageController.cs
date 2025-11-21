using System;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UIElements;

public class SignpostUIMessageController : MonoBehaviour
{
 private UIDocument m_Document;
 
 [Header("Signpost UI Popup Message")]
 [SerializeField] private SignpostUIPopupMessage m_SignpostUIPopupMessage;
 public void Awake()
 {
  //Creates the UI structure for the signpost message
  #region UI Creation

  VisualElement m_Container = new VisualElement();
  m_Container.name = "TextUIContainer";
  GetComponent<UIDocument>().rootVisualElement.Add(m_Container);
            
  VisualElement m_LableContainer = new VisualElement();
  m_LableContainer.name = "TextLableContainer";
  m_LableContainer.AddToClassList("TextLableContainer");
  m_Container.Add(m_LableContainer);

  Label m_LableText = new Label();
  m_LableText.name = "TextLable";
  m_LableText.text = "Test label text.";
  m_LableText.AddToClassList("TextLable");
  m_LableContainer.Add(m_LableText);

  #endregion
  
  SignpostUIPopupMessage m_SignpostText = GameObject.Find("Signpost Assembly V2").GetComponent<SignpostUIPopupMessage>();

  DataBinding m_OnScreenText = new DataBinding
  {
   dataSource = m_SignpostText,
   dataSourcePath = new Unity.Properties.PropertyPath("SignText"),
   bindingMode = BindingMode.ToTarget,
  };
  
  m_OnScreenText.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
  //m_OnScreenText.sourceToUiConverters.AddConverter( (ref float value) => $"Health: {value:F2}");
  m_LableText.SetBinding("text", m_OnScreenText);
 }
 
 private void Start()
 {
  m_SignpostUIPopupMessage.OnMessageRead += Handle_OnMessageRead;
 }

 private void OnDestroy()
 {
   m_SignpostUIPopupMessage.OnMessageRead -= Handle_OnMessageRead;
 }

 private void Handle_OnMessageRead(string SignpostTextMessage)
 {
   Debug.Log(SignpostTextMessage);
   m_Document.GetComponent<Label>().text = SignpostTextMessage;
 }
}



