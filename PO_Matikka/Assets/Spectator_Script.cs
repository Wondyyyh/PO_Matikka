using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator_Script : MonoBehaviour
{
    public GameObject CarToFollow;
    public GameObject LookAtDirection;
    Vector2 vecA;
    Vector2 vecB;
    float scalarDot;
    float radius = 90;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        vecA = LookAtDirection.transform.position;
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Vec = CarToFollow.transform.position - transform.position;
        float length = Vec.magnitude;

        if (LookAt() > 0.8 && length < radius)
        {            
            Debug.DrawRay(transform.position, CarToFollow.transform.position - transform.position, Color.magenta);
            cam.enabled = true;
        }
        else cam.enabled = false;
    }
 
    private float LookAt()
    {
        vecB = CarToFollow.transform.position;
        float vecAlen = vecA.magnitude;
        float vecBlen = vecB.magnitude;

        Vector2 vecNA = vecA / vecAlen; //normalized vector
        Vector2 vecNB = vecB / vecBlen; //normalized vector

        scalarDot = Vector2.Dot(vecNA, vecNB); // Dot Prod
        return scalarDot;
    }
}
