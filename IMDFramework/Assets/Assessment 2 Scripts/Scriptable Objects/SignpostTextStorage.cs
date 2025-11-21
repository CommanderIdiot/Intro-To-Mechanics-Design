using UnityEngine;

[CreateAssetMenu(fileName = "SignpostTextStorage", menuName = "Scriptable Objects/SignpostTextStorage")]
public class SignpostTextStorage : ScriptableObject
{
    [Header("Text")]
    [SerializeField] public string m_TextStorage;
}
