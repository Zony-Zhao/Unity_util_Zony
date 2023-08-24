using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace animals.scripts.util
{
    public static class MaterialUtil
    {
        //收集所有mats under this gameobject
        public static List<Material> getChildrenRendererMaterials(this Transform transform)
        {
            var mats = new List<Material>();
            transform.GetComponentsInChildren<Renderer>(true).ToList().ForEach(r => mats=mats.Union(r.materials).ToList());
            return mats;
        }
    }
}