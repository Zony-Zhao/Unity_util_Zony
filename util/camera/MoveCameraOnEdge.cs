using animals.scripts.util;
using UnityEngine;

namespace util.camera
{
    public class MoveCameraOnEdge : MonoBehaviour
    {
 
        public int boundary= 50;
        public float speed  = 5;
 
        private int theScreenWidth ;
        private int theScreenHeight ;
        private Camera cam;
        void Start() 
        {
            theScreenWidth = Screen.width;
            theScreenHeight = Screen.height;
            cam=CameraUtil.GetMainCamera();
        }
 
        void Update() 
        {
            if (!Application.isFocused)
            {
                return;
            }
            if (Input.mousePosition.x > theScreenWidth - boundary&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                cam.transform.position+=Vector3.right* speed * Time.deltaTime;
            }
     
            if (Input.mousePosition.x < 0 + boundary&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                cam.transform.position+=Vector3.left* speed * Time.deltaTime;
            }
     
            if (Input.mousePosition.y > theScreenHeight - boundary&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                cam.transform.position+=Vector3.forward* speed * Time.deltaTime;
            }
     
            if (Input.mousePosition.y < 0 + boundary&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                cam.transform.position+=Vector3.back* speed * Time.deltaTime;
            }
     
        }    
 
        void OnGUI() 
        {
        }
    }
}