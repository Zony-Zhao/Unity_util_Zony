using UnityEngine;

namespace util
{
    public class SelfDestroy : MonoBehaviour
    {
        public float timeToLive = 1f;
        protected float bornTime;

        protected virtual void Start()
        {
            bornTime = 0;
        }

        protected virtual void Update()
        {
            bornTime += Time.deltaTime;
            if (bornTime >= timeToLive) Destroy(gameObject);
        }
    }
}