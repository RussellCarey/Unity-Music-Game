using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public Transform mover;
    private Vector2 start;
    private Vector2 end;
    public Transform startOb;
    public Transform endOb;
    public float radius;
    public float duration;

    public float angleStart;
    public float angleEnd;
    public float travelDistance;


    // Start is called before the first frame update
    void Start()
    {
        start.x = startOb.position.x;
        start.y = startOb.position.y;
        end.x = endOb.position.x;
        end.y = endOb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FollowArc());

    }

    IEnumerator FollowArc()
    {

        Vector2 difference = end - start;
        float span = difference.magnitude;

        // Override the radius if it's too small to bridge the points.
        float absRadius = Mathf.Abs(radius);
        if (span > 2f * absRadius)
            radius = absRadius = span / 2f;

        Vector2 perpendicular = new Vector2(difference.y, -difference.x) / span;
        perpendicular *= Mathf.Sign(radius) * Mathf.Sqrt(radius * radius - span * span / 4f);

        Vector2 center = start + difference / 2f + perpendicular;

        Vector2 toStart = start - center;
        float startAngle = Mathf.Atan2(toStart.y, toStart.x);

        Vector2 toEnd = end - center;
        float endAngle = Mathf.Atan2(toEnd.y, toEnd.x);

        // Choose the smaller of two angles separating the start & end
        float travel = (endAngle - startAngle + 5f * Mathf.PI) % (2f * Mathf.PI) - Mathf.PI;

        float progress = 0f;
        do
        {
            float angle = startAngle + progress * travel;
            mover.position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * absRadius;
            progress += Time.deltaTime / duration;

            angleStart = startAngle;
            angleEnd = endAngle;
            travelDistance = travel;

            yield return null;
        } while (progress < 1f);

        mover.position = end;

        angleStart = startAngle;
        travelDistance = travel;

    }
}
