using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic.Utils
{
    public class CollectionUtils
    {
        public static T Random<T>(params T[] array)
        {
            var idx = UnityEngine.Random.Range(0, array.Length);
            return array[idx];
        }

        public static T RandomList<T>(IList<T> list)
        {
            if (list.Count == 0)
                throw new ArgumentException("empty list");
            var idx = UnityEngine.Random.Range(0, list.Count);
            return list[idx];
        }
    }
}