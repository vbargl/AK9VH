using UnityEngine;

namespace GameLogic.Utils
{
    public class Vector3IntWrapper
    {
        public Vector3IntWrapper(int x = 0, int y = 0, int z = 0)
        {
            Vector3Int = new Vector3Int(x, y, z);
        }

        public Vector3IntWrapper(Vector3Int v)
        {
            Vector3Int = v;
        }
        
        public Vector3IntWrapper(Vector2Int v)
        {
            Vector3Int = new Vector3Int(v.x, v.y);
        }

        public Vector3Int Vector3Int { get; private set; }
        
        public int x => Vector3Int.x;
        public int y => Vector3Int.y;
        public int z => Vector3Int.z;
        
        public override string ToString() => Vector3Int.ToString();

        public static implicit operator Vector3IntWrapper(Vector3Int sb)
        {
            return new Vector3IntWrapper(sb);
        }

        public static Vector3IntWrapper operator *(Vector3IntWrapper w, Vector3IntWrapper o) =>
            w.Vector3Int * o.Vector3Int;

        public static Vector3IntWrapper operator *(int scalar, Vector3IntWrapper w) =>
            w.Vector3Int * new Vector3Int(scalar, scalar);

        public static Vector3IntWrapper operator *(Vector3IntWrapper w, int scalar) =>
            w.Vector3Int * new Vector3Int(scalar, scalar);

        public static Vector3IntWrapper operator +(Vector3IntWrapper w, Vector3IntWrapper o) =>
            w.Vector3Int + o.Vector3Int;

        public static Vector3IntWrapper operator +(int scalar, Vector3IntWrapper sbw) =>
            sbw.Vector3Int + new Vector3Int(scalar, scalar);

        public static Vector3IntWrapper operator +(Vector3IntWrapper w, int scalar) =>
            w.Vector3Int + new Vector3Int(scalar, scalar);
    }
}