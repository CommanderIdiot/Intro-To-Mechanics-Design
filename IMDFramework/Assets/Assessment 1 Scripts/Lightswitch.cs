using UnityEngine;

public class Lightswitch : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_LightObject;
    
    public void Interact()
    {
        m_LightObject.SetActive(!m_LightObject.activeSelf);
    }
}
