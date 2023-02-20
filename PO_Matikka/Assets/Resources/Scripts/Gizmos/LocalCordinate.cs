using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LocalCordinate : MonoBehaviour
{
    public float localX = 0.0F;
    public float localy = 0.0F;
    void OnDrawGizmos()
    {
        // local to world
        Vector2 worldpoint = transform.position + localX * transform.right + localy * transform.up;
        //DrawVector()

        DrawVector(Vector2.zero, Vector2.right, Color.red);
        DrawVector(Vector2.zero, Vector2.up, Color.green);

        DrawVector(transform.position, transform.position + transform.right, Color.red);
        DrawVector(transform.position, transform.position + transform.up, Color.green);

        Debug.Log(transform.localToWorldMatrix.ToString());
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(worldpoint, 0.05f);
        
    }

    private void DrawVector(Vector3 from, Vector3 to, Color c)
    {
        Color curr = Gizmos.color;
        Gizmos.color = c;
        Gizmos.DrawLine(from, to);
        // Compute a location from "to towards from with 30degs"
        Vector3 loc = -(to - from);
        loc = Vector3.ClampMagnitude(loc, 0.1f);
        Quaternion rot30 = Quaternion.Euler(0, 0, 30);
        Vector3 loc1 = rot30 * loc;
        rot30 = Quaternion.Euler(0, 0, -30);
        Vector3 loc2 = rot30 * loc;
        Gizmos.DrawLine(to, to + loc1);
        Gizmos.DrawLine(to, to + loc2);
        Gizmos.color = curr;
    }
}
