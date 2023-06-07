using UnityEngine;

namespace SpaceTruckBongSimulator
{
    public static class BetterGizmos
    {
        public static void DrawWireCircle(Vector3 center, Vector3 normal, float radius, int segments = 16)
        {
            var space = Quaternion.LookRotation(normal);
            for (var i = 0; i < segments; i++)
            {
                var a1 = i / (float)segments * Mathf.PI * 2.0f;
                var a2 = (i + 1.0f) / segments * Mathf.PI * 2.0f;

                var p1 = new Vector2(Mathf.Cos(a1), Mathf.Sin(a1));
                var p2 = new Vector2(Mathf.Cos(a2), Mathf.Sin(a2));

                var w1 = center + space * p1 * radius;
                var w2 = center + space * p2 * radius;
                
                Gizmos.DrawLine(w1, w2);
            }
        }
    }
}