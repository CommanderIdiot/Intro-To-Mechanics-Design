using Unity.Properties;
using UnityEngine;

namespace UI_Elements_Demo.Adv_UI_Elements_Demo
{
    public class TransformWrapper : MonoBehaviour
    {
        [CreateProperty] 
        public float XPos { get { return transform.position.x; } }
        
        [CreateProperty] 
        public float YPos { get { return transform.position.y; } }
    }
}
