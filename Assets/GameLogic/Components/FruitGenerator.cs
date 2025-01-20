using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GameLogic.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace GameLogic.Components
{
    public class FruitGenerator : MonoBehaviour
    {
        public Maze maze;
        public ScoreCounter scoreCounter;
        
        public GameObject prefab;
        
        [Range(1.0f, 50.0f)]
        public int numberOfFruits;

        public List<Sprite> availableFruitSprites;

        public void Start()
        {
            for (var i = 0; i < numberOfFruits; i++)
                GenerateFruit();
        }
        
        private void GenerateFruit()
        {
            var fruit = Instantiate(prefab, transform);
            
            var spriteRenderer = fruit.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = CollectionUtils.RandomList(availableFruitSprites);
            
            var repositioner = fruit.GetComponent<Repositioner>();
            repositioner.maze = maze;
            
            var collisionEmitter = fruit.GetComponent<CollisionEventEmitter>();
            collisionEmitter.events.AddListener(scoreCounter.AddScore);
        }
    }
}