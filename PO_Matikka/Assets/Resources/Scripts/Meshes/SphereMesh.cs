using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SphereMesh : MonoBehaviour
{
    [Range(3, 255)]
    public int N = 8;
    [Range(1, 10)]
    public float Radius = 1.0f;
    public float OuterRadius = 3.0f;

    private float TAU = 2 * Mathf.PI;
    void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        verts.Add(Vector3.zero); //Add the center point of the circle
        //Vector3 v = Vector3.up * Radius;
        //verts.Add(v); //Add the first (zeroeth) vertex, which is just upwards
        for (int i = 0; i < N; i++)
        {
            float theta = TAU * i / N;
            Debug.Log("Angle: " + theta + ", which in deg is: " + 360 * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            verts.Add(v * Radius);
        }

        mesh.SetVertices(verts);
        List<int> tri_indices = new List<int>();
        for (int i = 0; i < N - 1; i++)
        {
            tri_indices.Add(0);
            tri_indices.Add(i + 1);
            tri_indices.Add(i + 2);
        }
        tri_indices.Add(0);
        tri_indices.Add(N);
        tri_indices.Add(1);
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    } 
    void GenerateDonut()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        verts.Add(Vector3.zero); //Add the center point of the circle
        //Vector3 v = Vector3.up * Radius;
        //verts.Add(v); //Add the first (zeroeth) vertex, which is just upwards
        for (int i = 0; i < N; i++)
        {
            float theta = TAU * i / N;
            Debug.Log("Angle: " + theta + ", which in deg is: " + 360 * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            verts.Add(v * Radius);
            verts.Add(v * OuterRadius);
        }

        mesh.SetVertices(verts);
        List<int> tri_indices = new List<int>();
        for (int i = 0; i < N - 1; i++)
        {
            int innerfirst = 2 * i;
            int outerfirst = innerfirst /1;
            int innersecond = outerfirst /1;

            tri_indices.Add(0);
            tri_indices.Add(i + 1);
            tri_indices.Add(i + 2);
        }
        tri_indices.Add(0);
        tri_indices.Add(N);
        tri_indices.Add(1);
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateMesh();

    }
    private void OnValidate()
    {
        GenerateMesh();
    }
    // Update is called once per frame
    void Update()
    {

    }
}