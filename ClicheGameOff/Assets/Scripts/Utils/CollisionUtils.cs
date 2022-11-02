using UnityEngine;

namespace Utils
{
    public static class CollisionUtils
    {
        public static bool CheckLayerCollision(LayerMask collisionLayer, GameObject other) => (collisionLayer.value & (1 << other.layer)) > 0;
    
        public static Vector3 GetRandomPointWithBox(BoxCollider boxCollider)
        {
            var bounds = boxCollider.bounds;
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
        }

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
