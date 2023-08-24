using animals.scripts.util;
using UnityEngine;

namespace util
{
    public class UIFollowObject : MonoBehaviour
    {
        public Vector2 offset;
        public Transform objTransform;
        private RectTransform rectTransform;
        private Camera camera;
        protected void Start()
        {
            camera = CameraUtil.GetMainCamera();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (camera == null)
            {
                Debug.LogWarning("no camera");
                return;
            }

            var temp = camera.WorldToScreenPoint(objTransform.position);
            var vector2 = (Vector2) temp + offset;
            rectTransform.position = vector2;
        }
    }
}