using UnityEngine;

namespace BeeGood.Managers.Contexts
{
    public class TransformContext : IManagerContext
    {
        public Transform Transform;

        public TransformContext(Transform transform)
        {
            Transform = transform;
        }
    }
}