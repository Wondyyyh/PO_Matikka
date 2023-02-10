using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Vectors : MonoBehaviour
{

    public GameObject a;
    public GameObject b;
    public float scalarDot;
    public float axis_lenght = 2.0f;

   private void OnDrawGizmos() 
   {
        Vector2 vecA = a.transform.position;
        Vector2 vecB = b.transform.position;

        float vecAlen = vecA.magnitude;
        float vecBlen = vecB.magnitude;

        Vector2 vecNA = vecA / vecAlen; //normalized vector
        Vector2 vecNB = vecB / vecBlen; //normalized vector

        scalarDot = Vector2.Dot(vecNA,vecNB); // Dot Prod

        Vector2 vecProj = vecNA * scalarDot; //Proje
        // Vector2 vecProj1 = vecNB * scalarDot; //testi

        // VECTOR A
        DrawVector(new Vector3(0,0,0), a.transform.position, Color.black);
        Gizmos.color = Color.black;
         // VECTOR B
        DrawVector(new Vector3(0,0,0), b.transform.position, Color.black);
        Gizmos.color = Color.black;

        //Draw normalized vector (from A)
        DrawVector(new Vector3(0, 0, 0), vecNA, Color.blue);
        //Draw normalized vector (from B)
        DrawVector(new Vector3(0, 0, 0), vecNB, Color.blue);


       
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
