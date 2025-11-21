using Assessment_1_Scripts.Scripts;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI_Elements_Demo.Adv_UI_Elements_Demo
{
    public class GameUIController : MonoBehaviour
    {
        private void Awake()
        {
            #region UI Creator

            VisualElement m_Container = new VisualElement();
            m_Container.name = "MainContainer";
            GetComponent<UIDocument>().rootVisualElement.Add(m_Container);
            
            VisualElement m_LableContainer = new VisualElement();
            m_LableContainer.name = "LableContainer";
            m_LableContainer.AddToClassList("PositionLabelContainer");
            m_Container.Add(m_LableContainer);
            
            Label m_HealthBar = new Label();
            m_HealthBar.name = "Health Label";
            m_HealthBar.text = "Health: ____";
            m_HealthBar.AddToClassList("HealthLabel");
            m_LableContainer.Add(m_HealthBar);
            
            Label m_XLabel = new Label();
            m_XLabel.name = "X Label";
            m_XLabel.text = "X: ____";
            m_XLabel.AddToClassList("XPositionLabel");
            m_LableContainer.Add(m_XLabel);
            
            Label m_YLabel = new Label();
            m_YLabel.name = "Y Label";
            m_YLabel.text = "Y: ____";
            m_YLabel.AddToClassList("YPositionLabel");
            m_LableContainer.Add(m_YLabel);

            #endregion

            HealthComponent m_PlayerCurrentHealth = GameObject.Find("PlayerPrefab").GetComponent<HealthComponent>();

            DataBinding m_HealthBarBinding = new DataBinding
            {
                dataSource = m_PlayerCurrentHealth,
                dataSourcePath = new Unity.Properties.PropertyPath("PlayerCurrentHealth"),
                bindingMode = BindingMode.ToTarget,
            };

            m_HealthBarBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
            m_HealthBarBinding.sourceToUiConverters.AddConverter( (ref float value) => $"Health: {value:F2}");
            m_HealthBar.SetBinding("text", m_HealthBarBinding);

            
            TransformWrapper m_PlayerTransform = GameObject.Find("PlayerPrefab").GetComponent<TransformWrapper>();

            DataBinding m_xBinding = new DataBinding
            {
                dataSource = m_PlayerTransform,
                dataSourcePath = new Unity.Properties.PropertyPath("XPos"),
                bindingMode = BindingMode.ToTarget,
            };

            m_xBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
            m_xBinding.sourceToUiConverters.AddConverter( (ref float value) => $"X: {value:F2}");
            m_XLabel.SetBinding("text", m_xBinding);

            DataBinding m_yBinding = new DataBinding
            {
                dataSource = m_PlayerTransform,
                dataSourcePath = new Unity.Properties.PropertyPath("YPos"),
                bindingMode = BindingMode.ToTarget,
            };
            
            m_yBinding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;
            m_yBinding.sourceToUiConverters.AddConverter( (ref float value) => $"Y: {value:F2}");
            m_YLabel.SetBinding("text", m_yBinding);
        }
    }
}

