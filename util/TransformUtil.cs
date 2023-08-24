using System;
using System.Collections.Generic;
using System.Linq;
using animals.scripts.util;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace util
{
    public static class TransformUtil
    {
        public static List<T> GetComponentsInSameRoot<T>(this Transform transform) where T : Component
        {
         

            return transform.root.GetComponentsInChildrenExcludeSelf<T>();
        }
        public static List<T> GetComponentsInChildrenExcludeSelf<T>(this Transform transform) where T : Component
        {
            var tList = new List<T>();
            foreach (Transform child in transform)
            {
                var scripts = child.GetComponentsInChildren<T>();
                if (scripts != null)
                    foreach (var sc in scripts)
                        tList.Add(sc);
            }

            return tList;
        }
         public static List<Transform> GetChildren(this Transform transform) 
        {
            var tList = new List<Transform>();
            foreach (Transform child in transform)
            {
                tList.Add(child);
                      
            }

            return tList;
        }
        
        public static void SetFirstChildActive(this Transform transform, bool active)
        {
            transform.GetChild(0).gameObject.SetActive(active);
        }

        public static void ToggleFirstChildActive(this Transform transform)
        {
            var go = transform.GetChild(0).gameObject;

            go.SetActive(!go.activeSelf);
        }
        public static Transform FindChildByNameRecursive(this Transform transform, string name)
        {
            foreach (Transform child in transform)
                if (child.name == name)
                    return child;
            Debug.LogError("didnt find child " + name);
            return null;
        }

        public static Transform FindChildByNameRecursive(this GameObject go, string name)
        {
            foreach (var child in go.transform.GetComponentsInChildren<Transform>(true))
                if (child.name == name)
                    return child;
            Debug.LogError("didnt find child " + name);
            return null;
        }

        public static void DestroyChildrenAndSelf(this Transform transform)
        {
            foreach (Transform chTransform in transform) GameObject.Destroy(chTransform.gameObject);
            GameObject.Destroy(transform.gameObject);
        }
        public static Transform ClearChildren(this Transform transform)
        {
            var initialCnt = transform.childCount;
            for (int i = 0; i < initialCnt; i++)
            {
                if (transform.GetChild(0).gameObject != null)
                {
                    if (Application.isPlaying)
                    {
                        Object.Destroy(transform.GetChild(0).gameObject);

                    }
                    else
                    {
                        Object.DestroyImmediate(transform.GetChild(0).gameObject);
                    }
                }
            }
            foreach (Transform child in transform)
            {
               
            }
            return transform;
        } 
        public static Transform SetChildrenActive(this Transform transform,bool active)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject != null)
                {
                   child.gameObject.SetActive(active);
                }
            }
            return transform;
        }
        public static T EnsureComponent<T>(this Transform transform)where T : Component
        {
            T comp;
            if (!transform.TryGetComponent<T>(out comp))
            {
              comp=  transform.gameObject.AddComponent<T>();
            }
            return comp;
        }  
        public static List<T> EnsureComponents<T>(this Transform transform,int num=2)where T : Component
        {
            var tList=transform.GetComponents<T>().ToList();
            if (tList .Count== num)
            {
            }
            else if (tList.Count > num)
            {
                //动态改变和判断,先读出来
                var cnt = tList.Count;

                for (int i = 0; i < cnt - num; i++)
                {
                    var comp = tList[0];
                    tList.Remove(comp);
                    Object.Destroy(comp);
                }
            }
            else
            {
                //动态改变和判断,先读出来
                var cnt = tList.Count;
                for (int i = 0; i < num-cnt; i++)
                {
                    tList.Add(transform.AddComponent<T>());
                  
                }
            }
            
            return tList;
        } 
        public static T EnsureComponent<T>(this Transform transform,out T t)where T : Component
        {
            
            return t= transform.EnsureComponent<T>();
        } 
        public static Component EnsureComponent(this Transform transform,Type type)
        {
            Component comp;
            if (!transform.TryGetComponent(type,out comp))
            {
              comp=  transform.gameObject.AddComponent(type);
            }
            return comp;
        }

        public static void LookAtTargetExcludeY(this Transform transform,Vector3 target)
        {
            transform.rotation = Quaternion.LookRotation((target - transform.position).ExcludeY());

        }
        public static void LookAtDirExcludeY(this Transform transform,Vector3 dir)
        {
            transform.rotation = Quaternion.LookRotation((dir).ExcludeY());

        }
    }
}