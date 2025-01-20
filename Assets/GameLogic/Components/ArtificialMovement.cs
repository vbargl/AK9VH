using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.Domain;
using GameLogic.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.Components
{
    [RequireComponent(typeof(Maze))]
    public class ArtificialMovement : MonoBehaviour
    {
        public Transform target;
        private Maze _maze;

        private Direction[,] _data;
        private Vector3IntWrapper _originPosition;

        private MazeState _lastState = MazeState.Idle;

        [Range(0.2f, 3f)]
        public float wanderingSpeed;

        [Range(0.2f, 3f)]
        public float surroundingSpeed;

        public void Start()
        {
            _maze = GetComponent<Maze>();
            StartCoroutine(Routine());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator Routine()
        {
            _lastState = MazeState.Idle;
            yield return Wander();
            
            while (true)
            {
                yield return CollectionUtils.RandomList(AvailableStates()) switch
                {
                    MazeState.Idle => BeIdle(),
                    MazeState.Wandering => Wander(),
                    MazeState.Surrounding => Surround(),
                    MazeState.Luring => Lure(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        private List<MazeState> AvailableStates()
        {
            var list = new List<MazeState>();
            if (_lastState != MazeState.Idle)
                list.Add(MazeState.Idle);
            if (_lastState != MazeState.Wandering)
                list.Add(MazeState.Wandering);
            if (_lastState != MazeState.Surrounding)
                list.Add(MazeState.Surrounding);
            if (_lastState != MazeState.Luring)
                list.Add(MazeState.Luring);
            return list;
        }
        
        private IEnumerator BeIdle()
        {
            _lastState = MazeState.Idle;
            var durationSec = Random.Range(5, 10);
            Debug.Log($"Maze state going to idle for {durationSec}s");
            
            yield return new WaitForSeconds(durationSec);
        }
        
        private IEnumerator Wander()
        {
            _lastState = MazeState.Wandering;
            var durationSec = Random.Range(5, 20);
            Debug.Log($"Maze is going to wander for: {durationSec}s");

            var steps = Mathf.CeilToInt(durationSec / wanderingSpeed); // not precise
            for(var i = 0; i < steps; i++)
            {
                var direction = CollectionUtils.RandomList(_maze.AvailableDirections());
                _maze.Move(direction);
                yield return new WaitForSeconds(wanderingSpeed);
            }
        }

        private IEnumerator Surround()
        {
            _lastState = MazeState.Surrounding;
            var durationSec = Random.Range(3, 7);
            Debug.Log($"Maze is going to surround player for: {durationSec}s");
            
            var steps = Mathf.CeilToInt(durationSec / surroundingSpeed); // not precise
            for(var i = 0; i < steps; i++)
            {
                var originPos = _maze.OriginPosition();
                var targetPos = target.position;
                
                // diff is distance vector from origin to target
                // +x target is right, -x target is left
                // +y target is up, -y target is down
                var diff = targetPos - originPos;

                // find out which axis has bigger difference
                if (Math.Abs(diff.x) > Math.Abs(diff.y))
                    _maze.Move(diff.x > 0 ? Direction.Right : Direction.Left);
                else
                    _maze.Move(diff.y > 0 ? Direction.Up : Direction.Down);
                
                yield return new WaitForSeconds(surroundingSpeed);
            }
        }

        private IEnumerator Lure()
        {
            _lastState = MazeState.Luring;
            var duration = Random.Range(1, 3);
            Debug.Log($"Maze is going lure player for {duration}s");
            
            _maze.wallTilemap.gameObject.SetActive(false);
            yield return new WaitForSeconds(duration);
            _maze.wallTilemap.gameObject.SetActive(true);
        }
    }
}