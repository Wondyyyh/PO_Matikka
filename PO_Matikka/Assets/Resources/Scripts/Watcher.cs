using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Watcher : MonoBehaviour
{
    public GameObject Car;
    void Update()
    {            
        Vector3 dir = Car.transform.position - transform.position;

        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        // Extract the y component of the rotation quaternion
        float yRot = rot.eulerAngles.y;

        // Set the rotation of the Watcher object to only rotate around the y-axis
        transform.rotation = Quaternion.Euler(0f, yRot, 0f);


        Physics.Raycast(transform.position, Car.transform.position);
        Debug.DrawRay(transform.position, (Car.transform.position - transform.position));

    }
}
