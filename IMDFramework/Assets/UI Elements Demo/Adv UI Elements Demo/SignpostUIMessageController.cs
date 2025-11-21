using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SignpostUIMessageController : MonoBehaviour
{
 private UIDocument m_Document;
 public void Awake()
 {
  #region UI Creation

  VisualElement m_Container = new VisualElement();
  m_Container.name = "TextUIContainer";
  GetComponent<UIDocument>().rootVisualElement.Add(m_Container);
            
  VisualElement m_LableContainer = new VisualElement();
  m_LableContainer.name = "TextLableContainer";
  m_LableContainer.AddToClassList("TextLableContainer");
  m_Container.Add(m_LableContainer);

  VisualElement m_LableText = new VisualElement();
  m_LableText.name = "TextLable";
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
  m_OnScreenText.sourceToUiConverters.AddConverter( (ref float value) => $"Health: {value:F2}");
  m_LableText.SetBinding("text", m_OnScreenText);
 }
}
