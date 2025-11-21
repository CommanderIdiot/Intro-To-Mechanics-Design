using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuHandler : MonoBehaviour
{
    private UIDocument m_UIDocument;
    
    private Button m_Assignment1Button;
    private Button m_Assignment2Button;
    private Button m_ExitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        m_UIDocument = GetComponent<UIDocument>();

        if (m_UIDocument == null)
        {
            Debug.LogError("No UIDocument found");
        }

        m_Assignment1Button = m_UIDocument.rootVisualElement.Q<Button>("A1Btn");
        m_Assignment2Button = m_UIDocument.rootVisualElement.Q<Button>("A2Btn");
        m_ExitButton = m_UIDocument.rootVisualElement.Q<Button>("ExitBtn");
    }

    private void Handle_PlayAssignment1(ClickEvent OnClicked)
    {
        Debug.Log("Assignment 1 was clicked.");
        //SceneManager.LoadScene("Assignment1");
    }
    
    private void Handle_PlayAssignment2(ClickEvent OnClicked)
    {
        Debug.Log("Assignment 2 was clicked.");
        //SceneManager.LoadScene("Assignment2");
    }
    
    private void Handle_ExitGame(ClickEvent OnClicked)
    {
        Debug.Log("Exit was clicked.");
    }

    private void OnEnable()
    {
        m_Assignment1Button.RegisterCallback<ClickEvent>(Handle_PlayAssignment1);
        m_Assignment2Button.RegisterCallback<ClickEvent>(Handle_PlayAssignment2);
        m_ExitButton.RegisterCallback<ClickEvent>(Handle_ExitGame);
    }
}
