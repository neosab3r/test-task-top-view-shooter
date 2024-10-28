using UnityEngine;

namespace BeeGood.Extensions
{
    public static class InstantiateExtension
    {
        public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotate) where T: MonoBehaviour
        {
            var obj = Object.Instantiate(prefab, position, rotate);
            return obj;
        }
    }
}