using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnCrossProd : MonoBehaviour
{

    public GameObject Obj;
    RaycastHit hit;
    public Vector3 xAxel;
    public Vector3 yAxel;
    public Vector3 cross_prod;
    public Vector3 SurfaceLookAtROt;
    public Quaternion testi;
    bool is_spawned = false;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_spawned)
        {
            Func();
        }        
    }

    private void Func()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            Debug.Log("Did Hit" + hit.point);
            PlaceObject();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.magenta);            
        }

        xAxel = hit.point + transform.right;
        yAxel = hit.point + hit.normal;    

        // Normalize vectors
        Vector3 xNorm = (xAxel - hit.point).normalized;
        Vector3 yNorm = (yAxel - hit.point).normalized;      

        // Crossproduction vector
        cross_prod = Vector3.Cross(xNorm, yNorm);
    }
    void PlaceObject()
    {
        testi = Quaternion.LookRotation(cross_prod, hit.normal);
        Debug.Log("Rotation set" + testi);
        Instantiate(Obj, hit.point, testi);
        Debug.Log("Instantiated");
    }
}
