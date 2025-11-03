using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AdvDemoUIScript : MonoBehaviour
{
    private VisualElement m_UIRoot;
    private ScrollView m_ScrollViewArea;
    
    private void Awake()
    {
        #region Root Element Creation
    
        VisualElement RootElement = new VisualElement();
        RootElement.name = "AdvDemoUIContainer";

        GetComponent<UIDocument>().rootVisualElement.Add(RootElement);
        m_UIRoot = RootElement;
    
        #endregion

        Button DemoButton = new Button();
        DemoButton.text = "Press me now!";
        DemoButton.name = "Button_LableSpawner";
        DemoButton.RegisterCallback<ClickEvent>(Handle_LableSpawn);
        DemoButton.AddToClassList("AdvDemoUIButton");
        m_UIRoot.Add(DemoButton);
    }

    private void Handle_LableSpawn(ClickEvent OnClickedEvent)
    {

        #region Create Scroll View

        Debug.Log("Button spawned.");

        if (m_ScrollViewArea == null)
        {
            ScrollView ScrollList = new ScrollView();
            ScrollList.name = "SCR_LabelContainer";
            ScrollList.AddToClassList("ScrollView");
            
            m_UIRoot.Add(ScrollList);
            m_ScrollViewArea = ScrollList;
        }

        #endregion

        Label ScrollViewLabel = new Label();
        ScrollViewLabel.name = "LBL_RandomNumberLabel";
        ScrollViewLabel.text = Random.Range(Int32.MinValue, Int32.MaxValue).ToString();
        ScrollViewLabel.AddToClassList("Label");
        m_ScrollViewArea.Add(ScrollViewLabel);

    }
}
