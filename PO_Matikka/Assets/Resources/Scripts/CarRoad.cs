using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Unity.VisualScripting;

public class CarRoad : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    [Range(0.0f, 1.0f)]
    public float TValue = 0.0f;

    [Range(2, 100)]
    public int Segments = 2;

    public BezierPoint[] points; //For storing all bezier points

    [SerializeField]
    private Mesh mesh = new Mesh();

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i + 1].Anchor.position,
                               points[i].contorl1.position,
                               points[i + 1].contorl0.position,
                               Color.magenta, default, 2f);
        }

        //Last part of the path from last bezier point to the first bezier point
        //Handles.DrawBezier(points[points.Length - 1].Anchor.position,
        //                       points[0].Anchor.position,
        //                       points[points.Length - 1].control1.position,
        //                       points[0].control0.position,
        //                       Color.magenta, default, 2f);


        // Get the point from bezier curve that corresponds our t-value
        Vector3 tPos = GetBezierPosition(TValue, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(TValue, points[0], points[1]);

        //Draw the position on the curve
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(tPos, 0.3f);

        //Draw the direction at the position
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(tPos, tPos + 5.0f * tDir);

        //Try to get the roation
        Quaternion rot = Quaternion.LookRotation(tDir);
        Handles.PositionHandle(tPos, rot);


        for (int z = 0; z < points.Length; z++)
        {

            int zNext = (z + 1) % points.Length; 
            // Get the index of the next point, wrapping around to the first point for the last point

            //Draw some points wrt t-position
            for (int n = 0; n < Segments; n++)
            {
                float tTest = n / (float)Segments;
                // Get the point from bezier curve that corresponds our t-value
                tPos = GetBezierPosition(tTest, points[z], points[zNext]);
                tDir = GetBezierDirection(tTest, points[z], points[zNext]);
                rot = Quaternion.LookRotation(tDir);

                float tTestNext = (n + 1) / (float)Segments;
                // Get the point from bezier curve that corresponds our t-value
                Vector3 tPosNext = GetBezierPosition(tTestNext, points[z], points[zNext]);
                Vector3 tDirNext = GetBezierDirection(tTestNext, points[z], points[zNext]);
                Quaternion rotNext = Quaternion.LookRotation(tDirNext);

                for (int i = 0; i < road2D.vertices.Length; i++)
                {
                    Vector3 roadpoint = road2D.vertices[i].point;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(tPos + rot * roadpoint, 0.15f);
                    Gizmos.DrawSphere(tPosNext + rotNext * roadpoint, 0.15f);
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(tPos + rot * roadpoint, tPosNext + rotNext * roadpoint);
                }

                for (int i = 0; i < road2D.vertices.Length - 1; i++)
                {
                    Vector3 roadpoint = road2D.vertices[i].point;
                    Vector3 roadpointNext = road2D.vertices[i + 1].point;

                    Gizmos.DrawLine(tPos + rot * roadpoint, tPos + rot * roadpointNext);
                }
                Vector3 pointLast = road2D.vertices[road2D.vertices.Length - 1].point;
                Vector3 pointFirst = road2D.vertices[0].point;

                Gizmos.DrawLine(tPos + rot * pointLast, tPos + rot * pointFirst);

            }
        }

        //Gizmos.DrawSphere(tPos + (rot * Vector3.right), 0.3f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.left), 0.3f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.up), 0.3f);
        //Gizmos.DrawSphere(tPos + (rot * Vector3.up * 2.0f), 0.3f);

    }

    private void Awake()
    {
        GenerateMesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void GenerateMesh()
    {
        // vertices //
        List<Vector3> verts = new List<Vector3>();
        // uvs //
        List<Vector2> uvs = new List<Vector2>();

        for (int z = 0; z < points.Length - 1; z++)
        {

            int zNext = (z + 1) % points.Length;
            //Go through each segment
            for (int n = 0; n <= Segments; n++)
            {
                // Compute the t-value for current segment
                float t = n / (float)Segments;

                // Get the point from bezier curve that corresponds our t-value
                Vector3 tPos = GetBezierPosition(t, points[z], points[zNext]);
                Vector3 tDir = GetBezierDirection(t, points[z], points[zNext]);
                Quaternion rot = Quaternion.LookRotation(tDir);

                //Loop through our road slice
                for (int index = 0; index < road2D.vertices.Length; index++)
                {
                    //Local point
                    Vector3 roadpoint = road2D.vertices[index].point;
                    //Local to world-transform
                    Vector3 worldpoint = tPos + rot * roadpoint;
                    //Add this world point to our verts
                    verts.Add(worldpoint);
                    //Add the corresponding UV-coord - hack hack
                    uvs.Add(new Vector2(roadpoint.x / 10.0f + 0.5f, t));

                }

            }
        }
        // triangles //

        //How many lines
        int num_lines = road2D.lineIndices.Length / 2;
        List<int> tri_indices = new List<int>();

        //Go through each but the last segment
        for (int n = 0; n < Segments; n++)
        {
            for (int line = 0; line < num_lines; line++)
            {
                // current "slice"
                int curr_first = n * road2D.vertices.Length +
                                  road2D.lineIndices[2 * line];

                int curr_second = n * road2D.vertices.Length +
                                 road2D.lineIndices[2 * line + 1];

                // next "slice"
                int next_first = curr_first + road2D.vertices.Length;
                int next_second = curr_second + road2D.vertices.Length;

                // First triangle
                tri_indices.Add(curr_first);
                tri_indices.Add(next_first);
                tri_indices.Add(curr_second);

                //SEcond triangle
                tri_indices.Add(curr_second);
                tri_indices.Add(next_first);
                tri_indices.Add(next_second);

            }
        }

        // normals //

        //Clear the mesh
        if (this.mesh != null)
            this.mesh.Clear();
        else
            this.mesh = new Mesh();

        // Set everything
        this.mesh.SetVertices(verts);
        this.mesh.SetUVs(0, uvs);
        this.mesh.SetTriangles(tri_indices, 0);
        this.mesh.RecalculateNormals();

    }

    Vector3 GetBezierPosition(float t, BezierPoint bp1, BezierPoint bp2)
    {
        // 1st Lerp
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.contorl1.position;
        Vector3 PtY = (1 - t) * bp1.contorl1.position + t * bp2.contorl0.position;
        Vector3 PtZ = (1 - t) * bp2.contorl0.position + t * bp2.Anchor.position;

        // 2nd Lerp
        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        return (1 - t) * PtR + t * PtS;
    }

    Vector3 GetBezierDirection(float t, BezierPoint bp1, BezierPoint bp2)
    {
        // 1st Lerp
        Vector3 PtX = (1 - t) * bp1.Anchor.position + t * bp1.contorl1.position;
        Vector3 PtY = (1 - t) * bp1.contorl1.position + t * bp2.contorl0.position;
        Vector3 PtZ = (1 - t) * bp2.contorl0.position + t * bp2.Anchor.position;

        // 2nd Lerp
        Vector3 PtR = (1 - t) * PtX + t * PtY;
        Vector3 PtS = (1 - t) * PtY + t * PtZ;

        return (PtS - PtR).normalized;
    }


    //Vanha koodi
    /*
    public bezierPoint BezierP1;
    public bezierPoint BezierP2;
    public bezierPoint BezierP3;
    public bezierPoint BezierP4;

    private void OnDrawGizmos()
    {

        Handles.DrawBezier(BezierP1.gameObject.transform.position, 
                           BezierP2.gameObject.transform.position,
                           BezierP1.control1.position,
                           BezierP2.control0.position,
                           Color.magenta, default, 1f);

        Handles.DrawBezier(BezierP2.gameObject.transform.position,
                           BezierP3.gameObject.transform.position,
                           BezierP2.control1.position,
                           BezierP3.control0.position,
                           Color.magenta, default, 1f);

        Handles.DrawBezier(BezierP3.gameObject.transform.position,
                           BezierP4.gameObject.transform.position,
                           BezierP3.control1.position,
                           BezierP4.control0.position,
                           Color.magenta, default, 1f);

        Handles.DrawBezier(BezierP4.gameObject.transform.position,
                           BezierP1.gameObject.transform.position,
                           BezierP4.control1.position,
                           BezierP1.control0.position,
                           Color.magenta, default, 1f);



    }
    */
}