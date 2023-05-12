using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class RadialTrigger : MonoBehaviour
{
    GameObject otherCar;
    private float radius = 7;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        otherCar = GameObject.Find("Auto2");
        rend = gameObject.GetComponentInChildren<Renderer>();
    }
    void Update()
    {
        Vector3 Vec = otherCar.transform.position - transform.position;
        float length = Vec.magnitude;

        if( length < radius )
        {
            rend.material.color = Color.red;
        }
        else rend.material.color = Color.white;
    }
}
