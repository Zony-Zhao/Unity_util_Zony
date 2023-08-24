using UnityEngine;

namespace animals.scripts.util
{
    public static class CameraUtil
    {
        private static Camera main;

        public static Camera GetMainCamera()
        {
            if (main != null)
                return main;
            return main = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
    }
}