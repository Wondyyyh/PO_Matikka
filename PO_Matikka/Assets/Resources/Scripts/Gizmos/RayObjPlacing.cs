using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayObjPlacing : MonoBehaviour
{

    public GameObject ObjToPlace;
    RaycastHit hit;
    bool found;
    public Vector3 xAxel;
    public Vector3 yAxel;
    public Vector3 cross_prod;
    public Vector3 SurfaceLookAtROt;
    public Quaternion testi;

    private void OnDrawGizmos()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            Debug.Log("Did Hit" + hit.point);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.magenta);
            Debug.Log("Did not Hit");
        }

        xAxel = hit.point + transform.right;
        yAxel= hit.point + hit.normal;
        Debug.Log(xAxel + "  " + yAxel);

        // Normalize vectors
        Vector3 xNorm = (xAxel - hit.point).normalized;
        Vector3 yNorm = (yAxel - hit.point).normalized;

        //Draw axes
        DrawVector(hit.point, xAxel, Color.red);
        DrawVector(hit.point, yAxel, Color.green);

        // Crossproduction vector
        cross_prod = Vector3.Cross(xNorm, yNorm);
        DrawVector(hit.point, hit.point + cross_prod, Color.black);
    }

    void Update()
    {       
        if (Input.GetKeyDown("space"))
        {
            PlaceObject();
        }  
    }

    void PlaceObject()
    {
        testi = Quaternion.LookRotation(cross_prod, hit.normal);
        Debug.Log("Rotation set" + testi);
        Instantiate(ObjToPlace,hit.point, testi);
        Debug.Log("Instantiated");
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
