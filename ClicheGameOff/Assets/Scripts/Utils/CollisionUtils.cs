using UnityEngine;

public static class CollisionUtils
{
    public static bool CheckLayerCollision(LayerMask collisionLayer, GameObject other) => (collisionLayer.value & (1 << other.layer)) > 0;    
}
