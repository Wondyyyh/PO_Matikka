using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereMesh : MonoBehaviour
{
    //Julkiset Slideriarvot
    [Range(3, 255)]
    public int Segments = 8;
    [Range(0, 10)]
    public float innerRadius = 1.0f;
    [Range(1, 10)]
    public float thickness = 1.0f;

    private float OuterRadius => innerRadius + thickness; //Ulkoradius
    private int VertexCount => Segments *2; //Pisteiden m‰‰r‰
    private float TAU = 2 * Mathf.PI; // TAU
    public static Vector2 GetUnitVectorByAngle ( float angRad)
    {
        return new Vector2(
            Mathf.Cos(angRad),
            Mathf.Sin(angRad)
            );
    }
    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        verts.Add(Vector3.zero); //Add the center point of the circle
        //Vector3 v = Vector3.up * Radius;
        //verts.Add(v); //Add the first (zeroeth) vertex, which is just upwards
        for (int i = 0; i < Segments; i++)
        {
            float theta = TAU * i / Segments;
            Debug.Log("Angle: " + theta + ", which in deg is: " + 360 * theta / TAU);
            Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            verts.Add(v * innerRadius);
        }

        mesh.SetVertices(verts);
        List<int> tri_indices = new List<int>();
        for (int i = 0; i < Segments - 1; i++)
        {
            tri_indices.Add(0);
            tri_indices.Add(i + 1);
            tri_indices.Add(i + 2);
        }
        tri_indices.Add(0);
        tri_indices.Add(Segments);
        tri_indices.Add(1);
        mesh.SetTriangles(tri_indices, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    } 
    void GenerateDonut()
    {
        Mesh mesh = new Mesh(); //Luodaan mesh
        int vCount = VertexCount;

        //Listat
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tri_indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>(); // UV LISTA

        for (int i = 0; i < Segments; i++) //Pisteiden luonti
        {
            float t = i / (float)Segments;
            float angRad = t * TAU;
            Vector2 dir = GetUnitVectorByAngle(angRad);


            verts.Add(dir * innerRadius); //inner point
            verts.Add(dir * OuterRadius); //Outer point

            //Awesome UVS
            Vector2 mid = new Vector2(0.5f, 0.5f);
            Vector2 s = dir * 0.5f;
            uvs.Add(mid + s*(innerRadius/OuterRadius)); //Inner UV
            uvs.Add(mid + s); //Outer UV


            //Not great uvs
            //uvs.Add(new Vector2(i / (float)Segments, 0)); //Inner UV
            //uvs.Add(new Vector2(i / (float)Segments, 1)); //Outer UV

            normals.Add(Vector3.forward); //Normaalit
            normals.Add(Vector3.forward);
        }

        for (int i = 0; i < Segments; i++) // Kolmioden luonti
        {
            int Root = 2 * i;
            int innerRoot = Root+ 1;
            int outerNext = (Root+ 2) % vCount;
            int innerNext = (Root+ 3) % vCount;

            tri_indices.Add(Root);
            tri_indices.Add(outerNext);
            tri_indices.Add(innerNext);


            tri_indices.Add(Root);
            tri_indices.Add(innerNext);
            tri_indices.Add(innerRoot);
        }

        //Setataan luodut kolmiot ja pisteet meshille
        mesh.SetVertices(verts);
        mesh.SetTriangles(tri_indices, 0);
        mesh.SetNormals(normals);

        //UV
        mesh.SetUVs(0, uvs);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    void Start()
    {
        GenerateDonut();
    }
    private void OnValidate()
    {
        GenerateDonut();
    }
}