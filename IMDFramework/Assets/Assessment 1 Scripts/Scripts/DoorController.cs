using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_DoorObject;
    
    [SerializeField] private int m_DoorCreekSound;

    [SerializeField] private AudioComponent m_AudioComponent;
    public void Interact()
    {
        m_DoorObject.SetActive(!m_DoorObject.activeSelf);

        m_AudioComponent.PlaySound(m_DoorCreekSound);
    }
}
