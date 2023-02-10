using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GizmosScript : MonoBehaviour
{
    public Transform target; //line target

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(0, -3), new Vector2(0, 3));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-3, 0), new Vector2(3, 0));
        Gizmos.color = Color.black;
        Gizmos.DrawLine(new Vector2(0, 0), target.position);

    }

}
