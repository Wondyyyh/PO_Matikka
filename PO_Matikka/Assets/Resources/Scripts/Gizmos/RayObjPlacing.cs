using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayObjPlacing : MonoBehaviour
{

    public GameObject ObjToPlace;
    RaycastHit hit;
    bool found;
    Vector3 xAxel;
    Vector3 yAxel;
    Vector3 cross_prod;
    Quaternion SurfaceLookAtROt;

    private void OnDrawGizmos()
    {
        xAxel= hit.point + transform.right;
        yAxel= hit.point + hit.normal;

        // Normalize vectors
        Vector3 xNorm = (xAxel - hit.point).normalized;
        Vector3 yNorm = (yAxel - hit.point).normalized;

        //Draw axes
        DrawVector(hit.point, xAxel, Color.red);
        DrawVector(hit.point, yAxel, Color.green);

        cross_prod = Vector3.Cross(xNorm, yNorm);
        DrawVector(hit.point, hit.point + cross_prod, Color.black);

    }

    void FixedUpdate()
    {
        if (!found)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.Log("Did Hit" + hit.point);
                found = true;
                SurfaceLookAtROt = Quaternion.LookRotation(hit.point,hit.point+cross_prod);
                Debug.Log("Rotation set" + SurfaceLookAtROt);
                PlaceObject();

            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }

          
    }

    void PlaceObject()
    {
        Debug.Log("Instantiated");
        Instantiate(ObjToPlace, hit.point, SurfaceLookAtROt);
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
