using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_DoorObject;
        
    public void Interact()
    {
        m_DoorObject.SetActive(!m_DoorObject.activeSelf);
    }
}
