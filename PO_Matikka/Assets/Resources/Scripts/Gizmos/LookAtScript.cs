using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtScript : MonoBehaviour
{
    public GameObject LookAtDirection;
    public GameObject PlayerTarget;
    public float radius = 1f;
    public float scalarDot;

    void OnDrawGizmos()
    {
        Vector2 vecA = LookAtDirection.transform.position;
        Vector2 vecB = PlayerTarget.transform.position;

        float vecAlen = vecA.magnitude;
        float vecBlen = vecB.magnitude;

        Vector2 vecNA = vecA / vecAlen; //normalized vector
        Vector2 vecNB = vecB / vecBlen; //normalized vector

        scalarDot = Vector2.Dot(vecNA, vecNB); // Dot Prod

        ////Draw sphere
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(transform.position, radius);

        //Draw line A
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, vecNA);
        //Draw line B
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, vecNB);

        //Draw player symbol
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(PlayerTarget.transform.position, .05f);
        //Draw player symbol
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(LookAtDirection.transform.position, .05f);

        if(scalarDot > 0.95) { Gizmos.color = Color.green; }
        else Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
