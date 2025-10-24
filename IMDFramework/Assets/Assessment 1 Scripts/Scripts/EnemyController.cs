using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform m_Player;
    private Rigidbody2D m_RB;


    private void awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector2 DirectionToPlayer = m_Player.position - transform.position;
        
        float Dot = Vector2.Dot(Vector2.right, DirectionToPlayer.normalized);

        if (Dot > 0)
        {
            m_RB.linearVelocityX = 2;
        }
        
    }
}
