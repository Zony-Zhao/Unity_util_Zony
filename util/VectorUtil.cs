using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace animals.scripts.util
{
    public static class VectorUtil
    {
        public static Vector3 ExcludeY(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0, vector3.z);
        }   
        public static Vector3 GetTopViewV2(this Vector3 vector3)
        {
            return new Vector2(vector3.x,  vector3.z);
        } 
      
        public static Vector2 GetNormalizedVector(this float rad)
        {
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        public static void  AssignPositionExcludeY(this Transform transform, Vector3 vector)
        {
            transform.position = new Vector3(vector.x, transform.position.y, vector.z);
        }

        public static void AssignPosition(this Transform transform, Vector3 vector3, Vector3 weight)
        {
            transform.position = new Vector3(weight.x==0?transform.position.x:vector3.x * weight.x, 
                weight.y==0?transform.position.y:vector3.y * weight.y, 
                weight.z==0?transform.position.z:vector3.z * weight.z);
        }

        public static Vector3 Average(this IEnumerable<Vector3> vectors)
        {
            var enumerable = vectors.ToList();
            return  enumerable.Aggregate(Vector3.zero, (acc, v) => acc + v) / enumerable.Count();
        }
    }
}