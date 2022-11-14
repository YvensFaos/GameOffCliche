using UnityEngine;

namespace Utils
{
    public static class TransformUtils
    {
        public static void ClearObjects(Transform transform)
        {
            var children = transform.childCount;
            for (var i = children - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public static Vector3 GetPointOnTheGround(Vector3 point, LayerMask floorMask, float maxDistance = 100.0f)
        {
            return Physics.Raycast(point, Vector3.down, out RaycastHit hit, maxDistance, floorMask) ? hit.point : point;
        }
    }
}