using UnityEngine;

namespace util
{
    public static class   RandomUtil
    {

        public static Vector3 NormalizedOnPlane()
        {
            return new Vector3(Random.value - .5f, 0, Random.value - .5f).normalized;
        }
    }
}