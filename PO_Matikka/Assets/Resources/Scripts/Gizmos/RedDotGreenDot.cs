using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RedDotGreenDot : MonoBehaviour
{
    public Transform target; //line target
    public float radius = 1;
    public float length = 0;

    void OnDrawGizmos()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector2 Vec = target.position - transform.position;
        length = Vec.magnitude;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, target.position);

        if (length > radius ) { Gizmos.color = Color.green; }
        else Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position, 0.2f);
    }

}
