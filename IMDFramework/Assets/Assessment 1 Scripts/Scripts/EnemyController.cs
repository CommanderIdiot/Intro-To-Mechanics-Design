using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform m_Player;
    private Rigidbody2D m_RB;


    private void awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }
}
