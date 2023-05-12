using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;
using System.Runtime.CompilerServices;

public class CarRoad : MonoBehaviour
{
    [SerializeField]
    Mesh2D road2D;

    [Range(0.0f, 0.833f)]
    public float TValue = 0.0f;
    //public float CarPosT;

    [Range(2, 100)]
    public int Segments = 2;

    public BezierPoint[] points; //For storing all bezier points

    [SerializeField]
    private Mesh mesh;

    private List<Vector3> verts = new List<Vector3>(); // vertices
    private List<Vector2> uvs = new List<Vector2>(); // uvs
    private List<int> tri_indices = new List<int>();

    public void MoveCar(float CarPosT, GameObject Car)
    {
        //Debug.Log(CarPosT);
        Matrix4x4 carPos = GetPositionFromBezierPath(CarPosT);
        Car.transform.position = carPos.GetPosition();

        Vector3 carDir = GetPositionFromBezierPath(CarPosT + 0.001f).GetPosition() - Car.transform.position;
        //Vector3 carDir = GetPositionFromBezierPath(CarPosT).GetPosition();
        Quaternion carRot = Quaternion.LookRotation(carDir, Vector3.up);
        Car.transform.rotation = carRot;
    }

    private void OnDrawGizmosSelected() //Bezier PATH
    {


        for (int i = 0; i < points.Length - 1; i++)
        {
            Handles.DrawBezier(points[i].Anchor.position,
                               points[i + 1].Anchor.position,
                               points[i].contorl1.position,
                               points[i + 1].contorl0.position,
                               Color.magenta, default, 2f);
        }

        // Get the point from bezier curve that corresponds our t-value
        Vector3 tPos = GetBezierPosition(TValue, points[0], points[1]);
        Vector3 tDir = GetBezierDirection(TValue, points[0], points[1]);

        //Draw the position on the curve
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(tPos, 0.3f);

        //Try to get the roation
        Quaternion rot = Quaternion.LookRotation(tDir);
        //Handles.PositionHandle(tPos, rot);

        // Draw all parts of the bezier path
        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawBezierPart(points[i], points[i + 1]);
        }
    }

    private void DrawBezierPart(BezierPoint point0, BezierPoint point1)
    {
        Vector3 tPos, tDir;
        Quaternion rot;

        //DRAW one road peace
        for (int n = 0; n < Segments; n++)
        {
            float tTest = n / (float)Segments;
            // Get the point from bezier curve that corresponds our t-value
            tPos = GetBezierPosition(tTest, point0, point1);
            tDir = GetBezierDirection(tTest, point0, point1);
            rot = Quaternion.LookRotation(tDir);

            float tTestNext = (n + 1) / (float)Segments;
            // Get the point from bezier curve that corresponds our t-value
            Vector3 tPosNext = GetBezierPosition(tTestNext, point0, point1);
            Vector3 tDirNext = GetBezierDirection(tTestNext, point0, point1);
            Quaternion rotNext = Quaternion.LookRotation(tDirNext);

            for (int i = 0; i < road2D.vertices.Length; i++)
            {
                Vector3 roadpoint = road2D.vertices[i].point;
                //Gizmos.color = Color.blue;
                //Gizmos.DrawSphere(tPos + rot * roadpoint, 0.15f);
                //Gizmos.DrawSphere(tPosNext + rotNext * roadpoint, 0.15f);
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

    private void Awake()
    {
        GenerateMesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        if (mesh == null) { mesh = new Mesh(); }
    }

    void GenerateMesh()
    {
        this.verts.Clear();
        this.uvs.Clear();
        this.tri_indices.Clear();

        // Draw all parts of the bezier MESH
        for (int i = 0; i < points.Length - 1; i++)
        {
            GenerateVerticesForBezierPart(points[i], points[i + 1]);
            GenerateTringlesForBEzierPart(i);
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

    private void GenerateVerticesForBezierPart(BezierPoint point0, BezierPoint point1) //MESH Part
    {
        //Go through each segment // <--- DRAW ONE MESH PEACE HERE
        for (int n = 0; n <= Segments; n++)
        {
            // Compute the t-value for current segment
            float t = n / (float)Segments;

            // Get the point from bezier curve that corresponds our t-value
            Vector3 tPos = GetBezierPosition(t, point0, point1);
            Vector3 tDir = GetBezierDirection(t, point0, point1);
            Quaternion rot = Quaternion.LookRotation(tDir);

            //Loop through ROAD SLICE
            for (int index = 0; index < road2D.vertices.Length; index++)
            {
                Vector3 roadpoint = road2D.vertices[index].point; //Local point
                Vector3 worldpoint = tPos + rot * roadpoint; //Local to world-transform

                verts.Add(worldpoint); //Add this world point to our verts                   
                uvs.Add(new Vector2(roadpoint.x / 10.0f + 0.5f, t)); //Add the corresponding UV-coord - hack hack

            }
        }
        // triangles //
        //How many lines
        int num_lines = road2D.lineIndices.Length / 2;
    }

    private void GenerateTringlesForBEzierPart(int part)
    {
        int num_lines = road2D.lineIndices.Length / 2;

        // Siirtymä seuraavan palan kolmiohin
        int offset = part * (Segments + 1) * road2D.vertices.Length;
        //Go through each but the last segment
        for (int n = 0; n < Segments; n++)
        {
            for (int line = 0; line < num_lines; line++)
            {
                // current "slice"
                int curr_first = offset + n * road2D.vertices.Length +
                                  road2D.lineIndices[2 * line];

                int curr_second = offset + n * road2D.vertices.Length +
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
    }


    Matrix4x4 GetPositionFromBezierPath(float t) // <--- Autolle positio tässä
    {
        int first_index;
        first_index = (int)(t * points.Length) % points.Length;

        // tvalue for corresponding path part
        float part_in_t = -1f;
        float new_t_value;
        part_in_t = (1f / (float)points.Length); // one part in amount of t
        new_t_value = (t - first_index * part_in_t) / (part_in_t);

        Vector3 pos;
        Vector3 dir;

        pos = GetBezierPosition(new_t_value, points[first_index], points[first_index + 1]);
        dir = GetBezierDirection(new_t_value, points[first_index], points[first_index + 1]);

        Quaternion rot = Quaternion.LookRotation(pos, dir);

        Matrix4x4 mat = Matrix4x4.identity;
        mat.SetTRS(pos, rot, Vector3.one);
        return mat;
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

        return (1 - t) * PtR + t * PtS; // 3rd lerp
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

        return (PtS - PtR).normalized; //Compute direction vector
    }

}
