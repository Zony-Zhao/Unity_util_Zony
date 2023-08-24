using UnityEngine;

namespace util
{
    [ExecuteInEditMode]
    public class FaceOrthographicCamera : MonoBehaviour
    {
        private new Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (camera != null)
                transform.rotation = camera.transform.rotation;
            else
                camera = Camera.main;
        }
    }
}