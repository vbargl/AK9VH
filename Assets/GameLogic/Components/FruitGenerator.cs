using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace GameLogic.Components
{
    public class FruitGenerator : MonoBehaviour
    {
        public Maze maze;
        
        public Tilemap tilemap;
        public Tile tile;

        private Vector3Int _lastPos;
        
        [Range(1.0f, 50.0f)]
        public int numberOfFruits;

        public void Start()
        {
            for (var i = 0; i < numberOfFruits; i++)
                GenerateFruit();
        }
        
        public void RegenerateFruit()
        {
            DeleteFruit();
            GenerateFruit();
        }
        
        private void GenerateFruit()
        {
            var (xDim, yDim) = maze.Dimensions();
            var x = Random.Range(0, xDim);
            var y = Random.Range(0, yDim);

            var pos = maze.CellCenter((x, y));
            tilemap.SetTile(pos, tile);
            _lastPos = pos;
            Debug.Log($"Fruit generated at {pos}");
        }

        private void DeleteFruit()
        {
            tilemap.SetTile(_lastPos, null);
            _lastPos = new Vector3Int();
        }
    }
}