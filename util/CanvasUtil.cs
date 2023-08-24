using UnityEngine;

namespace animals.scripts.util
{
    public static class  CanvasUtil
    {
        private static Canvas main;

        public static Canvas GetMainCanvas()
        {
            if (main != null)
                return main;
            return main = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        }
    }
}