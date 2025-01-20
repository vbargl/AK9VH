using System;
using System.Collections.Generic;
using static System.Linq.Enumerable;
using Random = UnityEngine.Random;

namespace GameLogic.Utils
{
    public static class EnumerableUtils
    {
        public static IEnumerable<Vector3IntWrapper> Foreach(IEnumerable<int> xSize, IEnumerable<int> ySize)
        {
            foreach (int x in xSize)
            foreach (var y in ySize)
                yield return new Vector3IntWrapper(x, y);
        }

        public static IEnumerable<Vector3IntWrapper> Foreach(int xSize, int ySize) =>
            Foreach(Range(0, xSize), Range(0, ySize));
        
        public static IEnumerable<(int, int)> ForeachInclusive(Range x, Range y)
        {
            for (var xi = x.Start.Value; xi <= x.End.Value; xi++)
            for (var yi = y.Start.Value; yi <= y.End.Value; yi++)
                yield return (xi, yi);
        }
    }
}