using System;
using animals.scripts.util;
using common;
using UnityEngine;

namespace util
{
    [ExecuteAlways]
    public class ScreenCursorToWorld : Singleton<ScreenCursorToWorld>
    {
        //?可能0.5比较好?
        public static float cursorHeight = 0f;

        private void Update()
        {
            var cam = CameraUtil.GetMainCamera();
            // if (InCourtGameManager.instance?.cam== null)
            // {
            //     Debug.LogWarning("未设置InCroutGameManager cam");
            // }
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, 1 << 7 /*layer-ground*/))
                transform.position = raycastHit.point + Vector3.up * cursorHeight;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, .5f);
        }
    }
}