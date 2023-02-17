using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToLocal : MonoBehaviour
{
    public GameObject worldPoint;
    public float localX;
    public float localY;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(worldPoint.transform.position, 0.1f);

        Vector2 v = worldPoint.transform.position - transform.position;

        // Compute local coordinates with dot product
        localX = Vector2.Dot(v, transform.right);
        localY = Vector2.Dot(v, transform.up);




    }

}
