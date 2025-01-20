using System;
using System.Collections.Generic;
using System.IO;
using GameLogic.Domain;
using GameLogic.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using static System.Linq.Enumerable;
using static GameLogic.Utils.EnumerableUtils;

namespace GameLogic.Components
{
    public class Maze : MonoBehaviour
    {
        // Represents grid and config how to manipulate with grid.
        public Vector3Int offset;
        public Vector3Int dimensions;

        // Manipulation with tiles
        private Grid _grid;
        public Tilemap wallTilemap;
        public Tile wallTile;
        public Tilemap originTilemap;
        public Tile originTile;

        // Corridor size configuration
        [Range(1.0f, 5.0f)] public int corridorSize;
        
        // Initial state configuration
        public bool randomize;
        
        private Direction[,] _data;
        private Vector3Int _dimensions;
        private (int, int) _origin;
        public (int, int) Dimensions() => (_dimensions.x, _dimensions.y);

        public void Start()
        {
            _grid = wallTilemap.layoutGrid;
            _dimensions = new Vector3Int(
                x: CheckSize(dimensions.x),
                y: CheckSize(dimensions.y));

            _data = new Direction[_dimensions.x, _dimensions.y];

            // Fill unreachable blocks with wall tile
            foreach (var pos in ForeachInclusive(..(_dimensions.x - 2), ..(_dimensions.y - 2)))
            {
                var tilePos = offset + pos.Translate(corridorSize) + corridorSize * Vector3Int.one;
                wallTilemap.SetTile(tilePos, wallTile);
            }

            _origin = (0, 0);
            _data.Set(_origin, Direction.None);
            originTilemap.SetTile(CellCenter(_origin), originTile);

            // (x=0,y) column direct down
            foreach (var pos in ForeachInclusive(..0, 1..(_dimensions.y - 1)))
            {
                _data.Set(pos, Direction.Down);
                Open(pos, Direction.Down);
            }

            // (x,y=0) row direct left
            foreach (var pos in ForeachInclusive(1..(_dimensions.x - 1), ..0))
            {
                _data.Set(pos, Direction.Left);
                Open(pos, Direction.Left);
            }

            if (randomize) RandomInitialization();
            else StructuredInitialization();
        }

        private void RandomInitialization()
        {
            // (x>0,y>0) direct either left or down
            foreach (var pos in ForeachInclusive(1..(_dimensions.x - 1), 1..(_dimensions.y - 1)))
            {
                var dir = CollectionUtils.Random(Direction.Left, Direction.Down);
                _data.Set(pos, dir);
                Open(pos, dir);
                switch (dir)
                {
                    case Direction.Left:
                        Close(pos, Direction.Down);
                        break;
                    case Direction.Down:
                        Close(pos, Direction.Left);
                        break;
                }
            }
        }

        private void StructuredInitialization()
        {
            // (x>0,y>0) direct left
            foreach (var pos in ForeachInclusive(1..(_dimensions.x - 1), 1..(_dimensions.y - 1)))
            {
                _data.Set(pos, Direction.Left);
                Open(pos, Direction.Left);
                Close(pos, Direction.Down);
            }
        }

        public Vector3 OriginPosition() => CellCenter(_origin) + _grid.cellSize / 2;

        public void Move(Direction newDir)
        {
            if (newDir == Direction.None) return;

            var newOrigin = _origin.Move(newDir);
            if (!InBounds(newOrigin))
                return;

            _data.Set(_origin, newDir);
            originTilemap.SetTile(CellCenter(_origin), null);
            Close(newOrigin, _data.Get(newOrigin));
            _data.Set(newOrigin, Direction.None);
            originTilemap.SetTile(CellCenter(newOrigin), originTile);
            Open(_origin, newDir);
            _origin = newOrigin;
        }

        public IList<Direction> AvailableDirections()
        {
            var dirs = new List<Direction>();
            var (x, y) = _origin;
            if (x > 0)
                dirs.Add(Direction.Left);
            if (x < _dimensions.x)
                dirs.Add(Direction.Right);
            if (y > 0)
                dirs.Add(Direction.Down);
            if (y < _dimensions.y)
                dirs.Add(Direction.Up);
            return dirs;
        }

        private void Open((int, int) pos, Direction dir)
        {
            if (!InBounds(pos.Move(dir)))
                return;

            wallTilemap.SetTilesBlock(Wall(pos, dir), new TileBase[corridorSize]);
        }

        private void Close((int, int) pos, Direction dir)
        {
            if (!InBounds(pos.Move(dir)))
                return;

            wallTilemap.SetTilesBlock(Wall(pos, dir), Repeat<TileBase>(wallTile, corridorSize).ToArray());
        }

        private bool InBounds((int, int) pos)
        {
            var (x, y) = pos;
            return x >= 0 && x < _dimensions.x &&
                   y >= 0 && y < _dimensions.y;
        }

        private BoundsInt Wall((int, int) pos, Direction dir)
        {
            var position = pos.Translate(corridorSize) + dir.Translate(corridorSize);
            var size = dir switch
            {
                Direction.Up or Direction.Down => new Vector3Int(corridorSize, 1, 1),
                Direction.Left or Direction.Right => new Vector3Int(1, corridorSize, 1),
                _ => throw new NotSupportedException()
            };

            return new BoundsInt(offset + position, size);
        }

        public Vector3Int CellCenter((int, int) pos)
        {
            if (!InBounds(pos))
                return Vector3Int.zero;
            
            var (x, y) = pos;
            return offset + pos.Translate(corridorSize) + corridorSize / 2 * Vector3Int.one;
        }

        private int CheckSize(int size)
        {
            var dimension = (size + 1) / (corridorSize + 1);
            var checksum = (corridorSize + 1) * dimension - 1;
            if (checksum != size)
                throw new InvalidDataException(
                    $"Invalid size, size must be dividable (size + 1)/{corridorSize + 1} == 0!");
            return dimension;
        }
    }

    internal static class DataExtensions
    {
        public static void Set(this Direction[,] array, (int, int) pos, Direction dir)
        {
            var (x, y) = pos;
            array[x, y] = dir;
        }

        public static Direction Get(this Direction[,] array, (int, int) pos)
        {
            var (x, y) = pos;
            return array[x, y];
        }
    }

    internal static class TranslateExtensions
    {
        public static (int, int) Move(this (int, int) pos, Direction dir)
        {
            var (x, y) = pos;
            return dir switch
            {
                Direction.None => pos,
                Direction.Down => (x, y - 1),
                Direction.Up => (x, y + 1),
                Direction.Left => (x - 1, y),
                Direction.Right => (x + 1, y),
                _ => throw new NotSupportedException()
            };
        }

        public static Vector3Int Translate(this (int, int) pos, int corridorSize)
        {
            var (x, y) = pos;
            return new Vector3Int(x, y) * (corridorSize + 1);
        }

        public static Vector3Int Translate(this Direction dir, int corridorSize) => dir switch
        {
            Direction.None => new Vector3Int(0, 0),
            Direction.Up => new Vector3Int(0, corridorSize),
            Direction.Down => new Vector3Int(0, -1),
            Direction.Left => new Vector3Int(-1, 0),
            Direction.Right => new Vector3Int(corridorSize, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}