using UnityEngine;

namespace util.camera
{
    public class FollowParent : MonoBehaviour
    {
        private Vector3 diff;
        private Transform parent;

        public void Start()
        {
            parent = transform.parent;
            diff = transform.position - parent.transform.position;
        }

        public void LateUpdate()
        {
            transform.position = parent.transform.position + diff;
        }
    }
}