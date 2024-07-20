using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour, IAnimatable
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed = 0.01f;

    private bool _canAnimate = false;
    private float t = 0f;

    private void Update()
    {
        if (_canAnimate == true)
        {
            t += Time.deltaTime * speed;

            if (t > 1)
            {
                t = 1;
                _canAnimate = false;
            }

            int segmentCount = (points.Length - 1) / 2;
            float segmentT = t * segmentCount;

            int segmentIndex = Mathf.Min((int)segmentT, segmentCount - 1);
            segmentT -= segmentIndex;

            transform.position = GetQuadraticBezierPoint(segmentT, points[segmentIndex * 2].position, points[segmentIndex * 2 + 1].position, points[segmentIndex * 2 + 2].position);
        }
    }

    private void OnDrawGizmos()
    {
        if (points != null && points.Length >= 3)
        {
            for (int i = 0; i < points.Length - 2; i += 2)
            {
                if (points[i] != null && points[i + 1] != null && points[i + 2] != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                    Gizmos.DrawLine(points[i + 1].position, points[i + 2].position);

                    Gizmos.color = Color.red;
                    for (float t = 0; t <= 1; t += 0.05f)
                    {
                        Gizmos.DrawLine(
                            GetQuadraticBezierPoint(t, points[i].position, points[i + 1].position, points[i + 2].position),
                            GetQuadraticBezierPoint(t + 0.05f, points[i].position, points[i + 1].position, points[i + 2].position)
                        );
                    }
                }
            }
        }
    }

    public void ExecuteAnimation(float delay = 0)
    {
        _canAnimate = true;
    }

    public bool AnimationFinished()
    {
        return t == 1;
    }

    private Vector3 GetQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}