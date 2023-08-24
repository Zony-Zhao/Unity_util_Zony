using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace util
{
    public static class DistanceUtil
    {
        public static T FindNearest<T>(this Transform transform,List<T> list)where  T:Component
        {
            return list.OrderBy(p => (transform.position - p.transform.position).magnitude).ToList()
                .ElementAtOrDefault(0);
        }
    }
}