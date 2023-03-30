using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


public class BezierCirclePath : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    public BezierPoint[] points;

    //[Range(0f, 1f)]
    //public float Tvalue = 0.0f;

    private void OnDrawGizmos()
    {
        for(int i = 0; i < points.Length-1; i++) //<-- BEZIER LINE HERE
        {
            Handles.DrawBezier(points[i].Anchor.position,
                points[i + 1].Anchor.position,
                points[i].contorl1.position,
                points[i + 1].contorl0.position,
                Color.magenta, default, 2f);                                                  
        }


        for (float t = 0f; t <= 1f; t += 0.1f) // <-- Jaetaan bezier path t arvoihin 0-1 väleillä 0.1f
        {
            // <-- Ensimmäisen leikkauksen sijainti
            Vector3 tPos = GetBezierPosition(t, points[0], points[1]);
            Vector3 tDir = GetBezierDir(t, points[0], points[1]);
            Quaternion rot = Quaternion.LookRotation(tDir); //Try to get rot

            // <-- Toisen leikkauksen sijainti
            Vector3 tPos1 = GetBezierPosition(t + 0.1f, points[0], points[1]);
            Vector3 tDir1 = GetBezierDir(t + 0.1f, points[0], points[1]);
            Quaternion rot1 = Quaternion.LookRotation(tDir1); //Try to get rot

            //
            // <--- Käydään leikkaukset läpi pareissa jotta voidaan muodostaa viivat leikkauksien välille
            //

            //Draw pos in curve
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(tPos, 0.15f);                   
            //Handles.PositionHandle(tPos, rot);

            Vector3[] verts = new Vector3[road2D.vertices.Length]; // <-- Muutetaan road2D pointit -> Vector3
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] = new Vector3(road2D.vertices[i].point.x, road2D.vertices[i].point.y);
            }
            
            //                               //
            //---   MOST IMPORTANT LOOP   ---//
            //                               //

            // <-- Piirretään poikkileikkauksen pallot, viivat ja 2 leikkauksen väliset viivat --> //
            for (int i = 0; i < road2D.vertices.Length-1; i++)
            {
                Vector3 a = verts[road2D.lineIndices[i]]; //piste A
                Vector3 b = verts[road2D.lineIndices[i + 1]]; //piste B
                Gizmos.color = Color.white;
                Gizmos.DrawLine(tPos + rot * a, tPos + rot * b); // Viiva A -> B

                Vector3 roadPoint = road2D.vertices[i].point;
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(tPos + rot * roadPoint, 0.15f); //Piirrä pallot

                if(t < .9)  // skip last one
                {
                    // <--- piirrä viivat kahden leikkauksen välille ---> //
                    Gizmos.color = Color.grey; 
                    Gizmos.DrawLine(tPos + rot * a, tPos1 + rot1 * a); 
                }
            }
            //---    Loop ends here   ---//
        }
    }

    Vector3 GetBezierPosition(float T, BezierPoint bp1, BezierPoint bp2 )
    {

        //First lerp
        Vector3 PtX = (1 - T) * bp1.Anchor.position + T * bp1.contorl1.position;
        Vector3 PtY = (1 - T) * bp1.contorl1.position + T * bp2.contorl0.position;
        Vector3 PtZ = (1 - T) * bp2.contorl0.position + T * bp2.Anchor.position;  
        //2nd lerp
        Vector3 PtR = (1 - T) * PtX + T * PtY;
        Vector3 PtS = (1 - T) * PtY + T * PtZ;

        return (1-T)*PtR + T*PtS;
    }  
    Vector3 GetBezierDir(float T, BezierPoint bp1, BezierPoint bp2 )
    {

        //First lerp
        Vector3 PtX = (1 - T) * bp1.Anchor.position + T * bp1.contorl1.position;
        Vector3 PtY = (1 - T) * bp1.contorl1.position + T * bp2.contorl0.position;
        Vector3 PtZ = (1 - T) * bp2.contorl0.position + T * bp2.Anchor.position;  
        //2nd lerp
        Vector3 PtR = (1 - T) * PtX + T * PtY;
        Vector3 PtS = (1 - T) * PtY + T * PtZ;

        return (PtS - PtR).normalized;
    }

}


//get the point form bezier curve that 
//Vector3 tPos = GetBezierPosition(Tvalue, points[0], points[1]);
//Vector3 tDir= GetBezierDir(Tvalue, points[0], points[1]);


//Gizmos.color = Color.white;
//Gizmos.DrawSphere(tPos + (rot*Vector3.right), 0.15f);
//Gizmos.DrawSphere(tPos + (rot*Vector3.left), 0.15f);
//Gizmos.DrawSphere(tPos + (rot*Vector3.up), 0.15f);

//Draw dir from pos
//Gizmos.color = Color.blue;
//Gizmos.DrawLine(tPos, tPos + 5 * tDir);