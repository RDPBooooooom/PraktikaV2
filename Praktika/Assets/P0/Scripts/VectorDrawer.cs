using UnityEngine;

namespace P0.Scripts
{
    public static class VectorDrawer
    {
        public static void DrawVector(Vector3 position, Vector3 vector, Color color)
        {
            Color before = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawRay(position, vector);
            Gizmos.color = before;
        }

        public static void DrawVector(Vector3 position, Vector3 unitVector, float lenght, Color color)
        {
            DrawVector(position, unitVector.normalized * lenght, color);
        }
    }
}