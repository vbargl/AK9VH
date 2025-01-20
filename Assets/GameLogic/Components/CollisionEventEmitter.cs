using UnityEngine;
using UnityEngine.Events;

namespace GameLogic.Components
{
    public class CollisionEventEmitter : MonoBehaviour
    {
        public UnityEvent events;
        private void OnCollisionEnter2D(Collision2D _) => 
            events.Invoke();
    }
}