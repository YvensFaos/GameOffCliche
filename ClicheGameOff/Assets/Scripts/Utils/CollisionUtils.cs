using UnityEngine;

namespace Utils
{
    public static class CollisionUtils
    {
        public static bool CheckLayerCollision(LayerMask collisionLayer, GameObject other) => (collisionLayer.value & (1 << other.layer)) > 0;
        
        public static bool GetValidPointInLayer(Vector3 currentPoint, Vector3 direction, float distance,
            LayerMask layer, out Vector3 validPoint)
        {
            validPoint = Vector3.zero;
            if (!Physics.Raycast(currentPoint, direction, out RaycastHit hit, distance, layer)) return false;
            validPoint = hit.point;
            return true;
        }
    }
}
