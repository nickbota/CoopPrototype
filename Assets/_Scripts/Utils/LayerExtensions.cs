using UnityEngine;

namespace CoopPrototype.Utils
{
    public static class LayerExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) > 0;
        }

        public static bool IsInLayerMask(this GameObject obj, LayerMask mask)
        {
            return (mask.value & (1 << obj.layer)) > 0;
        }
    }
}
