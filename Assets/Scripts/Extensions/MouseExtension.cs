using UnityEngine;

namespace BeeGood.Extensions
{
    public static class MouseExtension
    {
        public static Vector3 GetMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hitInfo, Mathf.Infinity) ? hitInfo.point : Vector3.zero;
        }
    }
}