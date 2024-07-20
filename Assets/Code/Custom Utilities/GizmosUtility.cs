using UnityEngine;

namespace CustomUtilities
{
    public static class GizmosUtility
    {
        public static void DrawCircle(Vector3 center, float radius, int segments)
        {
            float angle = 0f;
            for (int i = 0; i <= segments; i++)
            {
                // Calculate x and y coordinates of the point
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                Vector3 point = new Vector3(center.x + x, center.y + y, center.z);

                // Draw a line to the next point
                if (i > 0)
                {
                    Gizmos.DrawLine(point, center + new Vector3(
                        Mathf.Sin(Mathf.Deg2Rad * (angle - 360f / segments)) * radius,
                        Mathf.Cos(Mathf.Deg2Rad * (angle - 360f / segments)) * radius,
                        center.z));
                }

                angle += (360f / segments);
            }
        }
    }
}
