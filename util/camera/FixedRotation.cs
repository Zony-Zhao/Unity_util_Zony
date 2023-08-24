using UnityEngine;

namespace util.camera
{
    // 保证camera角度不变,结合FollowObject.cs可以放到一个prefab底下,但是固定的spring arm 相机
    //! 这个脚本,只有不是tank子级的时候才有用.
    //如果是角色的子级,当角色旋转,因为相机与角色还有一个距离,虽然自身旋转角度不变,但是相机相对角色的角度变了
    [ExecuteInEditMode]
    public class FixedRotation : MonoBehaviour
    {
        public bool useRotationEuler = true;
        public Vector3 rotationEuler;
        private Quaternion startRotation;

        private void Start()
        {
            //顺序很重要
            if (useRotationEuler)
                transform.rotation = Quaternion.Euler(rotationEuler - transform.parent.rotation.eulerAngles);
            startRotation = transform.rotation;
        }


        private void Update()
        {
        }

        private void LateUpdate()
        {
            //顺序很重要
            transform.rotation = startRotation;
            if (useRotationEuler) transform.rotation = Quaternion.Euler(rotationEuler);
        }
    }
}