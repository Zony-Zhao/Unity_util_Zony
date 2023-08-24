using UnityEngine;

namespace animals.scripts.util
{
    public static class MathfUtil
    {
        public static int GetPositivityIndicator(this float num)
        {
            if (num == 0)
            {
                return 0;
            }

            return num / Mathf.Abs(num) > 0 ? 1 : -1;
        }

        public static int RoundToInt(this float num)
        {
            return Mathf.RoundToInt(num);
        }
    }
}