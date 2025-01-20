using UnityEngine;

namespace GameLogic.Components
{
    public class Repositioner : MonoBehaviour
    {
        public Maze maze;

        public void Start() => 
            transform.SetPositionAndRotation(GeneratePosition(), Quaternion.identity);

        public void OnCollisionEnter2D(Collision2D _) => 
            transform.SetPositionAndRotation(GeneratePosition(), Quaternion.identity);

        private Vector3 GeneratePosition()
        {
            var (xDim, yDim) = maze.Dimensions();
            var x = Random.Range(0, xDim);
            var y = Random.Range(0, yDim);
            
            Vector3 pos = maze.CorridorCenter((x, y));
            pos.x += 0.5f;
            pos.y += 0.5f;
            return pos;
        }
    }
}