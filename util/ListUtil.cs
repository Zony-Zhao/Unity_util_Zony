using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace util
{
    public static class            ListUtil
    {
        public static T RandomElement<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public static T RandomInstantiatedElement<T>(this IList<T> list)where T:Component
        {
            return GameObject.Instantiate(list.RandomElement());
        }
        public static List<T> RandomDifferentElements<T>(this IList<T> list,int number)
        {
            var copy = new List<T>(list);
            var returnList = new List<T>();
            if (number > list.Count)
            {
                throw new Exception("list should have more elements than num required");
            }
            for (int i = 0; i < number; i++)
            {
                var element = copy[UnityEngine.Random.Range(0, copy.Count)];
                copy.Remove(element);
                returnList.Add(  element);
            }
            return returnList;
        }
        public static List<T> RandomDifferentInstantiatedElements<T>(this IList<T> list,int number) where T:Component
        {
            return list.RandomDifferentElements( number).Select(e => GameObject.Instantiate(e)).ToList();
        }
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
            return list;
        }

        public static void ForEachIndex<T>(this IList<T> list, Action<T, int> action)
        {
            for (var i = 0; i < list.Count; i++) action(list[i], i);
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            return list.OrderBy(e => Random.value).ToList();
        }
    }
}