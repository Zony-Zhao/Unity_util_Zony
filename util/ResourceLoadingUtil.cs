using UnityEngine;

namespace animals.scripts.util
{
    public static class ResourceLoadingUtil
    {
        public static Sprite LoadSprite(this string path)
        {
            return Resources.LoadAll(path)[1] as Sprite;
        }
    }
}