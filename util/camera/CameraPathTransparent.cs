using System.Collections.Generic;
using UnityEngine;

namespace util.camera
{
    public class CameraPathTransparent : MonoBehaviour
    {
        public Transform target;

        public Material transparentMat;


        private readonly List<MeshRenderer> coverings = new List<MeshRenderer>();

        private readonly List<Material> originalMats = new List<Material>();

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            var position = transform.position;
            var direction = target.position - position;
            RaycastHit hitInfo;
            var hitted = Physics.Raycast(position, direction, out hitInfo,
                direction.magnitude, 1 << 6); /*layer-cover*/
            if (hitted)
            {
                var go = hitInfo.collider.gameObject;
                var covering = go.GetComponent<MeshRenderer>();
                if (!coverings.Exists(c => c.gameObject.GetInstanceID() == go.GetInstanceID()))
                {
                    coverings.Add(covering);
                    originalMats.Add(covering.material);
                    covering.material = transparentMat;
                }
                //已经存在了
            }
            //没撞到任何东西,如果多次连续撞到,或导致整个list透明
            else
            {
                var i = 0;

                coverings.ForEach(c => c.material = originalMats[i++]);
                coverings.Clear();
                originalMats.Clear();
            }
        }
    }
}