using UnityEngine;

namespace util.camera
{
    public class UnInheritRotation : MonoBehaviour
    {
        // Start is called before the first frame update
        private Vector3 parentRotation = Vector3.zero;
        private Vector3 parentStartRotation;

        private void Start()
        {
            parentStartRotation = transform.parent.rotation.eulerAngles;
        }

        // Update is called once per frame
        private void Update()
        {
            // 加上父级的旋转,去正常的计算
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + parentRotation);
            // 记录父级的旋转
            parentRotation = transform.parent.rotation.eulerAngles - parentStartRotation;
        }

        private void LateUpdate()
        {
            //减去父级旋转,得到自己
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - parentRotation);
        }
    }
}