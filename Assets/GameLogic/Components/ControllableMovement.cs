using UnityEngine;

namespace GameLogic.Components
{
    public class ControllableMovement : MonoBehaviour
    {
        private static readonly int MovementY = Animator.StringToHash("MovementY");
        private static readonly int MovementX = Animator.StringToHash("MovementX");

        [Range(100.0f, 500.0f)] 
        public float speed;

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Vector2 _direction;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            Move();
            Animate();
        }

        private void Move()
        {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.y = Input.GetAxis("Vertical");
            
            _rigidbody.linearVelocity = _direction * (speed * Time.fixedDeltaTime);   
        }

        private void Animate()
        {
            _animator.SetFloat(MovementX, _direction.x);
            _animator.SetFloat(MovementY, _direction.y);
        }
    }
}