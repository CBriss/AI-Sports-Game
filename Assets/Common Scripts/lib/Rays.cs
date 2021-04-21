using UnityEngine;

public static class Rays
{
    public static float SendRay(Vector2 originalPos, Vector2 rayDirection, Color color, int layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(originalPos, rayDirection, Mathf.Infinity, layerMask);
        if (hit.collider != null)
        {
            Debug.DrawLine(originalPos, hit.collider.transform.position, color);
            return hit.distance;
        }
        return 0;
    }
}
