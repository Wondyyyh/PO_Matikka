using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    public Transform contorl0;
    public Transform contorl1;

    public Transform Anchor { get { return gameObject.transform; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(Anchor.position, contorl0.position);
        Gizmos.DrawLine(Anchor.position, contorl1.position);

    }


}
