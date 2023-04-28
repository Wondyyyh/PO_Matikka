using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    //[Range(1f, 1000f)]
    //public float Size = 100f;
    //[Range(2, 255)]
    //public int Segments = 10;

    //private Mesh mesh;

    //private void Awake()
    //{
    //    if (mesh == null)
    //    {
    //        mesh = new Mesh();
    //    }

    //    List<Vector3> verts = new List<Vector3>();
    //    List<int> tris = new List<int>();

    //    //Delta between segments
    //    float delta = Size / (float)Segments;

    //    //Generate verts
    //    float x = 0f;
    //    float y = 0f;
    //    for (int seg_x = 0; seg_x <= Segments; seg_x++)
    //    {
    //        x = (float)seg_x * delta;
    //        for (int seg_y = 0; seg_y <= Segments; seg_y++)
    //        {
    //            y = (float)seg_y * delta;
    //            verts.Add(new Vector3(x, 0.0f, y));
    //        }
    //    }

    //    //Tri indices
    //    for (int seg_x = 0; seg_x < Segments; seg_x++)
    //    {
    //        for (int seg_y = 0; seg_y < Segments; seg_y++)
    //        {
    //            int index = seg_x * (Segments + 1) + seg_y;
    //            int index_lower = index + 1;
    //            int index_next_col = index + (Segments + 1);
    //            int index_next_col_lower = index_next_col + 1;

    //            tris.Add(index_lower);
    //            tris.Add(index_next_col);
    //            tris.Add(index);

    //            tris.Add(index_lower);
    //            tris.Add(index_next_col_lower);
    //            tris.Add(index_next_col);


    //        }
    //    }

    //    // Assign the generated verts and tris to the mesh
    //    mesh.vertices = verts.ToArray();
    //    mesh.triangles = tris.ToArray();

    //    // Recalculate the normals of the mesh so that lighting is correct
    //    mesh.RecalculateNormals();

    //    //Create a new MeshRenderer and MeshFilter components
    //    MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
    //    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

    //    // Assign the generated mesh to the MeshFilter component
    //    meshFilter.mesh = mesh;
    [Range(1.0f, 1000.0f)]
    public float Size = 10.0f;
    [Range(2, 255)]
    public int Segments = 5;
    [Range(1, 10)]
    public int NoiseFactor = 3;
    [Range(0, 10)]
    public float Amplitude_First = 3;
    [Range(0, 10)]
    public float Amplitude_Second = 0;
    [Range(0, 10)]
    public float Amplitude_Third = 0;
    [Range(1, 20)]
    public float DivFirst = 10;
    [Range(1, 20)]
    public float DivSecond = 7;
    [Range(1, 20)]
    public float DivThird = 3;

    public bool Flatten = true;

    private Mesh mesh = null;



    private void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        GenerateMesh(Flatten);
    }

    private void GenerateMesh(bool flatten)
    {


        mesh.Clear();
        // vertices
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        // Delta between segments
        float delta = Size / (float)Segments;

        // Generate the vertices
        float x = 0.0f;
        float y = 0.0f;
        for (int seg_x = 0; seg_x <= Segments; seg_x++)
        {
            x = (float)seg_x * delta;
            for (int seg_y = 0; seg_y <= Segments; seg_y++)
            {
                float z1 = (Mathf.PerlinNoise(x / DivFirst, y / DivFirst) - 0.5f) * Amplitude_First * NoiseFactor; // <--- Perlin noiset tänne
                float z2 = (Mathf.PerlinNoise(x / DivSecond, y / DivSecond) - 0.5f) * Amplitude_Second; // <--- Perlin noiset tänne
                float z3 = (Mathf.PerlinNoise(x / DivThird, y / DivThird) - 0.5f) * Amplitude_Third; // <--- Perlin noiset tänne
                float z = z1 + z2 + z3;

                if (flatten && z < 0)
                {
                    z = 0;
                }
                y = (float)seg_y * delta;
                verts.Add(new Vector3(x, z, y));
            }
        }

        // Generate the triangle indices
        for (int seg_x = 0; seg_x < Segments; seg_x++)
        {
            for (int seg_y = 0; seg_y < Segments; seg_y++)
            {
                // Total count of vertices per row & col is: Segments + 1
                int index = seg_x * (Segments + 1) + seg_y;
                int index_lower = index + 1;
                int index_next_col = index + (Segments + 1);
                int index_next_col_lower = index_next_col + 1;

                tris.Add(index);
                tris.Add(index_lower);
                tris.Add(index_next_col);

                tris.Add(index_next_col);
                tris.Add(index_lower);
                tris.Add(index_next_col_lower);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
    }

    private void OnValidate()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        GenerateMesh(Flatten);
    }


}


