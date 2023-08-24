using UnityEngine;

namespace util
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public bool dontDestroy = true;

        private void Awake()
        {
            if (dontDestroy) DontDestroyOnLoad(gameObject);
        }
    }
}