using UnityEngine;

public class Signpost : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_SignpostDisplayObject;

    public void Interact()
    { 
        m_SignpostDisplayObject.SetActive(!m_SignpostDisplayObject.activeSelf);
    }
}
