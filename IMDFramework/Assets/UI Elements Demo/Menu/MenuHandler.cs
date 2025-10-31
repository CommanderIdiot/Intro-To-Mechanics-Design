using UnityEngine;
using UnityEngine.UIElements;

public class MenuHandler : MonoBehaviour
{
    private UIDocument m_UIDocument;
    
    private Button m_PlayButton;
    private Button m_ExitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        m_UIDocument = GetComponent<UIDocument>();

        if (m_UIDocument == null)
        {
            Debug.LogError("No UIDocument found");
        }

        m_PlayButton = m_UIDocument.rootVisualElement.Q<Button>("PlayBtn");
        m_ExitButton = m_UIDocument.rootVisualElement.Q<Button>("ExitBtn");
    }

    private void Handle_PlayGame(ClickEvent OnClicked)
    {
        Debug.Log("Play was clicked.");
    }
    
    private void Handle_ExitGame(ClickEvent OnClicked)
    {
        Debug.Log("Play was clicked.");
    }

    private void OnEnable()
    {
        m_PlayButton.RegisterCallback<ClickEvent>(Handle_PlayGame);
        m_ExitButton.RegisterCallback<ClickEvent>(Handle_ExitGame);
    }
}
