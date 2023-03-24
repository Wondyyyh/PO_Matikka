using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BezierCirclePath : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    public BezierPoint[] points;

    [Range(0f, 1f)]
    public float Tvalue = 0.0f;

    private void OnDrawGizmos()
    {
        for(int i = 0; i < points.Length-1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                points[i + 1].Anchor.position,
                points[i].contorl1.position,
                points[i + 1].contorl0.position,
                Color.magenta, default, 2f);                                                  
        }
        //get the point form bezier curve that 
        Vector3 tPos= GetBezierPosition(Tvalue, points[0], points[1]);
        Vector3 tDir= GetBezierDir(Tvalue, points[0], points[1]);

        //Draw pos in curve
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(tPos, 0.15f);

        //Draw dir from pos
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(tPos, tPos + 5 * tDir);

        //Try to get rot
        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot);


        //Draw some harjotuspisteet
        for(int i= 0; i < road2D.vertices.Length; i++)
        {
            Vector3 roadPoint = road2D.vertices[i].point;
            Gizmos.DrawSphere(tPos + rot * roadPoint, 0.15f);
        }
        //Gizmos.color = Color.white;
        //Gizmos.DrawSphere(tPos + (rot*Vector3.right), 0.15f);
        //Gizmos.DrawSphere(tPos + (rot*Vector3.left), 0.15f);
        //Gizmos.DrawSphere(tPos + (rot*Vector3.up), 0.15f);

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
